// <copyright file="HandlerDispatchDemo.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Demo
{
    using System;
    using System.Threading.Tasks;
    using Corvus.ContentHandling;
    using Microsoft.Extensions.DependencyInjection;
    using Spectre.Console;

    /// <summary>
    /// Dispatches payloads to handlers registered for (content type, handler class) pairs.
    /// </summary>
    public static class HandlerDispatchDemo
    {
        public static async Task RunAsync(IServiceProvider provider)
        {
            IContentDispatcher<IDocument> dispatcher = provider.GetRequiredService<IContentDispatcher<IDocument>>();

            var invoice = new InvoiceDocument { Title = "Consulting Q3", Amount = 1250m, Currency = "GBP" };
            var report = new ReportDocument { Title = "Annual Review", Author = "R. Corvus" };
            var draft = new DraftReportDocument { Title = "Draft Findings", Author = "R. Corvus" };

            AnsiConsole.MarkupLine("[bold]Lambda handler[/] — content type discovered from the payload:");
            await dispatcher.DispatchAsync(invoice, "console");

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold]Class handler with an injected dependency[/] ([teal]ReportPublishedHandler(INotificationService)[/]):");
            await dispatcher.DispatchAsync(report, "console");

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold]Fallback dispatch[/] — no handler for [teal]...report.draft[/]; the [teal]...report[/] handler takes it:");
            await dispatcher.DispatchAsync(draft, "console");

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold]Context handler[/] — one [teal]TContext[/] type parameter replaces the old 0–3 loose parameters (tuples for many values):");
            await dispatcher.DispatchAsync(invoice, ("howard", DateTimeOffset.Now), "audit");

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold]Result handler[/] — [teal]DispatchWithResultAsync<TResult>[/]:");
            string price = await dispatcher.DispatchWithResultAsync<string>(invoice, "pricing");
            AnsiConsole.MarkupLineInterpolated($"      [green]pricing handler returned:[/] {price}");

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[grey]In v4, every lambda handler above required a unique wrapper type compiled with Roslyn at runtime; keyed services resolve by (type, key), so v5 needs none of that.[/]");
        }
    }
}
