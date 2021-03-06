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

namespace Microsoft.OData.Core.UriParser.Semantic
{
    /// <summary>
    /// The result of parsing $levels option
    /// </summary>
    public sealed class LevelsClause
    {
        /// <summary>
        /// Whether targeting at level max.
        /// </summary>
        private bool isMaxLevel;

        /// <summary>
        /// The level value.
        /// </summary>
        private long level;

        /// <summary>
        /// Constructs a <see cref="LevelsClause"/> from given parameters.
        /// </summary>
        /// <param name="isMaxLevel">Flag indicating max level is specified.</param>
        /// <param name="level">The level value for the LevelsClause.</param>
        public LevelsClause(bool isMaxLevel, long level)
        {
            this.isMaxLevel = isMaxLevel;
            this.level = level;
        }

        /// <summary>
        /// Get a flag indicating whether max level is specified.
        /// </summary>
        public bool IsMaxLevel
        {
            get { return this.isMaxLevel; }
        }

        /// <summary>
        /// The level value for current expand option.
        /// </summary>
        /// <remarks>This value is trivial when IsMaxLevel is True.</remarks>
        public long Level
        {
            get { return level; }
        }
    }
}
