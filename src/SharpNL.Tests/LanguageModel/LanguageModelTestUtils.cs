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

using System;
using System.Collections.Generic;
using java.math;
using SharpNL.LanguageModel;
using SharpNL.NGram;
using SharpNL.Utility;

namespace SharpNL.Tests.LanguageModel {
    internal static class LanguageModelTestUtils {
        private static readonly Random R = new Random();

        private static readonly char[] Chars = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j'};

        public static IList<StringList> GenerateRandomVocabulary(int size) {
            var vocabulary = new List<StringList>();
            for (var i = 0; i < size; i++) {
                var sentence = GenerateRandomSentence();
                vocabulary.Add(sentence);
            }
            return vocabulary;
        }

        public static StringList GenerateRandomSentence() {
            var dimension = R.Next(10) + 1;
            var sentence = new string[dimension];
            for (var j = 0; j < dimension; j++) {
                var i = R.Next(10);
                var c = Chars[i];
                sentence[j] = c + "-" + c + "-" + c;
            }
            return new StringList(sentence);
        }

        public static double GetPerplexity(ILanguageModel lm, IList<StringList> testSet, int ngramSize) {
            var perplexity = new BigDecimal(1d);

            foreach (var sentence in testSet) {
                foreach (var ngram in NGramUtils.GetNGrams(sentence, ngramSize)) {
                    var ngramProbability = lm.CalculateProbability(ngram);
                    perplexity = perplexity.multiply(new BigDecimal(1d).divide(new BigDecimal(ngramProbability), MathContext.DECIMAL128));
                }
            }

            var p = Math.Log(perplexity.doubleValue());
            if (double.IsInfinity(p) || double.IsNaN(p)) {
                return double.PositiveInfinity; // over/underflow -> too high perplexity
            }
            var log = new BigDecimal(p);
            return Math.Pow(Math.E, log.divide(new BigDecimal(testSet.Count), MathContext.DECIMAL128).doubleValue());
        }
    }
}