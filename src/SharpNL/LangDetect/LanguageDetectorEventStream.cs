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
using System.Collections.Generic;
using SharpNL.ML.Model;
using SharpNL.Utility;

namespace SharpNL.LangDetect {
    /// <summary>
    /// Iterator-like class for modeling language detector events.
    /// </summary>
    /// <inheritdoc />
    public class LanguageDetectorEventStream : AbstractEventStream<LanguageSample> {

        private readonly ILanguageDetectorContextGenerator contextGenerator;

        /// <summary>
        /// Initializes the current instance via samples and feature generators.
        /// </summary>
        /// <param name="samples">The language sample stream.</param>
        /// <param name="contextGenerator">The context generator.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="contextGenerator"/> is null.</exception>
        public LanguageDetectorEventStream(IObjectStream<LanguageSample> samples, ILanguageDetectorContextGenerator contextGenerator) : base(samples) {
            this.contextGenerator = contextGenerator ?? throw new ArgumentNullException(nameof(contextGenerator));
        }

        /// <inheritdoc />
        protected override IEnumerator<Event> CreateEvents(LanguageSample sample) {
            yield return new Event(sample.Language.Lang, contextGenerator.GetContext(sample.Context));
        }
    }
}
