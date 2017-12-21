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
using System.IO;
using System.Text;
using NUnit.Framework;

namespace SharpNL.Tests.LangDetect {
    using SharpNL.LangDetect;
    using SharpNL.Utility;

    [TestFixture]
    public class LanguageDetectorMETest {

        private LanguageDetectorModel model;

        [OneTimeSetUp]
        public void Init() {
            model = TrainModel();
        }

        [Test]
        public void TestPredictLanguages() {
            var ld = new LanguageDetectorME(model);

            var languages = ld.PredictLanguages("estava em uma marcenaria na Rua Bruno");

            Assert.AreEqual(4, languages.Length);

            Assert.AreEqual("pob", languages[0].Lang);
            Assert.AreEqual("ita", languages[1].Lang);
            Assert.AreEqual("spa", languages[2].Lang);
            Assert.AreEqual("fra", languages[3].Lang);

        }

        [Test]
        public void TestPredictLanguage() {
            
            var ld = new LanguageDetectorME(model);
            var language = ld.PredictLanguage("Dove è meglio che giochi");

            Assert.AreEqual("ita", language.Lang);

        }

        [Test]
        public void TestSupportedLanguages() {

            var ld = new LanguageDetectorME(model);
            var supportedLanguages = ld.GetSupportedLanguages();

            Assert.AreEqual(4, supportedLanguages.Length);

            CollectionAssert.Contains(supportedLanguages, "pob");
            CollectionAssert.Contains(supportedLanguages, "ita");
            CollectionAssert.Contains(supportedLanguages, "spa");
            CollectionAssert.Contains(supportedLanguages, "fra");

        }

        [Test]
        public void TestSerialization() {
            using (var mem = new MemoryStream()) {
                model.Serialize(mem, true);

                mem.Seek(0, SeekOrigin.Begin);

                var deserializedModel = new LanguageDetectorModel(mem);

                Assert.NotNull(deserializedModel);
            }
        }

        public static LanguageDetectorModel TrainModel() {
            return TrainModel(new LanguageDetectorFactory());
        }
        public static LanguageDetectorModel TrainModel(LanguageDetectorFactory factory) {
            var sampleStream = CreateSampleStream();

            var parameters = new TrainingParameters();
            parameters.Set(Parameters.Iterations, "100");
            parameters.Set(Parameters.Cutoff, "5");
            parameters.Set(Parameters.DataIndexer, Parameters.DataIndexers.TwoPass);
            parameters.Set(Parameters.Algorithm, Parameters.Algorithms.NaiveBayes);

            return LanguageDetectorME.Train(sampleStream, parameters, factory);
        }

        public static LanguageDetectorSampleStream CreateSampleStream() {
            return new LanguageDetectorSampleStream(
                new PlainTextByLineStream(Tests.OpenFile("/opennlp/tools/doccat/DoccatSample.txt"), Encoding.UTF8)
            );            
        }

    }
}
