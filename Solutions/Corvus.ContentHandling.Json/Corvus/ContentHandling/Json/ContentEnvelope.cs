// <copyright file="ContentEnvelope.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// An envelope for content that could be of various different types, which
    /// do not have to share a common base.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Frequently, you need to send heterogenous message types down a single pipe, and dispatch them
    /// at the receiver. You often need common properties which help with dispatch, but which are unrelated to the actual
    /// message type itself (e.g. routing or tracing information).
    /// </para>
    /// <para>
    /// The <see cref="ContentEnvelope"/> enables you to wrap an entity in an envelope, and decorate it with a <see cref="PayloadContentType"/>
    /// to discriminate the actual type of the payload.
    /// </para>
    /// <para>
    /// The types in the payload can follow Endjin's standard <see cref="ContentFactory"/> content
    /// model, or you can specify your own type discriminator when you set it with <see cref="SetPayload{T}(T, string)"/>.
    /// </para>
    /// <para>
    /// When you want to get at the payload, you can inspect the <see cref="PayloadContentType"/> and then <see cref="TryGetPayload{T}(out T)"/>
    /// as a particular type.
    /// </para>
    /// <para>
    /// Because this match-then-dispatch pattern is so common, you can use any of the <see cref="Match{T1, T2}(Action{T1}, Action{T2})"/> overloads
    /// to do this. If your type conforms to the <see cref="ContentFactory"/> content type pattern, then you do not need to specify a content type to match (it
    /// will derive it from the specified type). If you manually specified a discriminator, then you specify the content type and match action as a tuple."/>).
    /// There are both synchronous and asynchronous versions of this pattern, supporting up to 5 different types.
    /// </para>
    /// <para>
    /// For larger numbers of types, we support a handler dispatch pattern, using <see cref="IContentHandler{T}"/> interface (or <see cref="IAsyncContentHandler{T}"/> for async dispatch.
    /// </para>
    /// </remarks>
    /// <example>
    /// var sendingContentEnvelope = new ContentEnvelope();
    ///
    /// // Set create an instance which supports the ContentFactory pattern
    /// var itemWithContentType = new ExampleItemWithContentType1();
    ///
    /// sendingContentEnvelope.SetPayload(itemWithContentType);
    ///
    /// // dispatch the envelope somewhere... e.g. serialized into a message queue
    ///
    /// // ...then receive it at the other end and deserialize the envelope
    ///
    /// ContentEnvelope receivedContentEnvelope;
    ///
    /// // Dispatch it to the functions that handle each of the content types
    /// receivedContentEnvelope.Match(
    ///     m1 => DoSomethingWithExampleItemWithContentType1(m1),
    ///     m2 => DoSomethingWithExampleItemWithContentType2(m2));.
    ///
    /// </example>
    public class ContentEnvelope
    {
        /// <summary>
        /// The default fallback json serializer settings.
        /// </summary>
        public static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentEnvelope"/> class.
        /// </summary>
        public ContentEnvelope()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentEnvelope"/> class.
        /// </summary>
        /// <param name="jsonSerializerSettings">The json serializer settings to use for the ContentEnvelope.</param>
        public ContentEnvelope(JsonSerializerSettings jsonSerializerSettings)
        {
            this.SerializerSettings = jsonSerializerSettings ?? JsonConvert.DefaultSettings?.Invoke() ?? DefaultJsonSerializerSettings;
        }

        private ContentEnvelope(JToken payload, string contentType, JsonSerializerSettings settings = null)
        {
            this.SerializedPayload = payload;
            this.PayloadContentType = contentType;
            this.SerializerSettings = settings ?? JsonConvert.DefaultSettings?.Invoke() ?? DefaultJsonSerializerSettings;
        }

        /// <summary>
        /// Gets the content type of the payload.
        /// </summary>
        public string PayloadContentType { get; private set; }

        /// <summary>
        /// Gets the <see cref="JsonSerializerSettings"/> to use for the content envelope.
        /// </summary>
        [JsonIgnore]
        public JsonSerializerSettings SerializerSettings { get; }

        /// <summary>
        /// Gets the serialized representation of the payload.
        /// </summary>
        [JsonProperty]
        internal JToken SerializedPayload { get; private set; }

        /// <summary>
        /// Construct a content envelope from a payload.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <param name="contentType">The content type of the payload.</param>
        /// <param name="settings">The serializer settings to use.</param>
        /// <returns>An instance of a content envelope initialized with the given payload.</returns>
        public static ContentEnvelope FromPayload<T>(T payload, string contentType = null, JsonSerializerSettings settings = null)
        {
            var result = new ContentEnvelope(settings);
            result.SetPayload(payload, contentType);
            return result;
        }

        /// <summary>
        /// Construct a content envelope from a json string.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="settings">The serializer settings to use.</param>
        /// <returns>The content envelope for the json.</returns>
        /// <remarks>
        /// If the root json entity contains a <c>contentType</c> property,
        /// you do not need to specify the <paramref name="contentType"/>, it
        /// will be automatically retrieved from the JObject.
        /// </remarks>
        public static ContentEnvelope FromJson(string jsonString, string contentType = null, JsonSerializerSettings settings = null)
        {
            if (jsonString is null)
            {
                throw new ArgumentNullException(nameof(jsonString));
            }

            var json = JToken.Parse(jsonString);

            return FromJson(json, contentType, settings);
        }

        /// <summary>
        /// Construct a content envelope from a json entity.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="settings">The serializer settings to use.</param>
        /// <returns>The content envelope for the json.</returns>
        /// <remarks>
        /// If the root json entity contains a <c>contentType</c> property,
        /// you do not need to specify the <paramref name="contentType"/>, it
        /// will be automatically retrieved from the JObject.
        /// </remarks>
        public static ContentEnvelope FromJson(JToken json, string contentType = null, JsonSerializerSettings settings = null)
        {
            if (json is null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                contentType = (string)json["contentType"];
            }

            return new ContentEnvelope(json, contentType, settings);
        }

        /// <summary>
        /// Construct a content envelope from a json entity.
        /// </summary>
        /// <param name="stream">The json text stream.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>The content envelope for the json.</returns>
        /// <remarks>
        /// If the root json entity contains a <c>contentType</c> property,
        /// you do not need to specify the <paramref name="contentType"/>, it
        /// will be automatically retrieved from the JObject.
        /// </remarks>
        public static ContentEnvelope FromJson(Stream stream, string contentType = null)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using var reader = new JsonTextReader(new StreamReader(stream));
            var json = JToken.Load(reader);
            return FromJson(json, contentType);
        }

        /// <summary>
        /// Construct a content envelope from a json entity.
        /// </summary>
        /// <param name="stream">The json text stream.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>The content envelope for the json.</returns>
        /// <remarks>
        /// If the root json entity contains a <c>contentType</c> property,
        /// you do not need to specify the <paramref name="contentType"/>, it
        /// will be automatically retrieved from the JObject.
        /// </remarks>
        public static async Task<ContentEnvelope> FromJsonAsync(Stream stream, string contentType = null)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using var reader = new JsonTextReader(new StreamReader(stream));
            JToken json = await JToken.LoadAsync(reader).ConfigureAwait(false);
            return FromJson(json, contentType);
        }

        /// <summary>
        /// Sets the payload in the envelope.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="payload">The payload to set.</param>
        /// <param name="payloadContentType">The content type of the payload. You do not need to set this if the payload conforms to the <see cref="ContentFactory"/> pattern.</param>
        public void SetPayload<T>(T payload, string payloadContentType = null)
        {
            if (string.IsNullOrEmpty(payloadContentType))
            {
                this.PayloadContentType = ContentFactory.GetContentType<T>();
            }
            else
            {
                this.PayloadContentType = payloadContentType;
            }

            this.SerializedPayload = JObject.FromObject(payload, JsonSerializer.Create(this.SerializerSettings));
        }

        /// <summary>
        /// Get the payload as the specified type.
        /// </summary>
        /// <typeparam name="T">The type of payload to retrieve.</typeparam>
        /// <returns>An instance of the type.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the payload is not accessible through the given type.</exception>
        public T GetContents<T>()
        {
            if (this.TryGetPayload(out T result))
            {
                return result;
            }

            throw new InvalidOperationException($"The payload of the message is not accessible through the type {typeof(T).FullName}");
        }

        /// <summary>
        /// Try to get the payload as the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="result">The payload, as the specified type.</param>
        /// <returns>True if the payload was available as the specified type.</returns>
        public bool TryGetPayload<T>(out T result)
        {
            if (this.SerializedPayload == null)
            {
                result = default;
                return true;
            }

            using JsonReader reader = this.SerializedPayload.CreateReader();
            try
            {
                result = JsonSerializer.Create(this.SerializerSettings).Deserialize<T>(reader);
                return true;
            }
            catch (JsonSerializationException)
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public async Task<bool> MatchAsync<T1, T2>((string contentType, Func<T1, Task> match) match1, (string contentType, Func<T2, Task> match) match2)
        {
            if (match1.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match1));
            }

            if (match1.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match1));
            }

            if (match2.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match2));
            }

            if (match2.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match2));
            }

            if (this.PayloadContentType == match1.contentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    await match1.match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match2.contentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    await match2.match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <typeparam name="T3">The type of the third match.</typeparam>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <param name="match3">The third match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public async Task<bool> MatchAsync<T1, T2, T3>((string contentType, Func<T1, Task> match) match1, (string contentType, Func<T2, Task> match) match2, (string contentType, Func<T3, Task> match) match3)
        {
            if (match1.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match1));
            }

            if (match1.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match1));
            }

            if (match2.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match2));
            }

            if (match2.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match2));
            }

            if (match3.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match3));
            }

            if (match3.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match3));
            }

            if (this.PayloadContentType == match1.contentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    await match1.match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match2.contentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    await match2.match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match3.contentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    await match3.match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <typeparam name="T3">The type of the third match.</typeparam>
        /// <typeparam name="T4">The type of the fourth match.</typeparam>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <param name="match3">The third match.</param>
        /// <param name="match4">The fourth match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public async Task<bool> MatchAsync<T1, T2, T3, T4>((string contentType, Func<T1, Task> match) match1, (string contentType, Func<T2, Task> match) match2, (string contentType, Func<T3, Task> match) match3, (string contentType, Func<T4, Task> match) match4)
        {
            if (match1.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match1));
            }

            if (match1.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match1));
            }

            if (match2.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match2));
            }

            if (match2.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match2));
            }

            if (match3.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match3));
            }

            if (match3.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match3));
            }

            if (match4.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match4));
            }

            if (match4.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match4));
            }

            if (this.PayloadContentType == match1.contentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    await match1.match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match2.contentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    await match2.match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match3.contentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    await match3.match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match4.contentType)
            {
                if (this.TryGetPayload(out T4 payload))
                {
                    await match4.match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <typeparam name="T3">The type of the third match.</typeparam>
        /// <typeparam name="T4">The type of the fourth match.</typeparam>
        /// <typeparam name="T5">The type of the fifth match.</typeparam>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <param name="match3">The third match.</param>
        /// <param name="match4">The fourth match.</param>
        /// <param name="match5">The fifth match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public async Task<bool> MatchAsync<T1, T2, T3, T4, T5>((string contentType, Func<T1, Task> match) match1, (string contentType, Func<T2, Task> match) match2, (string contentType, Func<T3, Task> match) match3, (string contentType, Func<T4, Task> match) match4, (string contentType, Func<T5, Task> match) match5)
        {
            if (match1.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match1));
            }

            if (match1.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match1));
            }

            if (match2.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match2));
            }

            if (match2.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match2));
            }

            if (match3.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match3));
            }

            if (match3.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match3));
            }

            if (match4.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match4));
            }

            if (match4.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match4));
            }

            if (match5.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match5));
            }

            if (match5.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match5));
            }

            if (this.PayloadContentType == match1.contentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    await match1.match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match2.contentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    await match2.match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match3.contentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    await match3.match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match4.contentType)
            {
                if (this.TryGetPayload(out T4 payload))
                {
                    await match4.match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match5.contentType)
            {
                if (this.TryGetPayload(out T5 payload))
                {
                    await match5.match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public async Task<bool> MatchAsync<T1, T2>(Func<T1, Task> match1, Func<T2, Task> match2)
        {
            if (match1 is null)
            {
                throw new ArgumentNullException(nameof(match1));
            }

            if (match2 is null)
            {
                throw new ArgumentNullException(nameof(match2));
            }

            string match1ContentType = ContentFactory.GetContentType<T1>();
            if (this.PayloadContentType == match1ContentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    await match1(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            string match2ContentType = ContentFactory.GetContentType<T2>();
            if (this.PayloadContentType == match2ContentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    await match2(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <typeparam name="T3">The type of the third match.</typeparam>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <param name="match3">The third match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public async Task<bool> MatchAsync<T1, T2, T3>(Func<T1, Task> match1, Func<T2, Task> match2, Func<T3, Task> match3)
        {
            if (match1 is null)
            {
                throw new ArgumentNullException(nameof(match1));
            }

            if (match2 is null)
            {
                throw new ArgumentNullException(nameof(match2));
            }

            if (match3 is null)
            {
                throw new ArgumentNullException(nameof(match3));
            }

            string match1ContentType = ContentFactory.GetContentType<T1>();
            if (this.PayloadContentType == match1ContentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    await match1(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            string match2ContentType = ContentFactory.GetContentType<T2>();
            if (this.PayloadContentType == match2ContentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    await match2(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            string match3ContentType = ContentFactory.GetContentType<T3>();
            if (this.PayloadContentType == match3ContentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    await match3(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <typeparam name="T3">The type of the third match.</typeparam>
        /// <typeparam name="T4">The type of the fourth match.</typeparam>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <param name="match3">The third match.</param>
        /// <param name="match4">The fourth match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public async Task<bool> MatchAsync<T1, T2, T3, T4>(Func<T1, Task> match1, Func<T2, Task> match2, Func<T3, Task> match3, Func<T4, Task> match4)
        {
            if (match1 is null)
            {
                throw new ArgumentNullException(nameof(match1));
            }

            if (match2 is null)
            {
                throw new ArgumentNullException(nameof(match2));
            }

            if (match3 is null)
            {
                throw new ArgumentNullException(nameof(match3));
            }

            if (match4 is null)
            {
                throw new ArgumentNullException(nameof(match4));
            }

            string match1ContentType = ContentFactory.GetContentType<T1>();
            if (this.PayloadContentType == match1ContentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    await match1(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            string match2ContentType = ContentFactory.GetContentType<T2>();
            if (this.PayloadContentType == match2ContentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    await match2(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            string match3ContentType = ContentFactory.GetContentType<T3>();
            if (this.PayloadContentType == match3ContentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    await match3(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            string match4ContentType = ContentFactory.GetContentType<T4>();
            if (this.PayloadContentType == match4ContentType)
            {
                if (this.TryGetPayload(out T4 payload))
                {
                    await match4(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <typeparam name="T3">The type of the third match.</typeparam>
        /// <typeparam name="T4">The type of the fourth match.</typeparam>
        /// <typeparam name="T5">The type of the fifth match.</typeparam>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <param name="match3">The third match.</param>
        /// <param name="match4">The fourth match.</param>
        /// <param name="match5">The fifth match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public async Task<bool> MatchAsync<T1, T2, T3, T4, T5>(Func<T1, Task> match1, Func<T2, Task> match2, Func<T3, Task> match3, Func<T4, Task> match4, Func<T5, Task> match5)
        {
            if (match1 is null)
            {
                throw new ArgumentNullException(nameof(match1));
            }

            if (match2 is null)
            {
                throw new ArgumentNullException(nameof(match2));
            }

            if (match3 is null)
            {
                throw new ArgumentNullException(nameof(match3));
            }

            if (match4 is null)
            {
                throw new ArgumentNullException(nameof(match4));
            }

            if (match5 is null)
            {
                throw new ArgumentNullException(nameof(match5));
            }

            string match1ContentType = ContentFactory.GetContentType<T1>();
            if (this.PayloadContentType == match1ContentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    await match1(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            string match2ContentType = ContentFactory.GetContentType<T2>();
            if (this.PayloadContentType == match2ContentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    await match2(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            string match3ContentType = ContentFactory.GetContentType<T3>();
            if (this.PayloadContentType == match3ContentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    await match3(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            string match4ContentType = ContentFactory.GetContentType<T4>();
            if (this.PayloadContentType == match4ContentType)
            {
                if (this.TryGetPayload(out T4 payload))
                {
                    await match4(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            string match5ContentType = ContentFactory.GetContentType<T5>();
            if (this.PayloadContentType == match5ContentType)
            {
                if (this.TryGetPayload(out T5 payload))
                {
                    await match5(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            return false;
        }

        /***/

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public bool Match<T1, T2>((string contentType, Action<T1> match) match1, (string contentType, Action<T2> match) match2)
        {
            if (match1.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match1));
            }

            if (match1.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match1));
            }

            if (match2.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match2));
            }

            if (match2.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match2));
            }

            if (this.PayloadContentType == match1.contentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    match1.match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match2.contentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    match2.match(payload);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <typeparam name="T3">The type of the third match.</typeparam>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <param name="match3">The third match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public bool Match<T1, T2, T3>((string contentType, Action<T1> match) match1, (string contentType, Action<T2> match) match2, (string contentType, Action<T3> match) match3)
        {
            if (match1.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match1));
            }

            if (match1.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match1));
            }

            if (match2.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match2));
            }

            if (match2.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match2));
            }

            if (match3.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match3));
            }

            if (match3.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match3));
            }

            if (this.PayloadContentType == match1.contentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    match1.match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match2.contentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    match2.match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match3.contentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    match3.match(payload);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <typeparam name="T3">The type of the third match.</typeparam>
        /// <typeparam name="T4">The type of the fourth match.</typeparam>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <param name="match3">The third match.</param>
        /// <param name="match4">The fourth match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public bool Match<T1, T2, T3, T4>((string contentType, Action<T1> match) match1, (string contentType, Action<T2> match) match2, (string contentType, Action<T3> match) match3, (string contentType, Action<T4> match) match4)
        {
            if (match1.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match1));
            }

            if (match1.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match1));
            }

            if (match2.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match2));
            }

            if (match2.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match2));
            }

            if (match3.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match3));
            }

            if (match3.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match3));
            }

            if (match4.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match4));
            }

            if (match4.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match4));
            }

            if (this.PayloadContentType == match1.contentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    match1.match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match2.contentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    match2.match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match3.contentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    match3.match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match4.contentType)
            {
                if (this.TryGetPayload(out T4 payload))
                {
                    match4.match(payload);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <typeparam name="T3">The type of the third match.</typeparam>
        /// <typeparam name="T4">The type of the fourth match.</typeparam>
        /// <typeparam name="T5">The type of the fifth match.</typeparam>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <param name="match3">The third match.</param>
        /// <param name="match4">The fourth match.</param>
        /// <param name="match5">The fifth match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public bool Match<T1, T2, T3, T4, T5>((string contentType, Action<T1> match) match1, (string contentType, Action<T2> match) match2, (string contentType, Action<T3> match) match3, (string contentType, Action<T4> match) match4, (string contentType, Action<T5> match) match5)
        {
            if (match1.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match1));
            }

            if (match1.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match1));
            }

            if (match2.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match2));
            }

            if (match2.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match2));
            }

            if (match3.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match3));
            }

            if (match3.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match3));
            }

            if (match4.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match4));
            }

            if (match4.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match4));
            }

            if (match5.match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match5));
            }

            if (match5.contentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match5));
            }

            if (this.PayloadContentType == match1.contentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    match1.match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match2.contentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    match2.match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match3.contentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    match3.match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match4.contentType)
            {
                if (this.TryGetPayload(out T4 payload))
                {
                    match4.match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match5.contentType)
            {
                if (this.TryGetPayload(out T5 payload))
                {
                    match5.match(payload);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public bool Match<T1, T2>(Action<T1> match1, Action<T2> match2)
        {
            if (match1 is null)
            {
                throw new ArgumentNullException(nameof(match1));
            }

            if (match2 is null)
            {
                throw new ArgumentNullException(nameof(match2));
            }

            string match1ContentType = ContentFactory.GetContentType<T1>();
            if (this.PayloadContentType == match1ContentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    match1(payload);
                    return true;
                }

                return false;
            }

            string match2ContentType = ContentFactory.GetContentType<T2>();
            if (this.PayloadContentType == match2ContentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    match2(payload);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <typeparam name="T3">The type of the third match.</typeparam>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <param name="match3">The third match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public bool Match<T1, T2, T3>(Action<T1> match1, Action<T2> match2, Action<T3> match3)
        {
            if (match1 is null)
            {
                throw new ArgumentNullException(nameof(match1));
            }

            if (match2 is null)
            {
                throw new ArgumentNullException(nameof(match2));
            }

            if (match3 is null)
            {
                throw new ArgumentNullException(nameof(match3));
            }

            string match1ContentType = ContentFactory.GetContentType<T1>();
            if (this.PayloadContentType == match1ContentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    match1(payload);
                    return true;
                }

                return false;
            }

            string match2ContentType = ContentFactory.GetContentType<T2>();
            if (this.PayloadContentType == match2ContentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    match2(payload);
                    return true;
                }

                return false;
            }

            string match3ContentType = ContentFactory.GetContentType<T3>();
            if (this.PayloadContentType == match3ContentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    match3(payload);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <typeparam name="T3">The type of the third match.</typeparam>
        /// <typeparam name="T4">The type of the fourth match.</typeparam>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <param name="match3">The third match.</param>
        /// <param name="match4">The fourth match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public bool Match<T1, T2, T3, T4>(Action<T1> match1, Action<T2> match2, Action<T3> match3, Action<T4> match4)
        {
            if (match1 is null)
            {
                throw new ArgumentNullException(nameof(match1));
            }

            if (match2 is null)
            {
                throw new ArgumentNullException(nameof(match2));
            }

            if (match3 is null)
            {
                throw new ArgumentNullException(nameof(match3));
            }

            if (match4 is null)
            {
                throw new ArgumentNullException(nameof(match4));
            }

            string match1ContentType = ContentFactory.GetContentType<T1>();
            if (this.PayloadContentType == match1ContentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    match1(payload);
                    return true;
                }

                return false;
            }

            string match2ContentType = ContentFactory.GetContentType<T2>();
            if (this.PayloadContentType == match2ContentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    match2(payload);
                    return true;
                }

                return false;
            }

            string match3ContentType = ContentFactory.GetContentType<T3>();
            if (this.PayloadContentType == match3ContentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    match3(payload);
                    return true;
                }

                return false;
            }

            string match4ContentType = ContentFactory.GetContentType<T4>();
            if (this.PayloadContentType == match4ContentType)
            {
                if (this.TryGetPayload(out T4 payload))
                {
                    match4(payload);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <typeparam name="T3">The type of the third match.</typeparam>
        /// <typeparam name="T4">The type of the fourth match.</typeparam>
        /// <typeparam name="T5">The type of the fifth match.</typeparam>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <param name="match3">The third match.</param>
        /// <param name="match4">The fourth match.</param>
        /// <param name="match5">The fifth match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public bool Match<T1, T2, T3, T4, T5>(Action<T1> match1, Action<T2> match2, Action<T3> match3, Action<T4> match4, Action<T5> match5)
        {
            if (match1 is null)
            {
                throw new ArgumentNullException(nameof(match1));
            }

            if (match2 is null)
            {
                throw new ArgumentNullException(nameof(match2));
            }

            if (match3 is null)
            {
                throw new ArgumentNullException(nameof(match3));
            }

            if (match4 is null)
            {
                throw new ArgumentNullException(nameof(match4));
            }

            if (match5 is null)
            {
                throw new ArgumentNullException(nameof(match5));
            }

            string match1ContentType = ContentFactory.GetContentType<T1>();
            if (this.PayloadContentType == match1ContentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    match1(payload);
                    return true;
                }

                return false;
            }

            string match2ContentType = ContentFactory.GetContentType<T2>();
            if (this.PayloadContentType == match2ContentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    match2(payload);
                    return true;
                }

                return false;
            }

            string match3ContentType = ContentFactory.GetContentType<T3>();
            if (this.PayloadContentType == match3ContentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    match3(payload);
                    return true;
                }

                return false;
            }

            string match4ContentType = ContentFactory.GetContentType<T4>();
            if (this.PayloadContentType == match4ContentType)
            {
                if (this.TryGetPayload(out T4 payload))
                {
                    match4(payload);
                    return true;
                }

                return false;
            }

            string match5ContentType = ContentFactory.GetContentType<T5>();
            if (this.PayloadContentType == match5ContentType)
            {
                if (this.TryGetPayload(out T5 payload))
                {
                    match5(payload);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// Dispatch the payload to a handler.
        /// </summary>
        /// <param name="contentHandlerFactory">The content handler factory to use to dispatch to a handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        public void DispatchToHandler(IContentHandlerDispatcher<ContentEnvelope> contentHandlerFactory, string handlerClass)
        {
            if (contentHandlerFactory is null)
            {
                throw new ArgumentNullException(nameof(contentHandlerFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentHandlerFactory.DispatchPayloadToHandler(this, this.PayloadContentType, handlerClass);
        }

        /// <summary>
        /// Dispatch the payload to a handler.
        /// </summary>
        /// <param name="contentHandlerFactory">The content handler factory to use to dispatch to a handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <returns>A <see cref="Task"/> which completes once the handler completes.</returns>
        public Task DispatchToHandlerAsync(IContentHandlerDispatcher<ContentEnvelope> contentHandlerFactory, string handlerClass)
        {
            if (contentHandlerFactory is null)
            {
                throw new ArgumentNullException(nameof(contentHandlerFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            return contentHandlerFactory.DispatchPayloadToHandlerAsync(this, this.PayloadContentType, handlerClass);
        }
    }
}
