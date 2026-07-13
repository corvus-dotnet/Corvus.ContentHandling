// <copyright file="Program.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Demo
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Spectre.Console;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            AnsiConsole.Write(new FigletText("Corvus").Color(Color.MediumPurple2));
            AnsiConsole.MarkupLine("[bold]Corvus.ContentHandling v5[/] — content-type driven registration, resolution and dispatch on [underline]keyed services[/]");
            AnsiConsole.WriteLine();

            await using ServiceProvider provider = DemoContainer.Build();

            (string Name, Func<IServiceProvider, Task> Run)[] demos =
            [
                ("Registration & the content registry", RegistrationDemo.RunAsync),
                ("Hierarchical fallback resolution", FallbackResolutionDemo.RunAsync),
                ("Handler dispatch", HandlerDispatchDemo.RunAsync),
                ("Polymorphic JSON serialization", PolymorphicJsonDemo.RunAsync),
                ("Content envelopes", ContentEnvelopeDemo.RunAsync),
            ];

            bool runAll = args.Contains("--all", StringComparer.OrdinalIgnoreCase) || !AnsiConsole.Profile.Capabilities.Interactive;
            if (runAll)
            {
                foreach ((string name, Func<IServiceProvider, Task> run) in demos)
                {
                    AnsiConsole.Write(new Rule($"[bold mediumpurple2]{name}[/]").LeftJustified());
                    AnsiConsole.WriteLine();
                    await run(provider);
                    AnsiConsole.WriteLine();
                }

                return;
            }

            const string exit = "Exit";
            while (true)
            {
                string choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Pick a [mediumpurple2]demo[/]:")
                        .AddChoices(demos.Select(d => d.Name).Append(exit)));

                if (choice == exit)
                {
                    return;
                }

                AnsiConsole.Write(new Rule($"[bold mediumpurple2]{choice}[/]").LeftJustified());
                AnsiConsole.WriteLine();
                await demos.Single(d => d.Name == choice).Run(provider);
                AnsiConsole.WriteLine();
            }
        }
    }
}
