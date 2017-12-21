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
using NUnit.Framework;

namespace SharpNL.Tests.LangDetect {
    using SharpNL.LangDetect;

    [TestFixture]
    public class LanguageTest {

        [Test]
        public void EmptyConfidence() {

            var languageCode = "aLanguage";
            var lang = new Language(languageCode);

            Assert.AreEqual(languageCode, lang.Lang);
            Assert.AreEqual(0, lang.Confidence, double.Epsilon);
        }

        [Test]
        public void NonEmptyConfidence() {

            var languageCode = "aLanguage";
            var confidence = 0.05d;
            var lang = new Language(languageCode, confidence);

            Assert.AreEqual(languageCode, lang.Lang);
            Assert.AreEqual(confidence, lang.Confidence, double.Epsilon);
        }

        [Test]
        public void EmptyLanguage() {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new Language(null));
        }

        [Test]
        public void EmptyLanguageConfidence() {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new Language(null, 0.05));
        }

        [Test]
        public void ToStringTest() {
            
            var lang = new Language("aLang");

            Assert.AreEqual("aLang (0)", lang.ToString());

            lang = new Language("aLang", 0.0886678);

            Assert.AreEqual("aLang (0.0886678)", lang.ToString());
        }

        [Test]
        public void TestGetHashCode() {

            var hashA = new Language("aLang").GetHashCode();
            var hashAA = new Language("aLang").GetHashCode();
            var hashB = new Language("BLang").GetHashCode();
            var hashA5 = new Language("aLang", 5.0).GetHashCode();
            var hashA6 = new Language("BLang", 6.0).GetHashCode();

            Assert.AreEqual(hashA, hashAA);
            Assert.AreNotEqual(hashA, hashB);
            Assert.AreNotEqual(hashA, hashA5);
            Assert.AreNotEqual(hashB, hashA5);
            Assert.AreNotEqual(hashA5, hashA6);
        }


        [Test]
        public void TestEquals() {

            var langA = new Language("langA");
            var langB = new Language("langB");
            var langA5 = new Language("langA5", 5.0);
            var langA6 = new Language("langA5", 6.0);

            Assert.AreEqual(langA, langA);
            Assert.AreEqual(langA5, langA5);

            Assert.AreNotEqual(langA, langA5);
            Assert.AreNotEqual(langA, langB);

            Assert.AreEqual(langA6, langA5);

            Assert.AreNotEqual(langA, "something else");
        }
    }
}
