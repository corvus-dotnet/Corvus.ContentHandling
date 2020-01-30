[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "RCS1102:Make class static.",
    Justification = "These test types need to be non-static for the content handling service to be able to use them",
    Scope = "namespaceanddescendants",
    Target = "Corvus.Extensions.Specs.Driver")]
