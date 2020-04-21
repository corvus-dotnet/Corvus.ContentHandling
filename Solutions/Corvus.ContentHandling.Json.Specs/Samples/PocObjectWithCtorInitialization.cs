// <copyright file="PocObjectWithCtorInitialization.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    public class PocObjectWithCtorInitialization
    {
        public PocObjectWithCtorInitialization(string someValue)
        {
            this.SomeValue = someValue;
        }

        public string SomeValue { get; }
    }
}
