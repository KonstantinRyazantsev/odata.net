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

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OData.Edm.Csdl.Parsing.Ast;
using Microsoft.OData.Edm.Expressions;
using Microsoft.OData.Edm.Validation;

namespace Microsoft.OData.Edm.Csdl.CsdlSemantics
{
    internal class CsdlSemanticsEnumMemberExpression : CsdlSemanticsExpression, IEdmEnumMemberExpression, IEdmCheckable
    {
        private readonly CsdlEnumMemberExpression expression;
        private readonly IEdmEntityType bindingContext;

        private readonly Cache<CsdlSemanticsEnumMemberExpression, IEnumerable<IEdmEnumMember>> referencedCache = new Cache<CsdlSemanticsEnumMemberExpression, IEnumerable<IEdmEnumMember>>();
        private static readonly Func<CsdlSemanticsEnumMemberExpression, IEnumerable<IEdmEnumMember>> ComputeReferencedFunc = (me) => me.ComputeReferenced();

        private readonly Cache<CsdlSemanticsEnumMemberExpression, IEnumerable<EdmError>> errorsCache = new Cache<CsdlSemanticsEnumMemberExpression, IEnumerable<EdmError>>();
        private static readonly Func<CsdlSemanticsEnumMemberExpression, IEnumerable<EdmError>> ComputeErrorsFunc = (me) => me.ComputeErrors();

        public CsdlSemanticsEnumMemberExpression(CsdlEnumMemberExpression expression, IEdmEntityType bindingContext, CsdlSemanticsSchema schema)
            : base(schema, expression)
        {
            this.expression = expression;
            this.bindingContext = bindingContext;
        }

        public override CsdlElement Element
        {
            get { return this.expression; }
        }

        public override EdmExpressionKind ExpressionKind
        {
            get { return EdmExpressionKind.EnumMember; }
        }

        public IEnumerable<IEdmEnumMember> EnumMembers
        {
            get { return this.referencedCache.GetValue(this, ComputeReferencedFunc, null); }
        }

        public IEnumerable<EdmError> Errors
        {
            get { return this.errorsCache.GetValue(this, ComputeErrorsFunc, null); }
        }

        private IEnumerable<IEdmEnumMember> ComputeReferenced()
        {
            IEnumerable<IEdmEnumMember> member;
            return EdmValueParser.TryParseEnumMember(this.expression.EnumMemberPath, this.Schema.Types, out member) ? member : null;
        }

        private IEnumerable<EdmError> ComputeErrors()
        {
            IEnumerable<IEdmEnumMember> member;
            if (!EdmValueParser.TryParseEnumMember(this.expression.EnumMemberPath, this.Schema.Types, out member))
            {
                return new EdmError[] { new EdmError(this.Location, EdmErrorCode.InvalidEnumMemberPath, Edm.Strings.CsdlParser_InvalidEnumMemberPath(this.expression.EnumMemberPath)) };
            }

            return Enumerable.Empty<EdmError>();
        }
    }
}
