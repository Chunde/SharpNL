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

using System.Linq;

namespace SharpNL.LangDetect {

    using NGram;
    using Utility.Normalizer;

    /// <summary>
    /// Class for holding the document language and its confidence.
    /// </summary>
    /// <inheritdoc />
    public class DefaultLanguageDetectorContextGenerator : ILanguageDetectorContextGenerator {

        /// <summary>
        /// Gets the minimum ngrams chars.
        /// </summary>
        protected readonly int MinLength;

        /// <summary>
        /// Gets the maximum ngrams chars.
        /// </summary>
        protected readonly int MaxLength;

        /// <summary>
        /// Gets the aggregated string normalizer.
        /// </summary>
        protected readonly AggregateStringNormalizer Normalizer;

        /// <summary>
        /// Creates a customizable <see cref="DefaultLanguageDetectorContextGenerator"/> that computes ngrams from text.
        /// </summary>
        /// <param name="minLength">The min ngrams chars.</param>
        /// <param name="maxLength">The max ngrams chars.</param>
        /// <param name="normalizers">The normalizers.</param>
        public DefaultLanguageDetectorContextGenerator(int minLength, int maxLength, params IStringNormalizer[] normalizers) {
            MinLength = minLength;
            MaxLength = maxLength;
            Normalizer = new AggregateStringNormalizer(normalizers);
        }

        /// <summary>
        /// Generates the context for a document using character ngrams.
        /// </summary>
        /// <param name="document">The document to extract data from.</param>
        /// <returns>An array with the generated context.</returns>
        /// <inheritdoc />
        public virtual string[] GetContext(string document) {
            var model = new NGramModel {
                {Normalizer.Normalize(document), MinLength, MaxLength}
            };
            return (from tokenList in model where tokenList.Count > 0 select tokenList[0]).ToArray();
        }
    }
}
