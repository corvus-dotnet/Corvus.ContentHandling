// <copyright file="CoreClrShim.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Internal
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Shim for APIs available only on CoreCLR.
    /// </summary>
    internal static class CoreClrShim
    {
        /// <summary>
        /// Gets a value indicating whether we are running on CoreCLR / netstandard.
        /// </summary>
        internal static bool IsRunningOnCoreClr => AssemblyLoadContext.Type != null;

        /// <summary>
        /// Try to get a type from an assembly qualified name.
        /// </summary>
        /// <param name="assemblyQualifiedName">The assembly qualified name of the type.</param>
        /// <returns>The type or null.</returns>
        public static Type TryGetType(string assemblyQualifiedName)
        {
            try
            {
                // Note that throwOnError=false only suppresses some exceptions, not all.
                return Type.GetType(assemblyQualifiedName, throwOnError: false);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Create a delegate from a methodinfo.
        /// </summary>
        /// <typeparam name="T">The type of the delegate.</typeparam>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> for which to create the delegate.</param>
        /// <returns>The delegate.</returns>
        public static T CreateDelegate<T>(this MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                return default;
            }

            return (T)(object)methodInfo.CreateDelegate(typeof(T));
        }

        /// <summary>
        /// A shium for the assembly load context.
        /// </summary>
        internal static class AssemblyLoadContext
        {
            /// <summary>
            /// The runtime loader type.
            /// </summary>
            internal static readonly Type Type = TryGetType(
               "System.Runtime.Loader.AssemblyLoadContext, System.Runtime.Loader, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
        }

        /// <summary>
        /// A shim for AppContext.
        /// </summary>
        internal static class AppContext
        {
            /// <summary>
            /// A shim for Type.
            /// </summary>
            internal static readonly Type Type = TryGetType(
                "System.AppContext, System.AppContext, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

            /// <summary>
            /// A shim for GetData.
            /// </summary>
            /// <remarks>
            ///  only available in netstandard 1.6+.
            /// </remarks>
            internal static readonly Func<string, object> GetData =
                Type.GetTypeInfo().GetDeclaredMethod("GetData")?.CreateDelegate<Func<string, object>>();
        }
    }
}