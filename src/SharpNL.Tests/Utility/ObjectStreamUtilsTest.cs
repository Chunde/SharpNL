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
using SharpNL.Utility;

namespace SharpNL.Tests.Utility {
    [TestFixture]
    internal class ObjectStreamUtilsTest {

        [Test]
        public void createObjectStreamTest() {
            var data = new [] {"dog", "cat", "pig", "frog"};

            var stream = ObjectStreamUtils.CreateObjectStream(data);

            foreach (var token in data) {
                Assert.AreEqual(token, stream.Read());
            }

            Assert.IsNull(stream.Read());
        }

        [Test]
        public void concatenateStreamTest() {
            var data1 = new [] { "dog1", "cat1", "pig1", "frog1" };
            var data2 = new[] { "dog2", "cat2", "pig2", "frog2" };
            var expected = new[] { "dog1", "cat1", "pig1", "frog1", "dog2", "cat2", "pig2", "frog2" };

            var stream = ObjectStreamUtils.ConcatenateObjectStream(data1, data2);

            foreach (var value in expected) {
                Assert.AreEqual(value, stream.Read());
            }

            Assert.IsNull(stream.Read());
        }

    }
}
