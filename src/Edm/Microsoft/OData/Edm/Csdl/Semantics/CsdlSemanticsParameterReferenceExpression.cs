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
    internal class CsdlSemanticsParameterReferenceExpression : CsdlSemanticsExpression, IEdmParameterReferenceExpression, IEdmCheckable
    {
        private readonly CsdlParameterReferenceExpression expression;
        private readonly IEdmEntityType bindingContext;

        private readonly Cache<CsdlSemanticsParameterReferenceExpression, IEdmOperationParameter> referencedCache = new Cache<CsdlSemanticsParameterReferenceExpression, IEdmOperationParameter>();
        private static readonly Func<CsdlSemanticsParameterReferenceExpression, IEdmOperationParameter> ComputeReferencedFunc = (me) => me.ComputeReferenced();

        public CsdlSemanticsParameterReferenceExpression(CsdlParameterReferenceExpression expression, IEdmEntityType bindingContext, CsdlSemanticsSchema schema)
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
            get { return EdmExpressionKind.ParameterReference; }
        }

        public IEdmOperationParameter ReferencedParameter
        {
            get { return this.referencedCache.GetValue(this, ComputeReferencedFunc, null); }
        }

        public IEnumerable<EdmError> Errors
        {
            get
            {
                if (this.ReferencedParameter is IUnresolvedElement)
                {
                    return this.ReferencedParameter.Errors();
                }

                return Enumerable.Empty<EdmError>();
            }
        }

        private IEdmOperationParameter ComputeReferenced()
        {
            return new UnresolvedParameter(new UnresolvedOperation(String.Empty, Edm.Strings.Bad_UnresolvedOperation(String.Empty), this.Location), this.expression.Parameter, this.Location);
        }
    }
}
