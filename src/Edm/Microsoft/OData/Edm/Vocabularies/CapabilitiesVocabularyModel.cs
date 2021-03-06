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

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;

namespace Microsoft.OData.Edm.Vocabularies.V1
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Representing Capabilities Vocabulary Model.
    /// </summary>
    internal class CapabilitiesVocabularyModel
    {
        /// <summary>
        /// The EDM model with capabilities vocabularies.
        /// </summary>
        public static readonly IEdmModel Instance;

        /// <summary>
        /// The change tracking term.
        /// </summary>
        public static readonly IEdmValueTerm ChangeTrackingTerm;

        /// <summary>
        /// Parse Capabilities Vocabulary Model from CapabilitiesVocabularies.xml
        /// </summary>
        static CapabilitiesVocabularyModel()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream("CapabilitiesVocabularies.xml"))
            {
                IEnumerable<EdmError> errors;
                Debug.Assert(stream != null, "CapabilitiesVocabularies.xml: stream!=null");
                CsdlReader.TryParse(new[] { XmlReader.Create(stream) }, out Instance, out errors);
            }

            ChangeTrackingTerm = Instance.FindDeclaredValueTerm(CapabilitiesVocabularyConstants.ChangeTracking);
        }
    }
}
