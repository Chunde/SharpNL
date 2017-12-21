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
using System.Runtime.Serialization.Formatters.Binary;

using NUnit.Framework;

namespace SharpNL.Tests.LangDetect {
    using SharpNL.LangDetect;

    [TestFixture]
    public class LanguageSampleTest {

        [Test]
        public void TestConstructor() {
            var lang = new Language("aLang");
            var context = "aContext";

            var sample = new LanguageSample(lang, context);

            Assert.AreEqual(lang, sample.Language);
            Assert.AreEqual(context, sample.Context);
        }

        [Test]
        public void TestLanguageSampleSerDe() {
            var lang = new Language("aLang");
            var context = "aContext";

            var languageSample = new LanguageSample(lang, context);

            LanguageSample deSerializedLanguageSample;
            using (var mem = new MemoryStream()) {

                var bf = new BinaryFormatter();

                bf.Serialize(mem, languageSample);

                mem.Seek(0, SeekOrigin.Begin);

                deSerializedLanguageSample = bf.Deserialize(mem) as LanguageSample;

            }

            Assert.NotNull(deSerializedLanguageSample);

            Assert.AreEqual(languageSample.Context, deSerializedLanguageSample.Context);
            Assert.AreEqual(languageSample.Language, deSerializedLanguageSample.Language);
            Assert.AreEqual(languageSample, deSerializedLanguageSample);
        }

        [Test]
        public void TestNullLang() {
            var context = "aContext";

            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new LanguageSample(null, context));
        }

        [Test]
        public void TestNullContext() {
            var lang = new Language("aLang");

            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new LanguageSample(lang, null));
        }

        [Test]
        public void TestToString() {
            var lang = new Language("aLang");
            var context = "aContext";

            LanguageSample sample = new LanguageSample(lang, context);

            Assert.AreEqual(lang.Lang + "\t" + context, sample.ToString());
        }

        [Test]
        public void TestHash() {

            int hashA = new LanguageSample(new Language("aLang"), "aContext").GetHashCode();
            int hashB = new LanguageSample(new Language("bLang"), "aContext").GetHashCode();
            int hashC = new LanguageSample(new Language("aLang"), "bContext").GetHashCode();

            Assert.AreNotEqual(hashA, hashB);
            Assert.AreNotEqual(hashA, hashC);
            Assert.AreNotEqual(hashB, hashC);
        }

        [Test]
        public void TestEquals() {

            var sampleA = new LanguageSample(new Language("aLang"), "aContext");
            var sampleA1 = new LanguageSample(new Language("aLang"), "aContext");
            var sampleB = new LanguageSample(new Language("bLang"), "aContext");
            var sampleC = new LanguageSample(new Language("aLang"), "bContext");

            Assert.AreEqual(sampleA, sampleA);
            Assert.AreEqual(sampleA, sampleA1);
            Assert.AreNotEqual(sampleA, sampleB);
            Assert.AreNotEqual(sampleA, sampleC);
            Assert.AreNotEqual(sampleB, sampleC);
            Assert.AreNotEqual(sampleA, "something else");
        }

    }
}
