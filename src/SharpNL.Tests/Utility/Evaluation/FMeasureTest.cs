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

using System;
using NUnit.Framework;
using SharpNL.Utility;
using SharpNL.Utility.Evaluation;

namespace SharpNL.Tests.Utility.Evaluation {
    public class FMeasureTest {
        private const double Delta = 1.0E-9d;

        private readonly object[] gold = {
            new Span(8, 9),
            new Span(9, 10),
            new Span(10, 12),
            new Span(13, 14),
            new Span(14, 15),
            new Span(15, 16)
        };

        private readonly object[] goldToMerge = {
            new Span(8, 9),
            new Span(9, 10),
            new Span(11, 11),
            new Span(13, 14),
            new Span(14, 15),
            new Span(15, 16),
            new Span(18, 19)
        };

        private readonly object[] predicted = {
            new Span(14, 15),
            new Span(15, 16),
            new Span(100, 120),
            new Span(210, 220),
            new Span(220, 230)
        };

        private readonly object[] predictedCompletelyDistinct = {
            new Span(100, 120),
            new Span(210, 220),
            new Span(211, 220),
            new Span(212, 220),
            new Span(220, 230)
        };

        private readonly object[] predictedToMerge = {
            new Span(8, 9),
            new Span(14, 15),
            new Span(15, 16),
            new Span(100, 120),
            new Span(210, 220),
            new Span(220, 230)
        };

        [Test]
        public void TestCountTruePositives() {
            Assert.AreEqual(0, FMeasure<object>.CountTruePositives(new object[] {}, new object[] {}));
            Assert.AreEqual(gold.Length, FMeasure<object>.CountTruePositives(gold, gold));
            Assert.AreEqual(0, FMeasure<object>.CountTruePositives(gold, predictedCompletelyDistinct));
            Assert.AreEqual(2, FMeasure<object>.CountTruePositives(gold, predicted));
        }

        [Test]
        public void TestPrecision() {
            Assert.AreEqual(1.0d, FMeasure<object>.Precision(gold, gold), Delta);
            Assert.AreEqual(0, FMeasure<object>.Precision(gold, predictedCompletelyDistinct), Delta);
            Assert.AreEqual(Double.NaN, FMeasure<object>.Precision(gold, new object[] { }), Delta);
            Assert.AreEqual(0, FMeasure<object>.Precision(new object[] { }, gold), Delta);
            Assert.AreEqual(2d / predicted.Length, FMeasure<object>.Precision(gold, predicted), Delta);
        }

        [Test]
        public void TestRecall() {
            Assert.AreEqual(1.0d, FMeasure<object>.Recall(gold, gold), Delta);
            Assert.AreEqual(0, FMeasure<object>.Recall(gold, predictedCompletelyDistinct), Delta);
            Assert.AreEqual(0, FMeasure<object>.Recall(gold, new object[] { }), Delta);
            Assert.AreEqual(Double.NaN, FMeasure<object>.Recall(new object[] { }, gold), Delta);
            Assert.AreEqual(2d / gold.Length, FMeasure<object>.Recall(gold, predicted), Delta);
        }

        [Test]
        public void TestEmpty() {
            var fm = new FMeasure<object>();
            Assert.AreEqual(-1, fm.Value, Delta);
            Assert.AreEqual(0, fm.RecallScore, Delta);
            Assert.AreEqual(0, fm.PrecisionScore, Delta);
        }

        [Test]
        public void TestPerfect() {
            var fm = new FMeasure<object>();
            fm.UpdateScores(gold, gold);
            Assert.AreEqual(1, fm.Value, Delta);
            Assert.AreEqual(1, fm.RecallScore, Delta);
            Assert.AreEqual(1, fm.PrecisionScore, Delta);
        }

        [Test]
        public void TestMerge() {
            var fm = new FMeasure<object>();
            fm.UpdateScores(gold, predicted);
            fm.UpdateScores(goldToMerge, predictedToMerge);

            var fmMerge = new FMeasure<object>();
            fmMerge.UpdateScores(gold, predicted);
            var toMerge = new FMeasure<object>();
            toMerge.UpdateScores(goldToMerge, predictedToMerge);
            fmMerge.MergeInto(toMerge);

            double selected1 = predicted.Length;
            double target1 = gold.Length;
            double tp1 = FMeasure<object>.CountTruePositives(gold, predicted);

            double selected2 = predictedToMerge.Length;
            double target2 = goldToMerge.Length;
            double tp2 = FMeasure<object>.CountTruePositives(goldToMerge, predictedToMerge);


            Assert.AreEqual((tp1 + tp2)/(target1 + target2), fm.RecallScore, Delta);
            Assert.AreEqual((tp1 + tp2)/(selected1 + selected2), fm.PrecisionScore, Delta);

            Assert.AreEqual(fm.RecallScore, fmMerge.RecallScore, Delta);
            Assert.AreEqual(fm.PrecisionScore, fmMerge.PrecisionScore, Delta);
        }
    }
}