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

using System;
using SharpNL.Analyzer;

namespace SharpNL.LangDetect {
    /// <summary>
    /// Represents a language detector analyzer which allows the easy abstraction of the language detectors. This class is thread-safe.
    /// </summary>
    public class LanguageDetectorAnalyzer : AbstractAnalyzer {
        private readonly ILanguageDetector languageDetector;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageDetectorAnalyzer"/> using the default analyzer weight;
        /// </summary>
        /// <param name="languageDetector">The language detector used by this analyzer.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="languageDetector"/> is null.</exception>
        public LanguageDetectorAnalyzer(ILanguageDetector languageDetector) : this(languageDetector, 0f) {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageDetectorAnalyzer" /> using the specified language detector and the analyzer weight.
        /// </summary>
        /// <param name="languageDetector">The language detector used by this analyzer.</param>
        /// <param name="weight">The analyzer weight.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="languageDetector"/> is null.</exception>
        public LanguageDetectorAnalyzer(ILanguageDetector languageDetector, float weight) : base(weight) {
            this.languageDetector = languageDetector ?? throw new ArgumentNullException(nameof(languageDetector));
        }

        /// <inheritdoc />
        protected override void Evaluate(ITextFactory factory, IDocument document) {

            var result = languageDetector.PredictLanguage(document.Text);
            if (result != null)
                factory.SetLanguage(document, result.Lang);

        }
    }
}
