//   OData .NET Libraries ver. 6.9
//   Copyright (c) Microsoft Corporation
//   All rights reserved. 
//   MIT License
//   Permission is hereby granted, free of charge, to any person obtaining a copy of
//   this software and associated documentation files (the "Software"), to deal in
//   the Software without restriction, including without limitation the rights to use,
//   copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the
//   Software, and to permit persons to whom the Software is furnished to do so,
//   subject to the following conditions:

//   The above copyright notice and this permission notice shall be included in all
//   copies or substantial portions of the Software.

//   THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//   FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//   COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//   IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//   CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Microsoft.OData.Core
{
    #region Namespaces
    using System;
    using Microsoft.OData.Edm;
    #endregion Namespaces

    /// <summary>
    /// Class that captures all the information needed to make readers behave
    /// differently when used inside of WCF Data Services or outside.
    /// </summary>
    internal sealed class ODataReaderBehavior
    {
        /// <summary>The default reader behavior for the OData library.</summary>
        private static readonly ODataReaderBehavior defaultReaderBehavior =
            new ODataReaderBehavior(
                ODataBehaviorKind.Default,
                ODataBehaviorKind.Default,
                /*allowDuplicatePropertyNames*/ false,
                /*typeResolver*/ null);

        /// <summary>The API behavior kind of this behavior.</summary>
        private readonly ODataBehaviorKind apiBehaviorKind;

        /// <summary>Custom type resolver used by the WCF DS Client.</summary>
        /// <remarks>
        /// This function is used instead of calling the IEdmModel.FindType.
        /// The first parameter to the function is the expected type (the type infered from the parent property or specified by the external caller).
        /// The second parameter is the type name from the payload.
        /// The function should return the resolved type, or null if no such type was found.
        /// </remarks>
        private readonly Func<IEdmType, string, IEdmType> typeResolver;

        /// <summary>
        /// If set to true, allows the writers to write duplicate properties of entries and 
        /// complex values (i.e., properties that have the same name). Defaults to 'false'.
        /// </summary>
        private bool allowDuplicatePropertyNames;

        /// <summary>The format behavior kind of this behavior.</summary>
        private ODataBehaviorKind formatBehaviorKind;

        /// <summary>Determines whether operations bound to the given type must be container qualified.</summary>
        private Func<IEdmEntityType, bool> operationsBoundToEntityTypeMustBeContainerQualified;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="formatBehaviorKind">The format behavior kind of this behavior.</param>
        /// <param name="apiBehaviorKind">The API behavior kind of this behavior.</param>
        /// <param name="allowDuplicatePropertyNames">
        /// If set to true, allows the writers to write duplicate properties of entries and 
        /// complex values (i.e., properties that have the same name). Defaults to 'false'.
        /// </param>
        /// <param name="typeResolver">Custom type resolver which takes both expected type and type name.
        /// This function is used instead of the IEdmModel.FindType is it's specified.
        /// The first parameter to the function is the expected type (the type infered from the parent property or specified by the external caller).
        /// The second parameter is the type name from the payload.
        /// The function should return the resolved type, or null if no such type was found.</param>
        private ODataReaderBehavior(
            ODataBehaviorKind formatBehaviorKind,
            ODataBehaviorKind apiBehaviorKind,
            bool allowDuplicatePropertyNames,
            Func<IEdmType, string, IEdmType> typeResolver)
        {
            this.formatBehaviorKind = formatBehaviorKind;
            this.apiBehaviorKind = apiBehaviorKind;
            this.allowDuplicatePropertyNames = allowDuplicatePropertyNames;
            this.typeResolver = typeResolver;
        }

        /// <summary>
        /// Get the default reader behavior for the OData library.
        /// </summary>
        /// <returns>The default reader behavior.</returns>
        internal static ODataReaderBehavior DefaultBehavior
        {
            get
            {
                return defaultReaderBehavior;
            }
        }

        /// <summary>
        /// If set to true, allows the writers to write duplicate properties of entries and 
        /// complex values (i.e., properties that have the same name). Defaults to 'false'.
        /// </summary>
        /// <remarks>
        /// Independently of this setting duplicate property names are never allowed if one 
        /// of the duplicate property names refers to a named stream property, 
        /// an association link or a collection.
        /// </remarks>
        internal bool AllowDuplicatePropertyNames
        {
            get 
            {
                return this.allowDuplicatePropertyNames; 
            }
        }

        /// <summary>
        /// Custom type resolver used by the WCF DS Client.
        /// </summary>
        internal Func<IEdmType, string, IEdmType> TypeResolver
        {
            get
            {
                return this.typeResolver;
            }
        }

        /// <summary>The format behavior kind of this behavior.</summary>
        internal ODataBehaviorKind FormatBehaviorKind
        {
            get 
            {
                return this.formatBehaviorKind; 
            }
        }

        /// <summary>The API behavior kind of this behavior.</summary>
        internal ODataBehaviorKind ApiBehaviorKind
        {
            get
            {
                return this.apiBehaviorKind;
            }
        }

        /// <summary>Determines whether operations bound to the given entity type must be container qualified.</summary>
        internal Func<IEdmEntityType, bool> OperationsBoundToEntityTypeMustBeContainerQualified
        {
            get
            {
                return this.operationsBoundToEntityTypeMustBeContainerQualified;
            }

            set
            {
                this.operationsBoundToEntityTypeMustBeContainerQualified = value;
            }
        }

        /// <summary>
        /// Create the reader behavior for the WCF Data Services client.
        /// </summary>
        /// <param name="typeResolver">Custom type resolver which takes both expected type and type name.
        /// This function is used instead of the IEdmModel.FindType is it's specified.
        /// The first parameter to the function is the expected type (the type infered from the parent property or specified by the external caller).
        /// The second parameter is the type name from the payload.
        /// The function should return the resolved type, or null if no such type was found.</param>
        /// <returns>The created reader behavior.</returns>
        internal static ODataReaderBehavior CreateWcfDataServicesClientBehavior(
            Func<IEdmType, string, IEdmType> typeResolver)
        {
            return new ODataReaderBehavior(ODataBehaviorKind.WcfDataServicesClient, ODataBehaviorKind.WcfDataServicesClient, /*allowDuplicatePropertyNames*/ true, typeResolver);
        }

        /// <summary>
        /// Create the reader behavior for the WCF Data Services server.
        /// </summary>
        /// <returns>The created reader behavior.</returns>
        internal static ODataReaderBehavior CreateWcfDataServicesServerBehavior()
        {
            return new ODataReaderBehavior(
                ODataBehaviorKind.ODataServer,
                ODataBehaviorKind.ODataServer, 
                /*allowDuplicatePropertyNames*/ true, 
                /*typeResolver*/ null);
        }

        /// <summary>
        /// Resets the format behavior of the current reader behavior to the default format behavior.
        /// </summary>
        internal void ResetFormatBehavior()
        {
            this.formatBehaviorKind = ODataBehaviorKind.Default;

            // Also reset all format knobs
            this.allowDuplicatePropertyNames = false;
            this.operationsBoundToEntityTypeMustBeContainerQualified = null;
        }
    }
}
