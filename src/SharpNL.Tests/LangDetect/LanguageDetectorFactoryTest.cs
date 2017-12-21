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

using System.IO;
using NUnit.Framework;

namespace SharpNL.Tests.LangDetect {
    using SharpNL.LangDetect;

    [TestFixture]
    public class LanguageDetectorFactoryTest {

        private LanguageDetectorModel model;

        [OneTimeSetUp]
        public void Init() {
            model = LanguageDetectorMETest.TrainModel(new DummyLangDetectorFactory());
        }

        [Test]
        public void TestCorrectFactory() {

            using (var mem = new MemoryStream()) {
                model.Serialize(mem, true);

                mem.Seek(0, SeekOrigin.Begin);

                var deserializedModel = new LanguageDetectorModel(mem);

                Assert.NotNull(deserializedModel);

                Assert.IsInstanceOf<DummyLangDetectorFactory>(deserializedModel.Factory);
            }

        }



        [Test]
        public void TestDummyFactoryContextGenerator() {

            var cg = model.Factory.GetContextGenerator();

            var context = cg.GetContext("a dummy text phrase to test if the context generator works!!!!!!!!!!!!");

            CollectionAssert.Contains(context, "!!!!!"); // default normalizer would remove the repeated !
            CollectionAssert.Contains(context, "a dum");
            CollectionAssert.Contains(context, "tg=[THE,CONTEXT,GENERATOR]");
        }
    }
}
