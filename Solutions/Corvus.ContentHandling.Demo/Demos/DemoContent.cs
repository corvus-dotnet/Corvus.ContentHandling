// <copyright file="DemoContent.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Demo
{
    using System.Threading;
    using System.Threading.Tasks;
    using Corvus.ContentHandling;
    using Spectre.Console;

    /// <summary>
    /// The demo domain: documents identified by content type.
    /// </summary>
    public interface IDocument
    {
        string ContentType { get; }

        string? Title { get; }
    }

    // Content type declared with the v5 [ContentType] attribute.
    [ContentType(RegisteredContentType)]
    public class InvoiceDocument : IDocument
    {
        public const string RegisteredContentType = "application/vnd.corvus.demo.invoice";

        public string ContentType => RegisteredContentType;

        public string? Title { get; set; }

        public decimal Amount { get; set; }

        public string? Currency { get; set; }
    }

    // Content type declared with the legacy static-field convention (v4 types work unchanged).
    public class ReportDocument : IDocument
    {
        public const string RegisteredContentType = "application/vnd.corvus.demo.report";

        public virtual string ContentType => RegisteredContentType;

        public string? Title { get; set; }

        public string? Author { get; set; }
    }

    // A more specific report; nothing is registered for its content type, so resolution and
    // dispatch fall back to the ReportDocument registration.
    public class DraftReportDocument : ReportDocument
    {
        public new const string RegisteredContentType = "application/vnd.corvus.demo.report.draft";

        public override string ContentType => RegisteredContentType;
    }

    public interface IDocumentNumberGenerator
    {
        string Next();
    }

    public class SequentialDocumentNumberGenerator : IDocumentNumberGenerator
    {
        private int counter;

        public string Next() => $"PO-{Interlocked.Increment(ref this.counter):D5}";
    }

    // Constructed through DI (keyed service) and then populated by the JSON deserializer.
    [ContentType(RegisteredContentType)]
    public class PurchaseOrderDocument : IDocument
    {
        public const string RegisteredContentType = "application/vnd.corvus.demo.purchaseorder";

        public PurchaseOrderDocument(IDocumentNumberGenerator numberGenerator)
        {
            this.Number = numberGenerator.Next();
        }

        public string ContentType => RegisteredContentType;

        public string? Title { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string Number { get; }
    }

    public interface INotificationService
    {
        void Notify(string message);
    }

    public class ConsoleNotificationService : INotificationService
    {
        public void Notify(string message)
        {
            AnsiConsole.MarkupLineInterpolated($"      [grey]notification service:[/] [italic]{message}[/]");
        }
    }

    // A class-based handler with an injected dependency.
    public class ReportPublishedHandler : IContentHandler<ReportDocument>
    {
        private readonly INotificationService notifications;

        public ReportPublishedHandler(INotificationService notifications)
        {
            this.notifications = notifications;
        }

        public ValueTask HandleAsync(ReportDocument payload, CancellationToken cancellationToken = default)
        {
            this.notifications.Notify($"report '{payload.Title}' by {payload.Author ?? "unknown"} was published");
            return ValueTask.CompletedTask;
        }
    }
}
