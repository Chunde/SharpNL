﻿// 
//  Copyright 2014 Gustavo J Knuppe (https://github.com/knuppe)
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
using SharpNL.Stemmer.Porter;

namespace SharpNL.Tests.Stemmer {
    [TestFixture]
    internal class PorterStemmerTest {

        [Test]
        public void TestStemming() {

            var stemmer = new PorterStemmer();

            Assert.AreEqual("deni", stemmer.Stem("deny"));
            Assert.AreEqual("declin", stemmer.Stem("declining"));
            Assert.AreEqual("divers", stemmer.Stem("diversity"));
            Assert.AreEqual("diver", stemmer.Stem("divers"));
            Assert.AreEqual("dental", stemmer.Stem("dental"));

        }       
    }
}