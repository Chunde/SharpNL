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
using SharpNL.SentenceDetector;

namespace SharpNL.Tests.Sentence {
    [TestFixture]
    public class DefaultEndOfSentenceScannerTest {


        [Test]
        public void TestScanning() {

            var scanner = new DefaultEndOfSentenceScanner(new [] {'.', '!', '?'});

            var pos = scanner.GetPositions("... um die Wertmarken zu auswählen !?");

            Assert.AreEqual(0, pos[0]);
            Assert.AreEqual(1, pos[1]);
            Assert.AreEqual(2, pos[2]);

            Assert.AreEqual(35, pos[3]);
            Assert.AreEqual(36, pos[4]);

        }

    }
}