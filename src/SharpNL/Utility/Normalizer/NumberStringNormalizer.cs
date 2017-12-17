// 
//  Copyright 2017 Gustavo J Knuppe (https://github.com/knuppe)
// 
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
// 
//       http://www.apache.org/licenses/LICENSE-2.0
// 
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// 
//   - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
//   - May you do good and not evil.                                         -
//   - May you find forgiveness for yourself and forgive others.             -
//   - May you share freely, never taking more than you give.                -
//   - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
//  

using System.Text.RegularExpressions;

namespace SharpNL.Utility.Normalizer {
    /// <summary>
    /// Normalizer for numbers.
    /// </summary>
    /// <inheritdoc />
    public class NumberStringNormalizer : IStringNormalizer {

        private static NumberStringNormalizer instance;
        private static readonly Regex NumberRegex = new Regex("\\d+", RegexOptions.Compiled);

        /// <summary>
        /// Gets the static instance of <see cref="NumberStringNormalizer"/>.
        /// </summary>
        public static NumberStringNormalizer Instange => instance ?? (instance = new NumberStringNormalizer());

        /// <inheritdoc />
        public string Normalize(string text) {
            return string.IsNullOrEmpty(text) ? text : NumberRegex.Replace(text, " ");
        }
    }
}
