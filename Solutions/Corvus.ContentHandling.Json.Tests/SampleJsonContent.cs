// <copyright file="SampleJsonContent.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Tests
{
    using System;
    using System.Collections.Generic;

    public interface ISomeContentInterface
    {
        string ContentType { get; }
    }

    public abstract class SomeContentAbstractBase
    {
        public abstract string ContentType { get; }
    }

    public class SomeContentBase
    {
        public const string RegisteredContentType = "application/vnd.corvus.jsontest.base";

        public virtual string ContentType => RegisteredContentType;

        public string? BaseValue { get; set; }
    }

    public class InterfaceContent : ISomeContentInterface
    {
        public const string RegisteredContentType = "application/vnd.corvus.jsontest.interfacecontent";

        public string ContentType => RegisteredContentType;

        public string? SomeValue { get; set; }
    }

    public class InterfaceContentWithChild : ISomeContentInterface
    {
        public const string RegisteredContentType = "application/vnd.corvus.jsontest.interfacecontentwithchild";

        public string ContentType => RegisteredContentType;

        public string? SomeValue { get; set; }

        public ISomeContentInterface? Child { get; set; }
    }

    public class InterfaceContentWithPocChild : ISomeContentInterface
    {
        public const string RegisteredContentType = "application/vnd.corvus.jsontest.interfacecontentwithpocchild";

        public string ContentType => RegisteredContentType;

        public PocObject? Child { get; set; }
    }

    public class AbstractBaseContent : SomeContentAbstractBase
    {
        public const string RegisteredContentType = "application/vnd.corvus.jsontest.abstractbasecontent";

        public override string ContentType => RegisteredContentType;

        public string? SomeValue { get; set; }
    }

    public class DerivedFromBaseContent : SomeContentBase
    {
        public new const string RegisteredContentType = "application/vnd.corvus.jsontest.base.derived";

        public override string ContentType => RegisteredContentType;

        public string? DerivedValue { get; set; }
    }

    public class ContentWithDictionary : ISomeContentInterface
    {
        public const string RegisteredContentType = "application/vnd.corvus.jsontest.contentwithdictionary";

        public string ContentType => RegisteredContentType;

        public Dictionary<string, string>? Values { get; set; }
    }

    public class ContentWithEnum : ISomeContentInterface
    {
        public const string RegisteredContentType = "application/vnd.corvus.jsontest.contentwithenum";

        public string ContentType => RegisteredContentType;

        public SomeEnum EnumValue { get; set; }
    }

    public enum SomeEnum
    {
        First = 0,
        Second = 1,
        Third = 2,
    }

    public class PocObject
    {
        public string? Name { get; set; }
    }

    public class PocObjectCtorInitialized
    {
        public PocObjectCtorInitialized(string name, int count)
        {
            this.Name = name;
            this.Count = count;
        }

        public string Name { get; }

        public int Count { get; }
    }

    public class InterfaceContentWithCtorInitializedPocChild : ISomeContentInterface
    {
        public const string RegisteredContentType = "application/vnd.corvus.jsontest.interfacecontentwithctorpocchild";

        public string ContentType => RegisteredContentType;

        public PocObjectCtorInitialized? Child { get; set; }
    }

    public interface ISampleService
    {
        string Describe();
    }

    public class DiInitializedContent : ISomeContentInterface
    {
        public const string RegisteredContentType = "application/vnd.corvus.jsontest.diinitializedcontent";

        public DiInitializedContent(ISampleService service)
        {
            this.Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public string ContentType => RegisteredContentType;

        public string? SomeValue { get; set; }

        public ISomeContentInterface? Child { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ISampleService Service { get; }
    }
}
