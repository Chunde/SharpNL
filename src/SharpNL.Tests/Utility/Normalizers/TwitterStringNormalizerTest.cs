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

using NUnit.Framework;
using SharpNL.Utility.Normalizer;

namespace SharpNL.Tests.Utility.Normalizers {
    [TestFixture]
    public class TwitterStringNormalizerTest {

        [Test]
        public void normalizeHashtag() {
            Assert.AreEqual("asdf   2nnfdf", TwitterStringNormalizer.Instange.Normalize("asdf #hasdk23 2nnfdf"));
        }

        [Test]
        public void normalizeUser() {
            Assert.AreEqual("asdf   2nnfdf", TwitterStringNormalizer.Instange.Normalize("asdf @hasdk23 2nnfdf"));
        }

        [Test]
        public void normalizeRT() {
            Assert.AreEqual(" 2nnfdf", TwitterStringNormalizer.Instange.Normalize("RT RT RT 2nnfdf"));
        }

        [Test]
        public void normalizeLaugh() {
            Assert.AreEqual("ahahah", TwitterStringNormalizer.Instange.Normalize("ahahahah"));
            Assert.AreEqual("haha", TwitterStringNormalizer.Instange.Normalize("hahha"));
            Assert.AreEqual("haha", TwitterStringNormalizer.Instange.Normalize("hahaa"));
            Assert.AreEqual("ahaha", TwitterStringNormalizer.Instange.Normalize("ahahahahhahahhahahaaaa"));
            Assert.AreEqual("jaja", TwitterStringNormalizer.Instange.Normalize("jajjajajaja"));
        }

        [Test]
        public void normalizeFace() {
            Assert.AreEqual("hello   hello", TwitterStringNormalizer.Instange.Normalize("hello :-) hello"));
            Assert.AreEqual("hello   hello", TwitterStringNormalizer.Instange.Normalize("hello ;) hello"));
            Assert.AreEqual("  hello", TwitterStringNormalizer.Instange.Normalize(":) hello"));
            Assert.AreEqual("hello  ", TwitterStringNormalizer.Instange.Normalize("hello :P"));
        }
    }
}
