// <copyright file="Registration.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Extensions.Specs.Driver
{
    using System;
    using Corvus.ContentHandling;
    using TechTalk.SpecFlow;

    public class Registration
    {
        public Registration(string type, string explicitContentType, string registrationKind)
        {
            this.Type = TypeMap.GetTypeFor(type);

            this.ContentType = explicitContentType;

            this.Kind = Enum.Parse<RegistrationKind>(registrationKind);
        }

        public Type Type { get; }

        public string ContentType { get; }

        public RegistrationKind Kind { get; }

        public void Register(ScenarioContext context, ContentFactory factory, bool allowExceptions)
        {
            try
            {
                switch (this.Kind)
                {
                    case RegistrationKind.Scoped:
                        this.RegisterScoped(factory);
                        break;
                    case RegistrationKind.Transient:
                        this.RegisterTransient(factory);
                        break;
                    default:
                        this.RegisterSingleton(factory);
                        break;
                }
            }
            catch (Exception e)
            {
                context.Add("Exception", e);
                if (allowExceptions)
                {
                    throw;
                }
            }
        }

        private void RegisterSingleton(ContentFactory factory)
        {
            if (string.IsNullOrEmpty(this.ContentType))
            {
                factory.RegisterSingletonContent(this.Type);
            }
            else
            {
                factory.RegisterSingletonContent(this.ContentType, this.Type);
            }
        }

        private void RegisterTransient(ContentFactory factory)
        {
            if (string.IsNullOrEmpty(this.ContentType))
            {
                factory.RegisterTransientContent(this.Type);
            }
            else
            {
                factory.RegisterTransientContent(this.ContentType, this.Type);
            }
        }

        private void RegisterScoped(ContentFactory factory)
        {
            if (string.IsNullOrEmpty(this.ContentType))
            {
                factory.RegisterScopedContent(this.Type);
            }
            else
            {
                factory.RegisterScopedContent(this.ContentType, this.Type);
            }
        }
    }
}
