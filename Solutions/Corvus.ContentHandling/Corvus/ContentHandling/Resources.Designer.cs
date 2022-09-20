﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Corvus.ContentHandling {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Corvus.ContentHandling.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The implementing type ({0}) for content type {1} uses constructor-based deserialization, so you cannot instantiate it in this way.
        /// </summary>
        internal static string ImplementingTypeNoDefaultCtor {
            get {
                return ResourceManager.GetString("ImplementingTypeNoDefaultCtor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The service named &apos;{0}&apos; is not assignable to type &apos;{1}&apos;..
        /// </summary>
        internal static string NamedServiceNotOfType {
            get {
                return ResourceManager.GetString("NamedServiceNotOfType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A type with the name &apos;{0}&apos; has already been added to the content factory..
        /// </summary>
        internal static string NamedTypeAlreadyAdded {
            get {
                return ResourceManager.GetString("NamedTypeAlreadyAdded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No service with has been registered with the name &apos;{0}&apos;..
        /// </summary>
        internal static string NoNamedServiceRegistered {
            get {
                return ResourceManager.GetString("NoNamedServiceRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No service for type &apos;{0}&apos; has been registered..
        /// </summary>
        internal static string NoServiceRegistered {
            get {
                return ResourceManager.GetString("NoServiceRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{0}&apos; must provide a static field &apos;RegisteredContentType&apos; of type &apos;String&apos;, or you must use the overload of this method that supplies a name..
        /// </summary>
        internal static string TypeNeedsStaticStringContentTypeField {
            get {
                return ResourceManager.GetString("TypeNeedsStaticStringContentTypeField", resourceCulture);
            }
        }
    }
}
