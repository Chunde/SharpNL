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
using SharpNL.Utility;
using SharpNL.Utility.Evaluation;

namespace SharpNL.LangDetect {
    /// <summary>
    /// Cross validator for language detector.
    /// </summary>
    public sealed class LanguageDetectorCrossValidator {
        private readonly TrainingParameters trainingParameters;
        private readonly LanguageDetectorFactory factory;
        private readonly IEvaluationMonitor<LanguageSample>[] listeners;

        private readonly Mean documentAccuracy = new Mean();

        /// <summary>
        /// Initializes the cross validator.
        /// </summary>
        /// <param name="trainingParameters">The machine learning parameters.</param>
        /// <param name="factory">The language detector factory.</param>
        /// <param name="listeners">The evaluation listeners.</param>
        /// <exception cref="ArgumentNullException"><paramref name="trainingParameters"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
        public LanguageDetectorCrossValidator(
            TrainingParameters trainingParameters,
            LanguageDetectorFactory factory,
            params IEvaluationMonitor<LanguageSample>[] listeners) {

            this.trainingParameters = trainingParameters ?? throw new ArgumentNullException(nameof(trainingParameters));
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.listeners = listeners;
        }

        /// <summary>
        /// Starts the evaluation.
        /// </summary>
        /// <param name="samples">The data to train and test.</param>
        /// <param name="nFolds">The number of folds.</param>
        public void Evaluate(IObjectStream<LanguageSample> samples, int nFolds) {
            
            var partitioner = new CrossValidationPartitioner<LanguageSample>(samples, nFolds);

            while (partitioner.HasNext) {

                var sampleStream = partitioner.Next();

                var model = LanguageDetectorME.Train(sampleStream, trainingParameters, factory);

                var evaluator = new LanguageDetectorEvaluator(new LanguageDetectorME(model), listeners);
                
                evaluator.Evaluate(sampleStream.GetTestSampleStream());

                documentAccuracy.Add(evaluator.Accuracy, evaluator.DocumentCount);
            }

        }

        /// <summary>
        /// Gets the accuracy for all iterations.
        /// </summary>
        public double DocumentAccuracy => documentAccuracy.Value;

        /// <summary>
        /// Gets the number of words which where validated over all iterations.
        /// </summary>
        /// <returns>The amount of folds multiplied by the total number of words.</returns>
        public long DocumentCount => documentAccuracy.Count;

    }
}
