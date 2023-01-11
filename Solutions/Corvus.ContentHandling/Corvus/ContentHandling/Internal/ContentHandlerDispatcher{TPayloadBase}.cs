// <copyright file="ContentHandlerDispatcher{TPayloadBase}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Internal
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Standard implementation of the <see cref="IContentHandlerDispatcher{TPayloadType}"/>.
    /// </summary>
    /// <typeparam name="TPayloadBase">The base type of the payload for which to dispatch the handlers.</typeparam>
    public class ContentHandlerDispatcher<TPayloadBase> : IContentHandlerDispatcher<TPayloadBase>
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentHandlerDispatcher{TPayloadType}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The current service provider for the dispatcher.</param>
        public ContentHandlerDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <inheritdoc/>
        public void DispatchPayloadToHandler(TPayloadBase payload, string contentType, string handlerClass)
        {
            this.Dispatch(payload, contentType, handlerClass);
        }

        /// <inheritdoc/>
        public void DispatchPayloadToHandler<TParam1>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1)
        {
            this.Dispatch(payload, contentType, handlerClass, param1);
        }

        /// <inheritdoc/>
        public void DispatchPayloadToHandler<TParam1, TParam2>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2)
        {
            this.Dispatch(payload, contentType, handlerClass, param1, param2);
        }

        /// <inheritdoc/>
        public void DispatchPayloadToHandler<TParam1, TParam2, TParam3>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            this.Dispatch(payload, contentType, handlerClass, param1, param2, param3);
        }

        /// <inheritdoc/>
        public Task DispatchPayloadToHandlerAsync(TPayloadBase payload, string contentType, string handlerClass)
        {
            return this.DispatchAsync(payload, contentType, handlerClass);
        }

        /// <inheritdoc/>
        public void DispatchPayloadToHandler(TPayloadBase payload, string handlerClass)
        {
            this.Dispatch(payload, ContentFactory.GetContentType(payload.GetType()), handlerClass);
        }

        /// <inheritdoc/>
        public void DispatchPayloadToHandler<TParam1>(TPayloadBase payload, string handlerClass, TParam1 param1)
        {
            this.Dispatch(payload, ContentFactory.GetContentType(payload.GetType()), handlerClass, param1);
        }

        /// <inheritdoc/>
        public void DispatchPayloadToHandler<TParam1, TParam2>(TPayloadBase payload, string handlerClass, TParam1 param1, TParam2 param2)
        {
            this.Dispatch(payload, ContentFactory.GetContentType(payload.GetType()), handlerClass, param1, param2);
        }

        /// <inheritdoc/>
        public void DispatchPayloadToHandler<TParam1, TParam2, TParam3>(TPayloadBase payload, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            this.Dispatch(payload, ContentFactory.GetContentType(payload.GetType()), handlerClass, param1, param2, param3);
        }

        /// <inheritdoc/>
        public Task DispatchPayloadToHandlerAsync(TPayloadBase payload, string handlerClass)
        {
            return this.DispatchAsync(payload, ContentFactory.GetContentType(payload.GetType()), handlerClass);
        }

        /// <inheritdoc/>
        public Task DispatchPayloadToHandlerAsync<TParam1>(TPayloadBase payload, string handlerClass, TParam1 param1)
        {
            return this.DispatchAsync(payload, ContentFactory.GetContentType(payload.GetType()), handlerClass, param1);
        }

        /// <inheritdoc/>
        public Task DispatchPayloadToHandlerAsync<TParam1, TParam2>(TPayloadBase payload, string handlerClass, TParam1 param1, TParam2 param2)
        {
            return this.DispatchAsync(payload, ContentFactory.GetContentType(payload.GetType()), handlerClass, param1, param2);
        }

        /// <inheritdoc/>
        public Task DispatchPayloadToHandlerAsync<TParam1, TParam2, TParam3>(TPayloadBase payload, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            return this.DispatchAsync(payload, ContentFactory.GetContentType(payload.GetType()), handlerClass, param1, param2, param3);
        }

        /// <inheritdoc/>
        public TResult DispatchPayloadToHandler<TResult>(TPayloadBase payload, string handlerClass)
        {
            return this.Dispatch<TResult>(payload, ContentFactory.GetContentType(payload.GetType()), handlerClass);
        }

        /// <inheritdoc/>
        public TResult DispatchPayloadToHandler<TParam1, TResult>(TPayloadBase payload, string handlerClass, TParam1 param1)
        {
            return this.Dispatch<TParam1, TResult>(payload, ContentFactory.GetContentType(payload.GetType()), handlerClass, param1);
        }

        /// <inheritdoc/>
        public TResult DispatchPayloadToHandler<TParam1, TParam2, TResult>(TPayloadBase payload, string handlerClass, TParam1 param1, TParam2 param2)
        {
            return this.Dispatch<TParam1, TParam2, TResult>(payload, ContentFactory.GetContentType(payload.GetType()), handlerClass, param1, param2);
        }

        /// <inheritdoc/>
        public TResult DispatchPayloadToHandler<TParam1, TParam2, TParam3, TResult>(TPayloadBase payload, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            return this.Dispatch<TParam1, TParam2, TParam3, TResult>(payload, ContentFactory.GetContentType(payload.GetType()), handlerClass, param1, param2, param3);
        }

        /// <inheritdoc/>
        public TResult DispatchPayloadToHandler<TResult>(TPayloadBase payload, string contentType, string handlerClass)
        {
            return this.Dispatch<TResult>(payload, contentType, handlerClass);
        }

        /// <inheritdoc/>
        public TResult DispatchPayloadToHandler<TParam1, TResult>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1)
        {
            return this.Dispatch<TParam1, TResult>(payload, contentType, handlerClass, param1);
        }

        /// <inheritdoc/>
        public TResult DispatchPayloadToHandler<TParam1, TParam2, TResult>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2)
        {
            return this.Dispatch<TParam1, TParam2, TResult>(payload, contentType, handlerClass, param1, param2);
        }

        /// <inheritdoc/>
        public TResult DispatchPayloadToHandler<TParam1, TParam2, TParam3, TResult>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            return this.Dispatch<TParam1, TParam2, TParam3, TResult>(payload, contentType, handlerClass, param1, param2, param3);
        }

        /// <inheritdoc/>
        public Task<TResult> DispatchPayloadToHandlerAsync<TResult>(TPayloadBase payload, string handlerClass)
        {
            return this.DispatchAsync<TResult>(payload, ContentFactory.GetContentType(payload.GetType()), handlerClass);
        }

        /// <inheritdoc/>
        public Task<TResult> DispatchPayloadToHandlerAsync<TParam1, TResult>(TPayloadBase payload, string handlerClass, TParam1 param1)
        {
            return this.DispatchAsync<TParam1, TResult>(payload, ContentFactory.GetContentType(payload.GetType()), handlerClass, param1);
        }

        /// <inheritdoc/>
        public Task<TResult> DispatchPayloadToHandlerAsync<TParam1, TParam2, TResult>(TPayloadBase payload, string handlerClass, TParam1 param1, TParam2 param2)
        {
            return this.DispatchAsync<TParam1, TParam2, TResult>(payload, ContentFactory.GetContentType(payload.GetType()), handlerClass, param1, param2);
        }

        /// <inheritdoc/>
        public Task<TResult> DispatchPayloadToHandlerAsync<TParam1, TParam2, TParam3, TResult>(TPayloadBase payload, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            return this.DispatchAsync<TParam1, TParam2, TParam3, TResult>(payload, ContentFactory.GetContentType(payload.GetType()), handlerClass, param1, param2, param3);
        }

        /// <inheritdoc/>
        public Task<TResult> DispatchPayloadToHandlerAsync<TResult>(TPayloadBase payload, string contentType, string handlerClass)
        {
            return this.DispatchAsync<TResult>(payload, contentType, handlerClass);
        }

        /// <inheritdoc/>
        public Task<TResult> DispatchPayloadToHandlerAsync<TParam1, TResult>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1)
        {
            return this.DispatchAsync<TParam1, TResult>(payload, contentType, handlerClass, param1);
        }

        /// <inheritdoc/>
        public Task<TResult> DispatchPayloadToHandlerAsync<TParam1, TParam2, TResult>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2)
        {
            return this.DispatchAsync<TParam1, TParam2, TResult>(payload, contentType, handlerClass, param1, param2);
        }

        /// <inheritdoc/>
        public Task<TResult> DispatchPayloadToHandlerAsync<TParam1, TParam2, TParam3, TResult>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            return this.DispatchAsync<TParam1, TParam2, TParam3, TResult>(payload, contentType, handlerClass, param1, param2, param3);
        }

        private void Dispatch(TPayloadBase payload, string contentType, string handlerClass)
        {
            IContentHandler<TPayloadBase> handler = this.serviceProvider.GetRequiredContent<IContentHandler<TPayloadBase>>(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            handler.Handle(payload);
        }

        private void Dispatch<TParam1>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1)
        {
            IContentHandler<TPayloadBase, TParam1> handler = this.serviceProvider.GetRequiredContent<IContentHandler<TPayloadBase, TParam1>>(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            handler.Handle(payload, param1);
        }

        private void Dispatch<TParam1, TParam2>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2)
        {
            IContentHandler<TPayloadBase, TParam1, TParam2> handler = this.serviceProvider.GetRequiredContent<IContentHandler<TPayloadBase, TParam1, TParam2>>(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            handler.Handle(payload, param1, param2);
        }

        private void Dispatch<TParam1, TParam2, TParam3>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            IContentHandler<TPayloadBase, TParam1, TParam2, TParam3> handler = this.serviceProvider.GetRequiredContent<IContentHandler<TPayloadBase, TParam1, TParam2, TParam3>>(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            handler.Handle(payload, param1, param2, param3);
        }

        private async Task DispatchAsync(TPayloadBase payload, string contentType, string handlerClass)
        {
            IAsyncContentHandler<TPayloadBase> handler = this.serviceProvider.GetRequiredContent<IAsyncContentHandler<TPayloadBase>>(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            await handler.HandleAsync(payload).ConfigureAwait(false);
        }

        private async Task DispatchAsync<TParam1>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1)
        {
            IAsyncContentHandler<TPayloadBase, TParam1> handler = this.serviceProvider.GetRequiredContent<IAsyncContentHandler<TPayloadBase, TParam1>>(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            await handler.HandleAsync(payload, param1).ConfigureAwait(false);
        }

        private async Task DispatchAsync<TParam1, TParam2>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2)
        {
            IAsyncContentHandler<TPayloadBase, TParam1, TParam2> handler = this.serviceProvider.GetRequiredContent<IAsyncContentHandler<TPayloadBase, TParam1, TParam2>>(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            await handler.HandleAsync(payload, param1, param2).ConfigureAwait(false);
        }

        private async Task DispatchAsync<TParam1, TParam2, TParam3>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            IAsyncContentHandler<TPayloadBase, TParam1, TParam2, TParam3> handler = this.serviceProvider.GetRequiredContent<IAsyncContentHandler<TPayloadBase, TParam1, TParam2, TParam3>>(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            await handler.HandleAsync(payload, param1, param2, param3).ConfigureAwait(false);
        }

        private TResult Dispatch<TResult>(TPayloadBase payload, string contentType, string handlerClass)
        {
            IContentHandlerWithResult<TPayloadBase, TResult> handler = this.serviceProvider.GetRequiredContent<IContentHandlerWithResult<TPayloadBase, TResult>>(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return handler.Handle(payload);
        }

        private TResult Dispatch<TParam1, TResult>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1)
        {
            IContentHandlerWithResult<TPayloadBase, TParam1, TResult> handler = this.serviceProvider.GetRequiredContent<IContentHandlerWithResult<TPayloadBase, TParam1, TResult>>(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return handler.Handle(payload, param1);
        }

        private TResult Dispatch<TParam1, TParam2, TResult>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2)
        {
            IContentHandlerWithResult<TPayloadBase, TParam1, TParam2, TResult> handler = this.serviceProvider.GetRequiredContent<IContentHandlerWithResult<TPayloadBase, TParam1, TParam2, TResult>>(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return handler.Handle(payload, param1, param2);
        }

        private TResult Dispatch<TParam1, TParam2, TParam3, TResult>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            IContentHandlerWithResult<TPayloadBase, TParam1, TParam2, TParam3, TResult> handler = this.serviceProvider.GetRequiredContent<IContentHandlerWithResult<TPayloadBase, TParam1, TParam2, TParam3, TResult>>(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return handler.Handle(payload, param1, param2, param3);
        }

        private Task<TResult> DispatchAsync<TResult>(TPayloadBase payload, string contentType, string handlerClass)
        {
            IAsyncContentHandlerWithResult<TPayloadBase, TResult> handler = this.serviceProvider.GetRequiredContent<IAsyncContentHandlerWithResult<TPayloadBase, TResult>>(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return handler.HandleAsync(payload);
        }

        private Task<TResult> DispatchAsync<TParam1, TResult>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1)
        {
            IAsyncContentHandlerWithResult<TPayloadBase, TParam1, TResult> handler = this.serviceProvider.GetRequiredContent<IAsyncContentHandlerWithResult<TPayloadBase, TParam1, TResult>>(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return handler.HandleAsync(payload, param1);
        }

        private Task<TResult> DispatchAsync<TParam1, TParam2, TResult>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2)
        {
            IAsyncContentHandlerWithResult<TPayloadBase, TParam1, TParam2, TResult> handler = this.serviceProvider.GetRequiredContent<IAsyncContentHandlerWithResult<TPayloadBase, TParam1, TParam2, TResult>>(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return handler.HandleAsync(payload, param1, param2);
        }

        private Task<TResult> DispatchAsync<TParam1, TParam2, TParam3, TResult>(TPayloadBase payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            IAsyncContentHandlerWithResult<TPayloadBase, TParam1, TParam2, TParam3, TResult> handler = this.serviceProvider.GetRequiredContent<IAsyncContentHandlerWithResult<TPayloadBase, TParam1, TParam2, TParam3, TResult>>(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return handler.HandleAsync(payload, param1, param2, param3);
        }
    }
}