// <copyright file="PolymorphicJsonDemo.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Demo
{
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Corvus.Json.Serialization;
    using Microsoft.Extensions.DependencyInjection;
    using Spectre.Console;
    using Spectre.Console.Json;

    /// <summary>
    /// Content-type-discriminated polymorphic serialization with System.Text.Json.
    /// </summary>
    public static class PolymorphicJsonDemo
    {
        public static Task RunAsync(IServiceProvider provider)
        {
            JsonSerializerOptions options = provider.GetRequiredService<IJsonSerializerOptionsProvider>().Instance;

            AnsiConsole.MarkupLine("[bold]Serialize through the polymorphic target[/] ([teal]IDocument[/]) — the [teal]contentType[/] property is the discriminator:");
            IDocument document = new InvoiceDocument { Title = "Consulting Q3", Amount = 1250m, Currency = "GBP" };
            string json = JsonSerializer.Serialize(document, options);
            AnsiConsole.Write(new Panel(new JsonText(json)).Header("JsonSerializer.Serialize<IDocument>(invoice)").RoundedBorder());

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold]Deserialize back through the interface[/] — the registry picks the concrete type:");
            IDocument? roundTripped = JsonSerializer.Deserialize<IDocument>(json, options);
            AnsiConsole.MarkupLineInterpolated($"      → [green]{roundTripped!.GetType().Name}[/] with Title = '{roundTripped.Title}'");

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold]Fallback applies to deserialization too[/] — a document stamped with an unregistered subtype:");
            string fallbackJson = """{"contentType":"application/vnd.corvus.demo.report.pdf.v2","title":"Archived Report","author":"R. Corvus"}""";
            AnsiConsole.Write(new Panel(new JsonText(fallbackJson)).Header("input (contentType has no exact registration)").RoundedBorder());
            IDocument? fallbackDocument = JsonSerializer.Deserialize<IDocument>(fallbackJson, options);
            AnsiConsole.MarkupLineInterpolated($"      → deserialized as [green]{fallbackDocument!.GetType().Name}[/] via the [teal]...demo.report[/] registration");

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold]DI-constructed content[/] — [teal]PurchaseOrderDocument[/] is resolved from the container (keyed service), then populated:");
            string poJson = """{"contentType":"application/vnd.corvus.demo.purchaseorder","title":"Server Hardware"}""";
            var purchaseOrder = (PurchaseOrderDocument)JsonSerializer.Deserialize<IDocument>(poJson, options)!;
            AnsiConsole.MarkupLineInterpolated($"      → Title = '{purchaseOrder.Title}' (from JSON), Number = '{purchaseOrder.Number}' (from the injected IDocumentNumberGenerator)");

            return Task.CompletedTask;
        }
    }
}
