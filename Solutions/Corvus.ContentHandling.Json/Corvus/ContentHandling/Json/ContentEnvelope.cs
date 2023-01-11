// <copyright file="ContentEnvelope.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json
{
    using System;
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using System.Threading.Tasks;

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
    /// model, or you can specify your own type discriminator when you set it with <see cref="SetPayload{T}(T, JsonSerializerOptions, string)"/>.
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
    /// var sendingContentEnvelope = new ContentEnvelope(serializerOptions);
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
        private JsonSerializerOptions? serializerOptions;
        private JsonNode? serializedPayload;
        private string? payloadContentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentEnvelope"/> class.
        /// </summary>
        /// <remark>
        /// <para>
        /// Creates an empty envelope with no content type, payload, or serializer settings.
        /// </para>
        /// <para>
        /// For an instance created this way to be useful, you will need to supply a payload at
        /// some point. Normally you should call the <see cref="SetPayload{T}(T, JsonSerializerOptions, string?)"/>
        /// overload that takes a <see cref="JsonSerializerOptions"/>, because most of the interesting
        /// operations on a <see cref="ContentEnvelope"/> involve deserializing the payload, which in turn
        /// requires access to serialization settings.
        /// </para>
        /// <para>
        /// (Note: the old Newtonsoft-based version of this library would supply default serialization
        /// settings if you didn't bring your own. This was problematic because it made it very easy
        /// accidentally to create a <see cref="ContentEnvelope"/> with serialization behaviour that
        /// was inconsistent with the rest of the application. Although it might have been 'easier'
        /// not to have to give it configuration settings, it meant it wouldn't have access to any
        /// converters, or your preferred casing conventions, leading to inconsistent behaviour.)
        /// </para>
        /// </remark>
        public ContentEnvelope()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentEnvelope"/> class.
        /// </summary>
        /// <param name="serializerOptions">
        /// Serialization options.
        /// </param>
        public ContentEnvelope(JsonSerializerOptions serializerOptions)
        {
            this.serializerOptions = serializerOptions;
        }

        private ContentEnvelope(
            JsonNode payload,
            string contentType,
            JsonSerializerOptions? serializerOptions)
        {
            this.serializedPayload = payload;
            this.payloadContentType = contentType;
            this.serializerOptions = serializerOptions;
        }

        /// <summary>
        /// Gets the content type of the payload.
        /// </summary>
        public string PayloadContentType => this.payloadContentType ?? throw new InvalidOperationException("You must supply a content type either during construction or by calling " + nameof(this.SetPayload) + " before trying to retrieve the " + nameof(this.PayloadContentType));

        /// <summary>
        /// Gets the <see cref="JsonSerializerOptions"/> to use for the content envelope.
        /// </summary>
        public JsonSerializerOptions SerializerOptions => this.serializerOptions ?? throw new InvalidOperationException("No JsonSerializerOptions were supplied when this ContentEnvelope was created, so you cannot perform operations that involve serialization");

        /// <summary>
        /// Gets the serialized representation of the payload.
        /// </summary>
        ////[JsonProperty]
        internal JsonNode SerializedPayload => this.serializedPayload ?? throw new InvalidOperationException("You must supply a payload either during construction or by calling " + nameof(this.SetPayload) + " before trying to retrieve the " + nameof(this.SerializedPayload));

        /// <summary>
        /// Construct a content envelope from a payload.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <param name="serializerOptions">Settings for the serialization process.</param>
        /// <param name="contentType">The content type of the payload.</param>
        /// <returns>An instance of a content envelope initialized with the given payload.</returns>
        public static ContentEnvelope FromPayload<T>(T payload, JsonSerializerOptions serializerOptions, string? contentType = null)
        {
            contentType = string.IsNullOrEmpty(contentType)
                    ? ContentFactory.GetContentType<T>()
                    : contentType;
            return new ContentEnvelope(
                JsonSerializer.SerializeToNode(payload, serializerOptions)!,
                contentType,
                serializerOptions);
        }

        /// <summary>
        /// Construct a content envelope from a json string.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>The content envelope for the json.</returns>
        /// <remarks>
        /// If the root json entity contains a <c>contentType</c> property,
        /// you do not need to specify the <paramref name="contentType"/>, it
        /// will be automatically retrieved from the JObject.
        /// </remarks>
        public static ContentEnvelope FromJson(string jsonString, string? contentType = null)
        {
            if (jsonString is null)
            {
                throw new ArgumentNullException(nameof(jsonString));
            }

            JsonNode json = JsonNode.Parse(jsonString)!;

            return FromJson(json, contentType);
        }

        /// <summary>
        /// Construct a content envelope from a json entity.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <param name="serializerOptions">Serializer options.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>The content envelope for the json.</returns>
        /// <remarks>
        /// If the root json entity contains a <c>contentType</c> property,
        /// you do not need to specify the <paramref name="contentType"/>, it
        /// will be automatically retrieved from the JObject.
        /// </remarks>
        public static ContentEnvelope FromJson(JsonNode json, JsonSerializerOptions serializerOptions, string? contentType = null)
        {
            ArgumentNullException.ThrowIfNull(json);

            if (string.IsNullOrEmpty(contentType))
            {
                contentType = json["contentType"]!.GetValue<string>();
            }

            return new ContentEnvelope(json, contentType, serializerOptions);
        }

        /// <summary>
        /// Construct a content envelope from a json entity.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>The content envelope for the json.</returns>
        /// <remarks>
        /// If the root json entity contains a <c>contentType</c> property,
        /// you do not need to specify the <paramref name="contentType"/>, it
        /// will be automatically retrieved from the JObject.
        /// </remarks>
        public static ContentEnvelope FromJson(JsonNode json, string? contentType = null)
        {
            ArgumentNullException.ThrowIfNull(json);

            if (string.IsNullOrEmpty(contentType))
            {
                contentType = json["contentType"]!.GetValue<string>();
            }

            return new ContentEnvelope(json, contentType, null);
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
        public static ContentEnvelope FromJson(Stream stream, string? contentType = null)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var json = JsonNode.Parse(stream);
            return FromJson(json!, contentType);
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
        public static async Task<ContentEnvelope> FromJsonAsync(Stream stream, string? contentType = null)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            JsonNode? json = await JsonSerializer.DeserializeAsync<JsonNode>(stream).ConfigureAwait(false);
            return FromJson(json!, contentType);
        }

        /// <summary>
        /// Sets the payload in the envelope.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="payload">The payload to set.</param>
        /// <param name="payloadContentType">The content type of the payload. You do not need to set this if the payload conforms to the <see cref="ContentFactory"/> pattern.</param>
        public void SetPayload<T>(T payload, string? payloadContentType = null)
        {
            if (string.IsNullOrEmpty(payloadContentType))
            {
                this.payloadContentType = ContentFactory.GetContentType<T>();
            }
            else
            {
                this.payloadContentType = payloadContentType;
            }

            this.serializedPayload = JsonSerializer.SerializeToNode(payload, this.SerializerOptions)!;
        }

        /// <summary>
        /// Sets the payload in the envelope.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="payload">The payload to set.</param>
        /// <param name="serializerOptions">Settings for the serialization process.</param>
        /// <param name="payloadContentType">The content type of the payload. You do not need to set this if the payload conforms to the <see cref="ContentFactory"/> pattern.</param>
        public void SetPayload<T>(T payload, JsonSerializerOptions serializerOptions, string? payloadContentType = null)
        {
            if (string.IsNullOrEmpty(payloadContentType))
            {
                this.payloadContentType = ContentFactory.GetContentType<T>();
            }
            else
            {
                this.payloadContentType = payloadContentType;
            }

            this.serializedPayload = JsonSerializer.SerializeToNode(payload, serializerOptions)!;
        }

        /// <summary>
        /// Get the payload as the specified type.
        /// </summary>
        /// <typeparam name="T">The type of payload to retrieve.</typeparam>
        /// <returns>An instance of the type.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the payload is not accessible through the given type.</exception>
        public T GetContents<T>()
        {
            if (this.TryGetPayload(this.SerializerOptions, out T result))
            {
                return result;
            }

            throw new InvalidOperationException($"The payload of the message is not accessible through the type {typeof(T).FullName}");
        }

        /// <summary>
        /// Get the payload as the specified type.
        /// </summary>
        /// <typeparam name="T">The type of payload to retrieve.</typeparam>
        /// <param name="serializerOptions">Settings for the serialization process.</param>
        /// <returns>An instance of the type.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the payload is not accessible through the given type.</exception>
        public T GetContents<T>(JsonSerializerOptions serializerOptions)
        {
            if (this.TryGetPayload(serializerOptions, out T result))
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
            => this.TryGetPayload<T>(this.SerializerOptions, out result);

        /// <summary>
        /// Try to get the payload as the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="serializerOptions">Settings for the serialization process.</param>
        /// <param name="result">The payload, as the specified type.</param>
        /// <returns>True if the payload was available as the specified type.</returns>
        public bool TryGetPayload<T>(JsonSerializerOptions serializerOptions, out T result)
        {
            if (this.SerializedPayload == null)
            {
                result = default!;
                return true;
            }

            try
            {
                result = this.SerializedPayload.Deserialize<T>(serializerOptions)!;
                return true;
            }
            catch (JsonException)
            {
                result = default!;
                return false;
            }
        }

        /// <summary>
        /// A match operation for the payload.
        /// </summary>
        /// <typeparam name="T1">The type of the first match.</typeparam>
        /// <typeparam name="T2">The type of the second match.</typeparam>
        /// <param name="serializerOptions">Settings for the serialization process.</param>
        /// <param name="match1">The first match.</param>
        /// <param name="match2">The second match.</param>
        /// <returns>True if the match was made, otherwise false.</returns>
        public async Task<bool> MatchAsync<T1, T2>(
            JsonSerializerOptions serializerOptions,
            (string ContentType, Func<T1, Task> Match) match1,
            (string ContentType, Func<T2, Task> Match) match2)
        {
            if (match1.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match1));
            }

            if (match1.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match1));
            }

            if (match2.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match2));
            }

            if (match2.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match2));
            }

            if (this.PayloadContentType == match1.ContentType)
            {
                if (this.TryGetPayload(serializerOptions, out T1 payload))
                {
                    await match1.Match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match2.ContentType)
            {
                if (this.TryGetPayload(serializerOptions, out T2 payload))
                {
                    await match2.Match(payload).ConfigureAwait(false);
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
#pragma warning disable CA1822 // Mark members as static
        public async Task<bool> MatchAsync<T1, T2, T3>((string ContentType, Func<T1, Task> Match) match1, (string ContentType, Func<T2, Task> Match) match2, (string ContentType, Func<T3, Task> Match) match3)
        {
            if (match1.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match1));
            }

            if (match1.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match1));
            }

            if (match2.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match2));
            }

            if (match2.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match2));
            }

            if (match3.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match3));
            }

            if (match3.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match3));
            }

            if (this.PayloadContentType == match1.ContentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    await match1.Match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match2.ContentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    await match2.Match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match3.ContentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    await match3.Match(payload).ConfigureAwait(false);
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
        public async Task<bool> MatchAsync<T1, T2, T3, T4>((string ContentType, Func<T1, Task> Match) match1, (string ContentType, Func<T2, Task> Match) match2, (string ContentType, Func<T3, Task> Match) match3, (string ContentType, Func<T4, Task> Match) match4)
        {
            if (match1.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match1));
            }

            if (match1.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match1));
            }

            if (match2.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match2));
            }

            if (match2.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match2));
            }

            if (match3.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match3));
            }

            if (match3.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match3));
            }

            if (match4.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match4));
            }

            if (match4.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match4));
            }

            if (this.PayloadContentType == match1.ContentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    await match1.Match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match2.ContentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    await match2.Match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match3.ContentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    await match3.Match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match4.ContentType)
            {
                if (this.TryGetPayload(out T4 payload))
                {
                    await match4.Match(payload).ConfigureAwait(false);
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
        public async Task<bool> MatchAsync<T1, T2, T3, T4, T5>((string ContentType, Func<T1, Task> Match) match1, (string ContentType, Func<T2, Task> Match) match2, (string ContentType, Func<T3, Task> Match) match3, (string ContentType, Func<T4, Task> Match) match4, (string ContentType, Func<T5, Task> Match) match5)
        {
            if (match1.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match1));
            }

            if (match1.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match1));
            }

            if (match2.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match2));
            }

            if (match2.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match2));
            }

            if (match3.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match3));
            }

            if (match3.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match3));
            }

            if (match4.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match4));
            }

            if (match4.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match4));
            }

            if (match5.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match5));
            }

            if (match5.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match5));
            }

            if (this.PayloadContentType == match1.ContentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    await match1.Match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match2.ContentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    await match2.Match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match3.ContentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    await match3.Match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match4.ContentType)
            {
                if (this.TryGetPayload(out T4 payload))
                {
                    await match4.Match(payload).ConfigureAwait(false);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match5.ContentType)
            {
                if (this.TryGetPayload(out T5 payload))
                {
                    await match5.Match(payload).ConfigureAwait(false);
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
        public bool Match<T1, T2>((string ContentType, Action<T1> Match) match1, (string ContentType, Action<T2> Match) match2)
        {
            if (match1.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match1));
            }

            if (match1.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match1));
            }

            if (match2.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match2));
            }

            if (match2.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match2));
            }

            if (this.PayloadContentType == match1.ContentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    match1.Match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match2.ContentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    match2.Match(payload);
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
        public bool Match<T1, T2, T3>((string ContentType, Action<T1> Match) match1, (string ContentType, Action<T2> Match) match2, (string ContentType, Action<T3> Match) match3)
        {
            if (match1.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match1));
            }

            if (match1.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match1));
            }

            if (match2.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match2));
            }

            if (match2.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match2));
            }

            if (match3.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match3));
            }

            if (match3.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match3));
            }

            if (this.PayloadContentType == match1.ContentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    match1.Match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match2.ContentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    match2.Match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match3.ContentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    match3.Match(payload);
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
        public bool Match<T1, T2, T3, T4>((string ContentType, Action<T1> Match) match1, (string ContentType, Action<T2> Match) match2, (string ContentType, Action<T3> Match) match3, (string ContentType, Action<T4> Match) match4)
        {
            if (match1.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match1));
            }

            if (match1.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match1));
            }

            if (match2.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match2));
            }

            if (match2.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match2));
            }

            if (match3.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match3));
            }

            if (match3.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match3));
            }

            if (match4.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match4));
            }

            if (match4.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match4));
            }

            if (this.PayloadContentType == match1.ContentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    match1.Match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match2.ContentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    match2.Match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match3.ContentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    match3.Match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match4.ContentType)
            {
                if (this.TryGetPayload(out T4 payload))
                {
                    match4.Match(payload);
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
        public bool Match<T1, T2, T3, T4, T5>((string ContentType, Action<T1> Match) match1, (string ContentType, Action<T2> Match) match2, (string ContentType, Action<T3> Match) match3, (string ContentType, Action<T4> Match) match4, (string ContentType, Action<T5> Match) match5)
        {
            if (match1.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match1));
            }

            if (match1.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match1));
            }

            if (match2.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match2));
            }

            if (match2.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match2));
            }

            if (match3.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match3));
            }

            if (match3.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match3));
            }

            if (match4.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match4));
            }

            if (match4.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match4));
            }

            if (match5.Match is null)
            {
                throw new ArgumentException("You must provide a match function", nameof(match5));
            }

            if (match5.ContentType is null)
            {
                throw new ArgumentException("You must provide a match content type", nameof(match5));
            }

            if (this.PayloadContentType == match1.ContentType)
            {
                if (this.TryGetPayload(out T1 payload))
                {
                    match1.Match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match2.ContentType)
            {
                if (this.TryGetPayload(out T2 payload))
                {
                    match2.Match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match3.ContentType)
            {
                if (this.TryGetPayload(out T3 payload))
                {
                    match3.Match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match4.ContentType)
            {
                if (this.TryGetPayload(out T4 payload))
                {
                    match4.Match(payload);
                    return true;
                }

                return false;
            }

            if (this.PayloadContentType == match5.ContentType)
            {
                if (this.TryGetPayload(out T5 payload))
                {
                    match5.Match(payload);
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