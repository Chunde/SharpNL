// 
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

using System.IO;
using NUnit.Framework;
using SharpNL.Tokenize;

namespace SharpNL.Tests.Tokenize {
    [TestFixture, TestOf(typeof (DetokenizationDictionary))]
    public class DetokenizationDictionaryTest {
        private DetokenizationDictionary dict;

        [OneTimeSetUp]
        public void Setup() {
            dict = new DetokenizationDictionary {
                {"\"", DetokenizationDictionary.Operation.RightLeftMatching},
                {"(", DetokenizationDictionary.Operation.MoveRight},
                {")", DetokenizationDictionary.Operation.MoveLeft},
                {"-", DetokenizationDictionary.Operation.MoveBoth}
            };
        }

        private static void TestEntries(DetokenizationDictionary dict) {
            Assert.AreEqual(DetokenizationDictionary.Operation.RightLeftMatching, dict["\""]);
            Assert.AreEqual(DetokenizationDictionary.Operation.MoveRight, dict["("]);
            Assert.AreEqual(DetokenizationDictionary.Operation.MoveLeft, dict[")"]);
            Assert.AreEqual(DetokenizationDictionary.Operation.MoveBoth, dict["-"]);
        }

        [Test]
        public void TestSerialization() {
            using (var data = new MemoryStream()) {
                dict.Serialize(data);
                data.Seek(0, SeekOrigin.Begin);

                var parsedDict = new DetokenizationDictionary(data);

                TestEntries(parsedDict);
            }
        }

        [Test]
        public void TestSimpleDict() {
            TestEntries(dict);
        }
    }
}