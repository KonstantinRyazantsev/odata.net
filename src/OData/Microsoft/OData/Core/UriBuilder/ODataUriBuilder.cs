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

namespace Microsoft.OData.Core.UriBuilder
{
    using System;
    using System.Collections.Generic;
    using Microsoft.OData.Core;
    using Microsoft.OData.Core.UriParser;
    using Microsoft.OData.Core.UriParser.Semantic;

    /// <summary>
    /// Builder to build a given semantic tree in ODataUri to url in the format of string
    /// </summary>
    public sealed class ODataUriBuilder
    {
        /// <summary>
        /// semantic tree of uri which will be build to relative url
        /// </summary>
        private readonly ODataUri odataUri;

        /// <summary>
        /// Mark whether the key is AsSegment
        /// </summary>
        private readonly ODataUrlConventions urlConventions;

        /// <summary>
        /// Constructor of the ODataUriBuilder
        /// </summary>
        /// <param name="urlConventions">ODataUriBuilder constructor</param>
        /// <param name="odataUri">semantic tree of the uri</param>
        public ODataUriBuilder(ODataUrlConventions urlConventions, ODataUri odataUri)
        {
            this.urlConventions = urlConventions;
            this.odataUri = odataUri;
        }

        /// <summary>
        /// Build ODataUri into a Uri, the result Uri's query options are URL encoded.
        /// </summary>
        /// <returns>uri of the semantic tree</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("DataWeb.Usage", "AC0018:SystemUriEscapeDataStringRule", Justification = "Values passed to this method are model elements like property names or keywords.")]
        public Uri BuildUri()
        {
            NodeToStringBuilder nodeToStringBuilder = new NodeToStringBuilder();
            SelectExpandClauseToStringBuilder selectExpandClauseToStringBuilder = new SelectExpandClauseToStringBuilder();

            String queryOptions = String.Empty;
            bool writeQueryPrefix = true;
            if (this.odataUri.Filter != null)
            {
                queryOptions = WriteQueryPrefixOrSeparator(writeQueryPrefix, queryOptions);
                writeQueryPrefix = false;
                queryOptions = string.Concat(queryOptions, "$filter", ExpressionConstants.SymbolEqual, Uri.EscapeDataString(nodeToStringBuilder.TranslateFilterClause(this.odataUri.Filter)));
            }

            if (this.odataUri.SelectAndExpand != null)
            {
                string result = selectExpandClauseToStringBuilder.TranslateSelectExpandClause(this.odataUri.SelectAndExpand, true);
                if (!string.IsNullOrEmpty(result))
                {
                    queryOptions = WriteQueryPrefixOrSeparator(writeQueryPrefix, queryOptions);
                    writeQueryPrefix = false;
                    queryOptions = string.Concat(queryOptions, result);
                }
            }

            if (this.odataUri.OrderBy != null)
            {
                queryOptions = WriteQueryPrefixOrSeparator(writeQueryPrefix, queryOptions);
                writeQueryPrefix = false;
                queryOptions = string.Concat(queryOptions, "$orderby", ExpressionConstants.SymbolEqual, Uri.EscapeDataString(nodeToStringBuilder.TranslateOrderByClause(this.odataUri.OrderBy)));
            }

            if (this.odataUri.Top != null)
            {
                queryOptions = WriteQueryPrefixOrSeparator(writeQueryPrefix, queryOptions);
                writeQueryPrefix = false;
                queryOptions = string.Concat(queryOptions, "$top", ExpressionConstants.SymbolEqual, Uri.EscapeDataString(this.odataUri.Top.ToString()));
            }

            if (this.odataUri.Skip != null)
            {
                queryOptions = WriteQueryPrefixOrSeparator(writeQueryPrefix, queryOptions);
                writeQueryPrefix = false;
                queryOptions = string.Concat(queryOptions, "$skip", ExpressionConstants.SymbolEqual, Uri.EscapeDataString(this.odataUri.Skip.ToString()));
            }

            if (this.odataUri.QueryCount != null)
            {
                queryOptions = WriteQueryPrefixOrSeparator(writeQueryPrefix, queryOptions);
                writeQueryPrefix = false;
                queryOptions = string.Concat(queryOptions, "$count", ExpressionConstants.SymbolEqual, Uri.EscapeDataString(this.odataUri.QueryCount == true ? ExpressionConstants.KeywordTrue : ExpressionConstants.KeywordFalse));
            }

            if (this.odataUri.Search != null)
            {
                queryOptions = WriteQueryPrefixOrSeparator(writeQueryPrefix, queryOptions);
                writeQueryPrefix = false;
                queryOptions = string.Concat(queryOptions, "$search", ExpressionConstants.SymbolEqual, Uri.EscapeDataString(nodeToStringBuilder.TranslateSearchClause(this.odataUri.Search)));
            }

            if (this.odataUri.ParameterAliasNodes != null && this.odataUri.ParameterAliasNodes.Count > 0)
            {
                string aliasNode = nodeToStringBuilder.TranslateParameterAliasNodes(this.odataUri.ParameterAliasNodes);
                queryOptions = String.IsNullOrEmpty(aliasNode) ? queryOptions : String.Concat(WriteQueryPrefixOrSeparator(writeQueryPrefix, queryOptions), aliasNode);
                writeQueryPrefix = false;
            }

            string res = String.Concat(this.odataUri.Path.ToResourcePathString(urlConventions), queryOptions);
            return this.odataUri.ServiceRoot == null ? new Uri(res, UriKind.Relative) : new Uri(this.odataUri.ServiceRoot, new Uri(res, UriKind.Relative));
        }

        /// <summary>
        /// Write ? or &amp; depending on whether it is the start of the whole query or query part.
        /// </summary>
        /// <param name="writeQueryPrefix">True if start of whole query, false if not. </param>
        /// <param name="queryOptions">add a queryPrefix to the queryOptions.</param>
        /// <returns>return  the queryOptions with a queryPrefix</returns>
        private static String WriteQueryPrefixOrSeparator(bool writeQueryPrefix, String queryOptions)
        {
            if (writeQueryPrefix)
            {
                return String.Concat(queryOptions, ExpressionConstants.SymbolQueryStart);
            }
            else
            {
                return String.Concat(queryOptions, ExpressionConstants.SymbolQueryConcatenate);
            }
        }
    }
}
