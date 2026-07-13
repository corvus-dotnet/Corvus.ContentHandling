// <copyright file="FallbackResolutionDemo.cs" company="Endjin Limited">
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
    /// The key feature: an unregistered content type falls back to progressively more general
    /// parents by removing trailing dot segments, preserving any "+suffix".
    /// </summary>
    public static class FallbackResolutionDemo
    {
        public static Task RunAsync(IServiceProvider provider)
        {
            AnsiConsole.MarkupLine("Only [teal]application/vnd.corvus.demo.report[/] is registered. Watch more specific requests fall back:");
            AnsiConsole.WriteLine();

            IContentRegistry registry = provider.GetRequiredService<IContentRegistry>();

            ShowProbe(registry, "application/vnd.corvus.demo.report.pdf.v2.draft");
            ShowProbe(registry, "application/vnd.corvus.demo.invoice.credit+audit");
            ShowProbe(registry, "application/vnd.corvus.demo.unknown.thing");

            AnsiConsole.MarkupLine("And resolution uses the fallback end-to-end:");
            object? resolved = provider.GetContent("application/vnd.corvus.demo.report.pdf.v2.draft");
            AnsiConsole.MarkupLineInterpolated($"      [teal]GetContent(\"application/vnd.corvus.demo.report.pdf.v2.draft\")[/] → [green]{resolved?.GetType().Name}[/]");

            return Task.CompletedTask;
        }

        private static void ShowProbe(IContentRegistry registry, string requested)
        {
            var tree = new Tree($"[bold]{requested.EscapeMarkup()}[/]");
            TreeNode? node = null;

            string candidate = requested;
            while (true)
            {
                bool hit = registry.TryGet(candidate, out ContentRegistration? registration);
                string label = hit
                    ? $"[green]✓ {candidate.EscapeMarkup()}[/]  →  [bold]{registration!.ImplementingType.Name}[/]"
                    : $"[grey]✗ {candidate.EscapeMarkup()}[/]";
                node = node is null ? tree.AddNode(label) : node.AddNode(label);

                if (hit)
                {
                    break;
                }

                // The same reduction the library applies: strip the +suffix, drop the last
                // dot segment of the stem, re-append the suffix.
                int plus = candidate.LastIndexOf('+');
                string suffix = plus >= 0 ? candidate[plus..] : string.Empty;
                string stem = plus >= 0 ? candidate[..plus] : candidate;
                int lastDot = stem.LastIndexOf('.');
                if (lastDot <= 0)
                {
                    node.AddNode("[red]✗ no registration found[/]");
                    break;
                }

                candidate = string.Concat(stem[..lastDot], suffix);
            }

            AnsiConsole.Write(tree);
            AnsiConsole.WriteLine();
        }
    }
}
