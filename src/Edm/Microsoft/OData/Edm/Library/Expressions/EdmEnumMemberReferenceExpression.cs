//   OData .NET Libraries
//   Copyright (c) Microsoft Corporation
//   All rights reserved. 

//   Licensed under the Apache License, Version 2.0 (the ""License""); you may not use this file except in compliance with the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 

//   THIS CODE IS PROVIDED ON AN  *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABLITY OR NON-INFRINGEMENT. 

//   See the Apache Version 2.0 License for specific language governing permissions and limitations under the License.

using Microsoft.OData.Edm.Expressions;

namespace Microsoft.OData.Edm.Library.Expressions
{
    /// <summary>
    /// Represents an EDM enumeration member reference expression.
    /// </summary>
    public class EdmEnumMemberReferenceExpression : EdmElement, IEdmEnumMemberReferenceExpression
    {
        private readonly IEdmEnumMember referencedEnumMember;

        /// <summary>
        /// Initializes a new instance of the <see cref="EdmEnumMemberReferenceExpression"/> class.
        /// </summary>
        /// <param name="referencedEnumMember">Referenced enum member.</param>
        public EdmEnumMemberReferenceExpression(IEdmEnumMember referencedEnumMember)
        {
            EdmUtil.CheckArgumentNull(referencedEnumMember, "referencedEnumMember");
            this.referencedEnumMember = referencedEnumMember;
        }

        /// <summary>
        /// Gets the referenced enum member.
        /// </summary>
        public IEdmEnumMember ReferencedEnumMember
        {
            get { return this.referencedEnumMember; }
        }

        /// <summary>
        /// Gets the kind of this expression.
        /// </summary>
        public EdmExpressionKind ExpressionKind
        {
            get { return EdmExpressionKind.EnumMemberReference; }
        }
    }
}