// <copyright file="DemoContainer.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Demo
{
    using System;
    using Corvus.ContentHandling.Json;
    using Microsoft.Extensions.DependencyInjection;
    using Spectre.Console;

    /// <summary>
    /// Builds the container shared by every demo section — the same wiring an application
    /// would use.
    /// </summary>
    public static class DemoContainer
    {
        public static ServiceProvider Build()
        {
            var services = new ServiceCollection();

            // Application services the content types and handlers depend on.
            services.AddSingleton<IDocumentNumberGenerator, SequentialDocumentNumberGenerator>();
            services.AddSingleton<INotificationService, ConsoleNotificationService>();

            // JSON support: aggregates registered JsonConverters into the application's
            // JsonSerializerOptions, and enables the { contentType, payload } envelope format.
            services.AddContentTypeBasedJsonSerializationSupport();

            // Properties (or root deserialization targets) of type IDocument use the
            // contentType discriminator to pick the concrete type.
            services.AddPolymorphicContentTarget<IDocument>();

            services.AddContentHandling(content => content

                // Serializer-constructed content: no service dependencies, registry-only.
                .AddSerializedContent<InvoiceDocument>()      // content type from [ContentType]
                .AddSerializedContent<ReportDocument>()       // content type from legacy static field

                // DI-managed content: a keyed transient, constructed by the container
                // (receiving IDocumentNumberGenerator) and then populated by the serializer.
                .AddTransientContent<PurchaseOrderDocument>()

                // Handlers, partitioned by handler class.
                .AddContentHandler<IDocument, InvoiceDocument>(
                    "console",
                    invoice => AnsiConsole.MarkupLineInterpolated(
                        $"      [green]lambda handler:[/] invoice '{invoice.Title}' for {invoice.Amount:0.00} {invoice.Currency}"))
                .AddContentHandler<IDocument, ReportDocument, ReportPublishedHandler>("console")
                .AddContentHandler<IDocument, InvoiceDocument, (string User, DateTimeOffset At)>(
                    InvoiceDocument.RegisteredContentType,
                    "audit",
                    (invoice, context, _) =>
                    {
                        AnsiConsole.MarkupLineInterpolated(
                            $"      [yellow]audit handler:[/] '{invoice.Title}' touched by {context.User} at {context.At:HH:mm:ss}");
                        return System.Threading.Tasks.ValueTask.CompletedTask;
                    })
                .AddContentHandlerWithResult<IDocument, InvoiceDocument, string>(
                    InvoiceDocument.RegisteredContentType,
                    "pricing",
                    invoice => $"{invoice.Amount * 1.2m:0.00} {invoice.Currency} (incl. VAT)")

                // An envelope handler: unwraps the ContentEnvelope payload before invoking.
                .AddContentEnvelopeHandler<InvoiceDocument>(
                    "notify",
                    invoice => AnsiConsole.MarkupLineInterpolated(
                        $"      [green]envelope handler:[/] received invoice '{invoice.Title}' from the envelope")));

            return services.BuildServiceProvider();
        }
    }
}
