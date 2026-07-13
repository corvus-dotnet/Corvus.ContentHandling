// <copyright file="ContentEnvelope.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json
{
    using System;
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A wrapper for a payload identified by a content type, enabling heterogeneous content to
    /// travel through a single channel (a queue, a document store, an API) and be dispatched
    /// by content type at the receiver.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The envelope serializes as <c>{ "contentType": ..., "payload": ... }</c> — the same wire
    /// format as earlier versions of this library.
    /// </para>
    /// <para>
    /// Operations involving serialization require <see cref="JsonSerializerOptions"/>, supplied
    /// at construction or per call. No default options are assumed: supplying your
    /// application's options (typically from <c>IJsonSerializerOptionsProvider</c>) keeps
    /// envelope serialization consistent with the rest of your application.
    /// </para>
    /// </remarks>
    public class ContentEnvelope
    {
        private JsonSerializerOptions? serializerOptions;
        private JsonNode? serializedPayload;
        private string? payloadContentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentEnvelope"/> class with no
        /// payload, content type, or serializer options.
        /// </summary>
        public ContentEnvelope()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentEnvelope"/> class.
        /// </summary>
        /// <param name="serializerOptions">The serializer options for payload operations.</param>
        public ContentEnvelope(JsonSerializerOptions serializerOptions)
        {
            this.serializerOptions = serializerOptions;
        }

        private ContentEnvelope(JsonNode payload, string contentType, JsonSerializerOptions? serializerOptions)
        {
            this.serializedPayload = payload;
            this.payloadContentType = contentType;
            this.serializerOptions = serializerOptions;
        }

        /// <summary>
        /// Gets the content type of the payload.
        /// </summary>
        public string PayloadContentType => this.payloadContentType ?? throw new InvalidOperationException($"You must supply a content type either during construction or by calling {nameof(this.SetPayload)} before trying to retrieve the {nameof(this.PayloadContentType)}.");

        /// <summary>
        /// Gets the <see cref="JsonSerializerOptions"/> for this content envelope.
        /// </summary>
        public JsonSerializerOptions SerializerOptions => this.serializerOptions ?? throw new InvalidOperationException($"No JsonSerializerOptions were supplied when this {nameof(ContentEnvelope)} was created, so you cannot perform operations that involve serialization.");

        /// <summary>
        /// Gets the serialized representation of the payload.
        /// </summary>
        internal JsonNode SerializedPayload => this.serializedPayload ?? throw new InvalidOperationException($"You must supply a payload either during construction or by calling {nameof(this.SetPayload)} before trying to retrieve the payload.");

        /// <summary>
        /// Constructs a content envelope from a payload.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <param name="serializerOptions">The serializer options.</param>
        /// <param name="contentType">
        /// The content type of the payload; derived from the payload instance (falling back to
        /// the payload type) when omitted.
        /// </param>
        /// <returns>A content envelope containing the payload.</returns>
        public static ContentEnvelope FromPayload<T>(T payload, JsonSerializerOptions serializerOptions, string? contentType = null)
        {
            return new ContentEnvelope(
                JsonSerializer.SerializeToNode(payload, serializerOptions)!,
                ResolveContentType(payload, contentType),
                serializerOptions);
        }

        /// <summary>
        /// Constructs a content envelope from a JSON string.
        /// </summary>
        /// <param name="jsonString">The JSON string.</param>
        /// <param name="contentType">
        /// The content type; when omitted, it is read from the JSON's <c>contentType</c> property.
        /// </param>
        /// <returns>The content envelope for the JSON.</returns>
        public static ContentEnvelope FromJson(string jsonString, string? contentType = null)
        {
            ArgumentNullException.ThrowIfNull(jsonString);

            return FromJson(JsonNode.Parse(jsonString)!, contentType);
        }

        /// <summary>
        /// Constructs a content envelope from a JSON node.
        /// </summary>
        /// <param name="json">The JSON node.</param>
        /// <param name="contentType">
        /// The content type; when omitted, it is read from the JSON's <c>contentType</c> property.
        /// </param>
        /// <returns>The content envelope for the JSON.</returns>
        public static ContentEnvelope FromJson(JsonNode json, string? contentType = null)
        {
            ArgumentNullException.ThrowIfNull(json);

            return new ContentEnvelope(json, string.IsNullOrEmpty(contentType) ? ReadContentTypeProperty(json) : contentType, null);
        }

        /// <summary>
        /// Constructs a content envelope from a JSON node.
        /// </summary>
        /// <param name="json">The JSON node.</param>
        /// <param name="serializerOptions">The serializer options.</param>
        /// <param name="contentType">
        /// The content type; when omitted, it is read from the JSON's <c>contentType</c> property.
        /// </param>
        /// <returns>The content envelope for the JSON.</returns>
        public static ContentEnvelope FromJson(JsonNode json, JsonSerializerOptions serializerOptions, string? contentType = null)
        {
            ArgumentNullException.ThrowIfNull(json);

            return new ContentEnvelope(json, string.IsNullOrEmpty(contentType) ? ReadContentTypeProperty(json) : contentType, serializerOptions);
        }

        /// <summary>
        /// Constructs a content envelope from a stream of JSON text.
        /// </summary>
        /// <param name="stream">The JSON text stream.</param>
        /// <param name="contentType">
        /// The content type; when omitted, it is read from the JSON's <c>contentType</c> property.
        /// </param>
        /// <returns>The content envelope for the JSON.</returns>
        public static ContentEnvelope FromJson(Stream stream, string? contentType = null)
        {
            ArgumentNullException.ThrowIfNull(stream);

            return FromJson(JsonNode.Parse(stream)!, contentType);
        }

        /// <summary>
        /// Constructs a content envelope from a stream of JSON text.
        /// </summary>
        /// <param name="stream">The JSON text stream.</param>
        /// <param name="contentType">
        /// The content type; when omitted, it is read from the JSON's <c>contentType</c> property.
        /// </param>
        /// <param name="cancellationToken">A token to observe for cancellation.</param>
        /// <returns>A task producing the content envelope for the JSON.</returns>
        public static async Task<ContentEnvelope> FromJsonAsync(Stream stream, string? contentType = null, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(stream);

            JsonNode? json = await JsonSerializer.DeserializeAsync<JsonNode>(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
            return FromJson(json!, contentType);
        }

        /// <summary>
        /// Sets the payload in the envelope, using the envelope's serializer options.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="payload">The payload to set.</param>
        /// <param name="payloadContentType">
        /// The content type of the payload; derived from the payload instance (falling back to
        /// the payload type) when omitted.
        /// </param>
        public void SetPayload<T>(T payload, string? payloadContentType = null)
        {
            this.payloadContentType = ResolveContentType(payload, payloadContentType);
            this.serializedPayload = JsonSerializer.SerializeToNode(payload, this.SerializerOptions)!;
        }

        /// <summary>
        /// Sets the payload in the envelope.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="payload">The payload to set.</param>
        /// <param name="serializerOptions">The serializer options; retained by the envelope.</param>
        /// <param name="payloadContentType">
        /// The content type of the payload; derived from the payload instance (falling back to
        /// the payload type) when omitted.
        /// </param>
        public void SetPayload<T>(T payload, JsonSerializerOptions serializerOptions, string? payloadContentType = null)
        {
            ArgumentNullException.ThrowIfNull(serializerOptions);

            this.payloadContentType = ResolveContentType(payload, payloadContentType);
            this.serializedPayload = JsonSerializer.SerializeToNode(payload, serializerOptions)!;
            this.serializerOptions = serializerOptions;
        }

        /// <summary>
        /// Gets the payload as the specified type, using the envelope's serializer options.
        /// </summary>
        /// <typeparam name="T">The type of payload to retrieve.</typeparam>
        /// <returns>The payload.</returns>
        /// <exception cref="InvalidOperationException">The payload is not accessible through the given type.</exception>
        public T GetContents<T>()
        {
            return this.GetContents<T>(this.SerializerOptions);
        }

        /// <summary>
        /// Gets the payload as the specified type.
        /// </summary>
        /// <typeparam name="T">The type of payload to retrieve.</typeparam>
        /// <param name="serializerOptions">The serializer options.</param>
        /// <returns>The payload.</returns>
        /// <exception cref="InvalidOperationException">The payload is not accessible through the given type.</exception>
        public T GetContents<T>(JsonSerializerOptions serializerOptions)
        {
            if (this.TryGetPayload(serializerOptions, out T result))
            {
                return result;
            }

            throw new InvalidOperationException($"The payload of the message is not accessible through the type {typeof(T).FullName}");
        }

        /// <summary>
        /// Tries to get the payload as the specified type, using the envelope's serializer options.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="result">The payload, as the specified type.</param>
        /// <returns>True if the payload was available as the specified type.</returns>
        public bool TryGetPayload<T>(out T result)
        {
            return this.TryGetPayload(this.SerializerOptions, out result);
        }

        /// <summary>
        /// Tries to get the payload as the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="serializerOptions">The serializer options.</param>
        /// <param name="result">The payload, as the specified type.</param>
        /// <returns>True if the payload was available as the specified type.</returns>
        public bool TryGetPayload<T>(JsonSerializerOptions serializerOptions, out T result)
        {
            try
            {
                T? deserialized = this.SerializedPayload.Deserialize<T>(serializerOptions);
                if (deserialized is null)
                {
                    result = default!;
                    return false;
                }

                result = deserialized;
                return true;
            }
            catch (JsonException)
            {
                result = default!;
                return false;
            }
            catch (NotSupportedException)
            {
                result = default!;
                return false;
            }
        }

        /// <summary>
        /// Begins a synchronous pattern match over the payload content type.
        /// </summary>
        /// <returns>A matcher to configure with <c>When</c> cases and execute.</returns>
        /// <example>
        /// <code>
        /// bool handled = envelope.Match()
        ///     .When&lt;ExampleContent&gt;(c => HandleExample(c))
        ///     .When&lt;OtherContent&gt;(c => HandleOther(c))
        ///     .Else(e => HandleUnknown(e))
        ///     .Execute();
        /// </code>
        /// </example>
        public ContentEnvelopeMatcher Match()
        {
            return new ContentEnvelopeMatcher(this);
        }

        /// <summary>
        /// Begins an asynchronous pattern match over the payload content type.
        /// </summary>
        /// <returns>A matcher to configure with <c>When</c> cases and execute.</returns>
        public ContentEnvelopeAsyncMatcher MatchAsync()
        {
            return new ContentEnvelopeAsyncMatcher(this);
        }

        /// <summary>
        /// Dispatches this envelope to the content handler for its payload content type.
        /// </summary>
        /// <param name="dispatcher">The content dispatcher.</param>
        /// <param name="handlerClass">The class of handler to dispatch to.</param>
        /// <param name="cancellationToken">A token to observe for cancellation.</param>
        /// <returns>A task which completes when the envelope has been handled.</returns>
        public ValueTask DispatchToHandlerAsync(IContentDispatcher<ContentEnvelope> dispatcher, string handlerClass, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dispatcher);

            return dispatcher.DispatchAsync(this, this.PayloadContentType, handlerClass, cancellationToken);
        }

        private static string ResolveContentType<T>(T payload, string? contentType)
        {
            if (!string.IsNullOrEmpty(contentType))
            {
                return contentType;
            }

            // Prefer instance-based discovery: it sees the payload's runtime type (and its
            // ContentType property), so a payload held through an interface- or base-typed
            // variable still resolves. The static type is only a fallback for null payloads.
            return payload is not null
                ? ContentTypes.GetContentType(payload)
                : ContentTypes.GetContentType<T>();
        }

        private static string ReadContentTypeProperty(JsonNode json)
        {
            return json[ContentTypeJson.ContentTypePropertyName] is JsonNode contentTypeNode
                ? contentTypeNode.GetValue<string>()
                : throw new ArgumentException($"The JSON does not contain a '{ContentTypeJson.ContentTypePropertyName}' property; pass the contentType argument explicitly.", nameof(json));
        }
    }
}
