// <copyright file="SampleContent.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Tests
{
    using Corvus.ContentHandling;

    [ContentType(RegisteredContentType)]
    public class AttributedContent
    {
        public const string RegisteredContentType = "application/vnd.corvus.test.attributed";

        public string ContentType => RegisteredContentType;
    }

    public class LegacyFieldContent
    {
        public const string RegisteredContentType = "application/vnd.corvus.test.legacy";

        public string ContentType => RegisteredContentType;
    }

    public class ExplicitContent
    {
        public string? Value { get; set; }
    }

    public class OtherExplicitContent
    {
        public string? Value { get; set; }
    }

    public class ContentWithoutParameterlessCtor
    {
        public ContentWithoutParameterlessCtor(string value)
        {
            this.Value = value;
        }

        public string Value { get; }
    }

    public interface ISampleDependency
    {
        void Record(string message);
    }

    public class ContentWithDependency
    {
        public ContentWithDependency(ISampleDependency dependency)
        {
            this.Dependency = dependency;
        }

        public ISampleDependency Dependency { get; }
    }
}
