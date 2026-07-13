// <copyright file="ContentEnvelopeDemo.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Demo
{
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Corvus.ContentHandling;
    using Corvus.ContentHandling.Json;
    using Corvus.Json.Serialization;
    using Microsoft.Extensions.DependencyInjection;
    using Spectre.Console;
    using Spectre.Console.Json;

    /// <summary>
    /// Content envelopes: heterogeneous payloads through one channel, dispatched by content type.
    /// </summary>
    public static class ContentEnvelopeDemo
    {
        public static async Task RunAsync(IServiceProvider provider)
        {
            JsonSerializerOptions options = provider.GetRequiredService<IJsonSerializerOptionsProvider>().Instance;

            var invoice = new InvoiceDocument { Title = "Consulting Q3", Amount = 1250m, Currency = "GBP" };
            var envelope = ContentEnvelope.FromPayload(invoice, options);

            AnsiConsole.MarkupLine("[bold]The envelope wire format[/] — unchanged from v4, so existing documents round-trip:");
            string json = JsonSerializer.Serialize(envelope, options);
            AnsiConsole.Write(new Panel(new JsonText(json)).Header("ContentEnvelope.FromPayload(invoice)").RoundedBorder());

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold]The fluent matcher[/] — replaces v4's thirty fixed-arity Match overloads:");
            ContentEnvelope received = JsonSerializer.Deserialize<ContentEnvelope>(json, options)!;
            bool matched = received.Match()
                .When<ReportDocument>(r => AnsiConsole.MarkupLineInterpolated($"      report case: {r.Title}"))
                .When<InvoiceDocument>(i => AnsiConsole.MarkupLineInterpolated($"      [green]invoice case:[/] '{i.Title}' for {i.Amount:0.00} {i.Currency}"))
                .Else(e => AnsiConsole.MarkupLineInterpolated($"      unmatched: {e.PayloadContentType}"))
                .Execute();
            AnsiConsole.MarkupLineInterpolated($"      [grey]Execute() returned {matched}[/]");

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold]Envelope handler dispatch[/] — the handler receives the [italic]unwrapped[/] payload:");
            await received.DispatchToHandlerAsync(provider.GetRequiredService<IContentDispatcher<ContentEnvelope>>(), "notify");
        }
    }
}
