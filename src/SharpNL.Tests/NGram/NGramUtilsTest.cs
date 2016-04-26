//  
//  Copyright 2016 Gustavo J Knuppe (https://github.com/knuppe)
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

using System.Collections.Generic;
using NUnit.Framework;
using SharpNL.NGram;
using SharpNL.Utility;

namespace SharpNL.Tests.NGram {
    [TestFixture]
    public class NGramUtilsTest {
        [Test]
        public void TestBigramMLProbability() {
            var set = new List<StringList> {
                new StringList("<s>", "I", "am", "Sam", "</s>"),
                new StringList("<s>", "Sam", "I", "am", "</s>"),
                new StringList("<s>", "I", "do", "not", "like", "green", "eggs", "and", "ham", "</s>"),
                new StringList("")
            };

            var d = NGramUtils.CalculateBigramMLProbability("<s>", "I", set);
            Assert.That(d, Is.EqualTo(0.6666666666666666d).Within(0.000001));

            d = NGramUtils.CalculateBigramMLProbability("Sam", "</s>", set);
            Assert.That(d, Is.EqualTo(0.5d).Within(0.000001));

            d = NGramUtils.CalculateBigramMLProbability("<s>", "Sam", set);
            Assert.That(d, Is.EqualTo(0.3333333333333333d).Within(0.000001));
        }

        [Test]
        public void TestGetNGrams() {
            var nGrams = NGramUtils.GetNGrams(new StringList("I", "saw", "brown", "fox"), 2);

            Assert.That(nGrams.Count, Is.EqualTo(3));

            nGrams = NGramUtils.GetNGrams(new StringList("I", "saw", "brown", "fox"), 3);
            Assert.That(nGrams.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestLinearInterpolation() {
            var set = new List<StringList> {
                new StringList("the", "green", "book", "STOP"),
                new StringList("my", "blue", "book", "STOP"),
                new StringList("his", "green", "house", "STOP"),
                new StringList("book", "STOP")
            };
            var lambda = 1d/3d;
            var d = NGramUtils.CalculateTrigramLinearInterpolationProbability("the", "green", "book", set, lambda,
                lambda, lambda);

            Assert.That(d, Is.EqualTo(0.5714285714285714d).Within(0.000000000001));
        }

        [Test]
        public void TestLinearInterpolation2() {
            var set = new List<StringList> {
                new StringList("D", "N", "V", "STOP"),
                new StringList("D", "N", "V", "STOP")
            };
            var lambda = 1d/3d;
            var d = NGramUtils.CalculateTrigramLinearInterpolationProbability("N", "V", "STOP", set, lambda, lambda,
                lambda);

            Assert.That(d, Is.EqualTo(1d).Within(0.75d));
        }

        [Test]
        public void TestNgramMLProbability() {
            var set = new List<StringList> {
                new StringList("<s>", "I", "am", "Sam", "</s>"),
                new StringList("<s>", "Sam", "I", "am", "</s>"),
                new StringList("<s>", "I", "do", "not", "like", "green", "eggs", "and", "ham", "</s>"),
                new StringList("")
            };
            var d = NGramUtils.CalculateNgramMLProbability(new StringList("I", "am", "Sam"), set);
            Assert.That(d, Is.EqualTo(0.5d).Within(0.00001));

            d = NGramUtils.CalculateNgramMLProbability(new StringList("Sam", "I", "am"), set);
            Assert.That(d, Is.EqualTo(1d).Within(0.00001));
        }


        [Test]
        public void TestTrigramMLProbability() {
            var set = new List<StringList> {
                new StringList("<s>", "I", "am", "Sam", "</s>"),
                new StringList("<s>", "Sam", "I", "am", "</s>"),
                new StringList("<s>", "I", "do", "not", "like", "green", "eggs", "and", "ham", "</s>"),
                new StringList("")
            };
            var d = NGramUtils.CalculateTrigramMLProbability("I", "am", "Sam", set);
            Assert.That(d, Is.EqualTo(0.5d).Within(0.00001));

            d = NGramUtils.CalculateTrigramMLProbability("Sam", "I", "am", set);
            Assert.That(d, Is.EqualTo(1d).Within(0.00001));
        }
    }
}