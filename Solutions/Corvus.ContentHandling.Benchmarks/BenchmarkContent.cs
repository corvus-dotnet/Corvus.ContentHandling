// <copyright file="BenchmarkContent.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

#pragma warning disable SA1402, SA1649 // benchmark sample types are grouped in one file

namespace Corvus.ContentHandling.Benchmarks
{
    using System.Threading;
    using System.Threading.Tasks;
    using Corvus.ContentHandling;

    // The sample types are IDENTICAL (names, content types, shapes) in the v4 and v5
    // benchmark projects so the scenarios measure like-for-like work.
    public class ContentType01
    {
        public const string RegisteredContentType = "application/vnd.corvus.benchmark.content01";

        public string ContentType => RegisteredContentType;
    }

    public class ContentType02
    {
        public const string RegisteredContentType = "application/vnd.corvus.benchmark.content02";

        public string ContentType => RegisteredContentType;
    }

    public class ContentType03
    {
        public const string RegisteredContentType = "application/vnd.corvus.benchmark.content03";

        public string ContentType => RegisteredContentType;
    }

    public class ContentType04
    {
        public const string RegisteredContentType = "application/vnd.corvus.benchmark.content04";

        public string ContentType => RegisteredContentType;
    }

    public class ContentType05
    {
        public const string RegisteredContentType = "application/vnd.corvus.benchmark.content05";

        public string ContentType => RegisteredContentType;
    }

    public class ContentType06
    {
        public const string RegisteredContentType = "application/vnd.corvus.benchmark.content06";

        public string ContentType => RegisteredContentType;
    }

    public class ContentType07
    {
        public const string RegisteredContentType = "application/vnd.corvus.benchmark.content07";

        public string ContentType => RegisteredContentType;
    }

    public class ContentType08
    {
        public const string RegisteredContentType = "application/vnd.corvus.benchmark.content08";

        public string ContentType => RegisteredContentType;
    }

    public class ContentType09
    {
        public const string RegisteredContentType = "application/vnd.corvus.benchmark.content09";

        public string ContentType => RegisteredContentType;
    }

    public class ContentType10
    {
        public const string RegisteredContentType = "application/vnd.corvus.benchmark.content10";

        public string ContentType => RegisteredContentType;
    }

    public abstract class BenchmarkPayload
    {
        public abstract string ContentType { get; }

        public string? Id { get; set; }
    }

    public class CreatedPayload : BenchmarkPayload
    {
        public const string RegisteredContentType = "application/vnd.corvus.benchmark.payload.created";

        public override string ContentType => RegisteredContentType;
    }

    public class CreatedSpecialPayload : CreatedPayload
    {
        public new const string RegisteredContentType = "application/vnd.corvus.benchmark.payload.created.special";

        public override string ContentType => RegisteredContentType;
    }

    public class CreatedPayloadClassHandler : IContentHandler<CreatedPayload>
    {
        public static volatile string? LastHandled;

        public ValueTask HandleAsync(CreatedPayload payload, CancellationToken cancellationToken = default)
        {
            LastHandled = payload.Id;
            return ValueTask.CompletedTask;
        }
    }

    public interface IBenchmarkContent
    {
        string ContentType { get; }
    }

    public class BenchmarkContentA : IBenchmarkContent
    {
        public const string RegisteredContentType = "application/vnd.corvus.benchmark.json.contenta";

        public string ContentType => RegisteredContentType;

        public string? SomeValue { get; set; }

        public int Count { get; set; }
    }

    public interface IBenchmarkService
    {
        string Describe();
    }

    public class NullBenchmarkService : IBenchmarkService
    {
        public string Describe() => "benchmark";
    }

    public class DiInitializedBenchmarkContent : IBenchmarkContent
    {
        public const string RegisteredContentType = "application/vnd.corvus.benchmark.json.diinitialized";

        private readonly IBenchmarkService service;

        public DiInitializedBenchmarkContent(IBenchmarkService service)
        {
            this.service = service;
        }

        public string ContentType => RegisteredContentType;

        public string? SomeValue { get; set; }
    }
}
