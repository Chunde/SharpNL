//  
//  Copyright 2016 Gustavo J Knuppe (https://github.com/knuppe)
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

using NUnit.Framework;
using SharpNL.LanguageModel;

namespace SharpNL.Tests.LanguageModel {
    /// <summary>
    ///     Tests for evaluating accuracy of language models
    /// </summary>
    [TestFixture]
    public class LanguageModelEvaluationTest {
        [Test]
        public void TestPerplexityComparison() {
            var trainingVocabulary = LanguageModelTestUtils.GenerateRandomVocabulary(11000);

            //var trainingVocabulary = LanguageModelTestUtils.GenerateRandomVocabulary(1100000);
            var testVocabulary = LanguageModelTestUtils.GenerateRandomVocabulary(100);

            var unigramLM = new NGramLanguageModel(1);
            foreach (var sentence in trainingVocabulary) {
                unigramLM.Add(sentence, 1, 1);
            }
            var unigramPerplexity = LanguageModelTestUtils.GetPerplexity(unigramLM, testVocabulary, 1);

            var bigramLM = new NGramLanguageModel(2);
            foreach (var sentence in trainingVocabulary) {
                bigramLM.Add(sentence, 1, 2);
            }
            var bigramPerplexity = LanguageModelTestUtils.GetPerplexity(bigramLM, testVocabulary, 2);
            Assert.That(unigramPerplexity, Is.GreaterThanOrEqualTo(bigramPerplexity));

            var trigramLM = new NGramLanguageModel(3);
            foreach (var sentence in trainingVocabulary) {
                trigramLM.Add(sentence, 2, 3);
            }
            var trigramPerplexity = LanguageModelTestUtils.GetPerplexity(trigramLM, testVocabulary, 3);


            Assert.That(bigramPerplexity, Is.GreaterThanOrEqualTo(trigramPerplexity));
        }
    }
}