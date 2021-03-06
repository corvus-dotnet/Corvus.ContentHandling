﻿// <copyright file="GlobalSuppressions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "RCS1102:Make class static.",
    Justification = "These test types need to be non-static for the content handling service to be able to use them",
    Scope = "namespaceanddescendants",
    Target = "Corvus.Extensions.Specs.Driver")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1210:Using directives should be ordered alphabetically by namespace",
    Justification = "We need this until https://github.com/SpecFlowOSS/SpecFlow/issues/1828 is fixed")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1633:Using directives should be ordered alphabetically by namespace",
    Justification = "We need this until https://github.com/SpecFlowOSS/SpecFlow/issues/1828 is fixed")]
