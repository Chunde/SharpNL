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
using SharpNL.Formats.LetsMT;

namespace SharpNL.Tests.Formats.Letsmt {
    [TestFixture]
    public class LetsmtDocumentTest {
        [Test]
        public void TestParsingSimpleDoc() {
            using (var file = Tests.OpenFile("/opennlp/tools/formats/letsmt/letsmt-with-words.xml")) {
                var doc = LetsmtDocument.Parse(file);

                Assert.AreEqual(2, doc.Sentences.Count);

                Assert.IsNull(doc.Sentences[0].NonTokenizedText);
                CollectionAssert.AreEqual(new[] {
                    "The",
                    "Apache",
                    "Software",
                    "Foundation",
                    "uses",
                    "various",
                    "licenses",
                    "to",
                    "distribute",
                    "software",
                    "and",
                    "documentation",
                    ",",
                    "to",
                    "accept",
                    "regular",
                    "contributions",
                    "from",
                    "individuals",
                    "and",
                    "corporations",
                    ",",
                    "and",
                    "to",
                    "accept",
                    "larger",
                    "grants",
                    "of",
                    "existing",
                    "software",
                    "products",
                    "."
                }, doc.Sentences[0].Tokens);

                Assert.IsNull(doc.Sentences[1].NonTokenizedText);

                CollectionAssert.AreEqual(new[] {
                    "All",
                    "software",
                    "produced",
                    "by",
                    "The",
                    "Apache",
                    "Software",
                    "Foundation",
                    "or",
                    "any",
                    "of",
                    "its",
                    "projects",
                    "or",
                    "subjects",
                    "is",
                    "licensed",
                    "according",
                    "to",
                    "the",
                    "terms",
                    "of",
                    "the",
                    "documents",
                    "listed",
                    "below",
                    "."
                }, doc.Sentences[1].Tokens);
            }
        }
    }
}
