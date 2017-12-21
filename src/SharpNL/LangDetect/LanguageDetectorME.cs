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
using SharpNL.ML;
using SharpNL.Utility;

namespace SharpNL.LangDetect {

    /// <summary>
    /// A maximum entropy language detector.
    /// </summary>
    /// <inheritdoc />
    public sealed class LanguageDetectorME : ILanguageDetector {
        private readonly LanguageDetectorModel model;
        private readonly ILanguageDetectorContextGenerator contextGenerator;

        /// <summary>
        /// Initializes the current instance with a language detector model. Default feature generation is used.
        /// </summary>
        /// <param name="model">The language detector model.</param>
        public LanguageDetectorME(LanguageDetectorModel model) {
            this.model = model ?? throw new ArgumentNullException(nameof(model));

            contextGenerator = model.Factory.GetContextGenerator();
        }

        /// <inheritdoc />
        public Language[] PredictLanguages(string content) {
            if (string.IsNullOrEmpty(content))
                return new Language[] { };

            var eval = model.MaxentModel.Eval(contextGenerator.GetContext(content));
            var lang = new Language[eval.Length];
            for (var i = 0; i < eval.Length; i++)
                lang[i] = new Language(model.MaxentModel.GetOutcome(i), eval[i]);
            
            Array.Sort(lang, (a, b) => b.Confidence.CompareTo(a.Confidence));

            return lang;
        }

        /// <inheritdoc />
        public Language PredictLanguage(string content) {
            var result = PredictLanguages(content);
            return result.Length == 0 ? null : result[0];
        }

        /// <inheritdoc />
        public string[] GetSupportedLanguages() {
            return model.MaxentModel.GetOutcomes();
        }

        /// <summary>
        /// Trains language detection model with the given parameters.
        /// </summary>
        /// <param name="samples">The langauge samples.</param>
        /// <param name="trainingParameters">The machine learnable parameters.</param>
        /// <param name="factory">The language detector factory.</param>
        /// <returns>The trained <see cref="LanguageDetectorModel"/> object.</returns>
        public static LanguageDetectorModel Train(
            IObjectStream<LanguageSample> samples,
            TrainingParameters trainingParameters,
            LanguageDetectorFactory factory) {
            return Train(samples, trainingParameters, factory, null);
        }

        /// <summary>
        /// Trains language detection model with the given parameters.
        /// </summary>
        /// <param name="samples">The langauge samples.</param>
        /// <param name="trainingParameters">The machine learnable parameters.</param>
        /// <param name="factory">The language detector factory.</param>
        /// <param name="monitor">
        /// A evaluation monitor that can be used to listen the messages during the training or it can cancel the training operation.
        /// This argument can be a <c>null</c> value.
        /// </param>
        /// <returns>The trained <see cref="LanguageDetectorModel"/> object.</returns>
        public static LanguageDetectorModel Train(
            IObjectStream<LanguageSample> samples,
            TrainingParameters trainingParameters,
            LanguageDetectorFactory factory,
            Monitor monitor) {

            if (samples == null)
                throw new ArgumentNullException(nameof(samples));

            if (trainingParameters == null)
                throw new ArgumentNullException(nameof(trainingParameters));

            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var manifestInfoEntries = new Dictionary<string, string>();

            trainingParameters.SetIfAbsent(Parameters.DataIndexer, Parameters.DataIndexers.OnePass);

            var trainer = TrainerFactory.GetEventTrainer(trainingParameters, manifestInfoEntries, monitor);

            var model = trainer.Train(new LanguageDetectorEventStream(samples, factory.GetContextGenerator()));

            return new LanguageDetectorModel(model, manifestInfoEntries, factory);
        }
    }
}
