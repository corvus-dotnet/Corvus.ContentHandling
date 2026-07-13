// <copyright file="RegistrationDemo.cs" company="Endjin Limited">
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
    /// Shows how content is registered and what the content registry records.
    /// </summary>
    public static class RegistrationDemo
    {
        public static Task RunAsync(IServiceProvider provider)
        {
            AnsiConsole.MarkupLine("Content registers as [bold]keyed services[/] (key = content type) via [teal]services.AddContentHandling(content => ...)[/].");
            AnsiConsole.MarkupLine("Content types come from a [teal][[ContentType]][/] attribute, the legacy [teal]RegisteredContentType[/] static field, or an explicit string.");
            AnsiConsole.WriteLine();

            IContentRegistry registry = provider.GetRequiredService<IContentRegistry>();

            Table table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("Content type")
                .AddColumn("Implementing type")
                .AddColumn("Construction")
                .AddColumn("Lifetime");

            foreach (string contentType in registry.ContentTypes)
            {
                registry.TryGet(contentType, out ContentRegistration? registration);
                table.AddRow(
                    $"[teal]{contentType.EscapeMarkup()}[/]",
                    registration!.ImplementingType.Name.EscapeMarkup(),
                    registration.Construction == ContentConstruction.FromServices
                        ? "[green]FromServices[/] (keyed service)"
                        : "[yellow]BySerializer[/] (registry only)",
                    registration.Lifetime?.ToString() ?? "[grey]n/a[/]");
            }

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[grey]Handlers appear alongside content, keyed by \"{contentType}+{handlerClass}\" — one keyspace, which is what makes fallback work for dispatch too.[/]");

            return Task.CompletedTask;
        }
    }
}
