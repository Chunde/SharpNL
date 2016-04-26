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

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SharpNL.LanguageModel;
using SharpNL.NGram;
using SharpNL.Utility;

namespace SharpNL.Tests.LanguageModel {
    [TestFixture]
    public class NgramLanguageModelTest {
        [Test]
        public void TestBigram() {
            var model = new NGramLanguageModel(2);
            model.Add(new StringList("I", "see", "the", "fox"), 1, 2);
            model.Add(new StringList("the", "red", "house"), 1, 2);
            model.Add(new StringList("I", "saw", "something", "nice"), 1, 2);
            var probability = model.CalculateProbability(new StringList("I", "saw", "the", "red", "house"));

            Assert.That(probability, Is.InRange(0d, 1d), "a probability measure should be between 0 and 1 [was {0} ]",
                probability);


            var tokens = model.PredictNextTokens(new StringList("I", "saw"));

            Assert.That(tokens, Is.EqualTo(new StringList("something")));
        }

        [Test]
        public void TestBigramProbabilityNoSmoothing() {
            var model = new NGramLanguageModel(2, 0);
            model.Add(new StringList("<s>", "I", "am", "Sam", "</s>"), 1, 2);
            model.Add(new StringList("<s>", "Sam", "I", "am", "</s>"), 1, 2);
            model.Add(new StringList("<s>", "I", "do", "not", "like", "green", "eggs", "and", "ham", "</s>"), 1, 2);

            var probability = model.CalculateProbability(new StringList("<s>", "I"));
            Assert.That(probability, Is.EqualTo(0.666d).Within(0.001));

            probability = model.CalculateProbability(new StringList("Sam", "</s>"));
            Assert.That(probability, Is.EqualTo(0.5d).Within(0.001));

            probability = model.CalculateProbability(new StringList("<s>", "Sam"));
            Assert.That(probability, Is.EqualTo(0.333d).Within(0.001));

            probability = model.CalculateProbability(new StringList("am", "Sam"));
            Assert.That(probability, Is.EqualTo(0.5d).Within(0.001));

            probability = model.CalculateProbability(new StringList("I", "am"));
            Assert.That(probability, Is.EqualTo(0.666d).Within(0.001));

            probability = model.CalculateProbability(new StringList("I", "do"));
            Assert.That(probability, Is.EqualTo(0.333d).Within(0.001));

            probability = model.CalculateProbability(new StringList("I", "am", "Sam"));
            Assert.That(probability, Is.EqualTo(0.333d).Within(0.001));
        }

        [Test]
        public void TestEmptyVocabularyProbability() {
            var model = new NGramLanguageModel();

            Assert.That(model.CalculateProbability(new StringList(string.Empty)), Is.EqualTo(0d),
                "Probability with an empty vocabulary is always 0");

            Assert.That(model.CalculateProbability(new StringList("1", "2", "3")), Is.EqualTo(0d),
                "Probability with an empty vocabulary is always 0");
        }

        [Test]
        public void TestNgramModel() {
            var model = new NGramLanguageModel(4) {
                {new StringList("I", "saw", "the", "fox"), 1, 4},
                {new StringList("the", "red", "house"), 1, 4},
                {new StringList("I", "saw", "something", "nice"), 1, 2}
            };
            var probability = model.CalculateProbability(new StringList("I", "saw", "the", "red", "house"));

            Assert.That(probability, Is.InRange(0d, 1d), "a probability measure should be between 0 and 1 [was {0} ]",
                probability);

            var tokens = model.PredictNextTokens(new StringList("I", "saw"));

            Assert.That(tokens, Is.EqualTo(new StringList("the", "fox")));
        }

        [Test]
        public void TestRandomVocabularyAndSentence() {
            var model = new NGramLanguageModel();
            foreach (var sentence in LanguageModelTestUtils.GenerateRandomVocabulary(10)) {
                model.Add(sentence, 2, 3);
            }
            var probability = model.CalculateProbability(LanguageModelTestUtils.GenerateRandomSentence());
            Assert.That(probability, Is.InRange(0d, 1d), "a probability measure should be between 0 and 1 [was {0} ]",
                probability);
        }

        [Test]
        public void TestSerializedNGramLanguageModel() {
            var languageModel = new NGramLanguageModel(Tests.OpenFile("/opennlp/tools/ngram/ngram-model.xml"), 3);

            var probability = languageModel.CalculateProbability(new StringList("The", "brown", "fox", "jumped"));
            Assert.That(probability, Is.InRange(0d, 1d), "a probability measure should be between 0 and 1 [was {0} ]",
                probability);

            var tokens = languageModel.PredictNextTokens(new StringList("fox"));

            Assert.That(tokens, Is.EqualTo(new StringList("jumped")));
        }

        [Test]
        public void TestTrigram() {
            var model = new NGramLanguageModel(3) {
                {new StringList("I", "see", "the", "fox"), 2, 3},
                {new StringList("the", "red", "house"), 2, 3},
                {new StringList("I", "saw", "something", "nice"), 2, 3}
            };
            var probability = model.CalculateProbability(new StringList("I", "saw", "the", "red", "house"));


            Assert.That(probability, Is.InRange(0d, 1d), "a probability measure should be between 0 and 1 [was {0} ]",
                probability);

            var tokens = model.PredictNextTokens(new StringList("I", "saw"));

            Assert.That(tokens, Is.EqualTo(new StringList("something", "nice")));
        }

        [Test]
        public void TestTrigramLanguageModelCreationFromText() {
            var ngramSize = 3;
            var languageModel = new NGramLanguageModel(ngramSize);

            var stream = Tests.OpenFile("/opennlp/tools/languagemodel/sentences.txt", Encoding.UTF8);

            string line;
            while ((line = stream.ReadLine()) != null) {
                var list = new List<string>(line.Split(new[] {' '}, StringSplitOptions.None));
                var generatedStrings = NGramGenerator.Generate(list, ngramSize, " ");
                foreach (var generatedString in generatedStrings) {
                    var tokens = generatedString.Split(new[] {' '}, StringSplitOptions.None);
                    if (tokens.Length > 0)
                        languageModel.Add(new StringList(tokens), 1, ngramSize);
                }
            }


            var predited = languageModel.PredictNextTokens(new StringList("neural", "network", "language"));

            Assert.That(predited, Is.EqualTo(new StringList("models")));

            var p1 = languageModel.CalculateProbability(new StringList("neural", "network", "language", "models"));
            var p2 = languageModel.CalculateProbability(new StringList("neural", "network", "language", "model"));

            Assert.That(p1, Is.GreaterThan(p2));
        }
    }
}