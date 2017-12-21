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

namespace SharpNL.LangDetect {
    using Utility.Evaluation;


    /// <summary>
    /// The langauge detector evaluator measures the performance of the given <see cref="ILanguageDetector"/>
    /// with the provided reference <see cref="LanguageSample"/>s.
    /// </summary>
    /// <seealso cref="ILanguageDetector"/>
    /// <seealso cref="LanguageSample"/>
    public class LanguageDetectorEvaluator : Evaluator<LanguageSample, string> {

        private readonly ILanguageDetector languageDetector;
        private readonly Mean accuracy = new Mean();

        /// <summary>
        /// Initializes the current instance.
        /// </summary>
        /// <param name="languageDetector">The language detector</param>
        /// <param name="listeners">The evaluation listeners.</param>
        public LanguageDetectorEvaluator(ILanguageDetector languageDetector,
            params IEvaluationMonitor<LanguageSample>[] listeners)
            : base(listeners) {

            this.languageDetector = languageDetector ?? throw new ArgumentNullException(nameof(languageDetector));
        }

        /// <inheritdoc />
        protected override LanguageSample ProcessSample(LanguageSample reference) {
            Language predicted = languageDetector.PredictLanguage(reference.Context);

            FMeasure.UpdateScores(new [] { reference.Language.Lang }, new [] { predicted.Lang });

            accuracy.Add(reference.Language.Lang == predicted.Lang ? 1 : 0);

            return new LanguageSample(predicted, reference.Context);
        }

        /// <summary>
        /// Gets the accuracy of provided <see cref="ILanguageDetector"/>.
        /// <para>accuracy = correctly categorized documents / total documents</para>
        /// </summary>
        public double Accuracy => accuracy.Value;

        /// <summary>
        /// Gets the document count.
        /// </summary>
        public long DocumentCount => accuracy.Count;
    }
}
