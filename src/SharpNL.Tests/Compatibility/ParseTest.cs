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

using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SharpNL.Parser;

namespace SharpNL.Tests.Compatibility {

    //
    // DO NOT USE THIS TESTS AS SAMPLES TO BUILD YOUR STUFF !
    //  
    //  I use some things here, that are not needed in a "real" implementation 
    //

    [TestFixture]
    internal class ParseTest {

        internal const string modelFile = "opennlp/models/en-parser-chunking.bin";

        private static opennlp.tools.parser.ParserModel OpenJavaModel(string fileName) {
            java.io.FileInputStream inputStream = null;
            try {
                inputStream = OpenNLP.OpenInputStream(fileName);
                return new opennlp.tools.parser.ParserModel(inputStream);
            } finally {
                inputStream?.close();
            }
        }

        private static opennlp.tools.parser.Parser CreateJavaParser(string fileName) {
            return opennlp.tools.parser.ParserFactory.create(OpenJavaModel(fileName));
        }

        private static ParserModel OpenSharpModel(string fileName) {
            FileStream fileStream = null;
            try {
                fileStream = Tests.OpenFile(fileName);
                return new ParserModel(fileStream);
            } finally {
                fileStream?.Close();
            }
        }
        private static IParser CreateSharpParser(string fileName) {
            return ParserFactory.Create(OpenSharpModel(fileName));
        }

        [Test]
        public void TestParsers() {
            var jParser = CreateJavaParser(modelFile) as opennlp.tools.parser.chunking.Parser;
            var sParser = CreateSharpParser(modelFile) as SharpNL.Parser.Chunking.Parser;

            Assert.NotNull(jParser);
            Assert.NotNull(sParser);
        }

        [Test, TestOf(typeof(ParserTool))]
        public void TestQuestionsManually() {
            var sentences = new[] {
                "How are you?",
                "Do animals eat fruit ?",
                "What do animals eat ?",  
                "Why do animals eat fruit ?",
                "How much fruit do animals eat ?",
                "Where do animals eat ?",
                "When do animals eat ?"
            };

            var results = new[] {
                "(TOP (SBAR (WHADVP (WRB How)) (S (VP (VBP are))) (. you?)))",
                "(TOP (SQ (VBP Do) (NP (NNS animals)) (VP (VB eat) (NP (NN fruit))) (. ?)))",
                "(TOP (SBARQ (WHNP (WP What)) (SQ (VBP do) (NP (NNS animals)) (VP (VB eat))) (. ?)))", 
                "(TOP (SBARQ (WHADVP (WRB Why)) (SQ (VBP do) (NP (NNS animals)) (VP (VB eat) (NP (NN fruit)))) (. ?)))",
                "(TOP (SBAR (WHADVP (WRB How)) (S (NP (JJ much) (NN fruit)) (VP (VBP do) (S (NP (NNS animals)) (VP (VB eat))))) (. ?)))",
                "(TOP (SBARQ (WHADVP (WRB Where)) (SQ (VBP do) (NP (NNS animals)) (VP (VB eat))) (. ?)))",
                "(TOP (SBARQ (WHADVP (WRB When)) (SQ (VBP do) (NP (NNS animals)) (VP (VB eat))) (. ?)))"
            };

            var sParser = CreateSharpParser(modelFile);

            for (var i = 0; i < sentences.Length; i++) {
          
                var sParses = ParserTool.ParseLine(sentences[i], sParser, 1);

                Assert.That(sParses.Length, Is.EqualTo(1));
                Assert.That(sParses[0].ToString(), Is.EqualTo(results[i]));
            }
        }


        [Test, TestOf(typeof(ParserTool))]
        public void TestParse() {

            var jParser = CreateJavaParser(modelFile);
            var sParser = CreateSharpParser(modelFile);
            
            var sentences = new[] {
                "Let all your things have their places; let each part of your business have its time.",
                "It has become appallingly obvious that our technology has exceeded our humanity.",
                "The real problem is not whether machines think but whether men do.",  
                "The worst form of inequality is to try to make unequal things equal.",
                "People won't have time for you if you are always angry or complaining.",
                "To keep the body in good health is a duty... otherwise we shall not be able to keep our mind strong and clear.",
                "You, yourself, as much as anybody in the entire universe, deserve your love and affection."
            };


            foreach (var sentence in sentences) {

                var jParses = opennlp.tools.cmdline.parser.ParserTool.parseLine(sentence, jParser, 1);
                var sParses = ParserTool.ParseLine(sentence, sParser, 1);

                var jsb = new java.lang.StringBuffer();

                jParses[0].show(jsb);

                Assert.That(sParses[0].ToString(), Is.EqualTo(jsb.ToString()));
            }
        }

        [Test, TestOf(typeof(ParserTool))]
        public void TestQuestions() {
            var sentences = new [] {
                "How are you?",
                "Do animals eat fruit ?",
                "What do animals eat ?",  
                "Why do animals eat fruit ?",
                "How much fruit do animals eat ?",
                "Where do animals eat ?",
                "When do animals eat ?"
            };

            var jParser = CreateJavaParser(modelFile);
            var sParser = CreateSharpParser(modelFile);

            foreach (var sentence in sentences) {
                var jParses = opennlp.tools.cmdline.parser.ParserTool.parseLine(sentence, jParser, 1);
                var sParses = ParserTool.ParseLine(sentence, sParser, 1);

                CheckParseChild(jParses, sParses);
            }
        }

        private static void CheckParseChild(IReadOnlyList<opennlp.tools.parser.Parse> jParses, IReadOnlyList<Parse> sParses) {

            Assert.That(sParses.Count, Is.EqualTo(jParses.Count));

            for (var i = 0; i < sParses.Count; i++) {
                var jParse = jParses[i];
                var sParse = sParses[i];
                
                Assert.That(sParse.Type, Is.EqualTo(jParse.getType()));
                
                Assert.That(sParse.IsFlat, Is.EqualTo(jParse.isFlat()));
                Assert.That(sParse.IsPosTag, Is.EqualTo(jParse.isPosTag()));
                Assert.That(sParse.IsChunk, Is.EqualTo(jParse.isChunk()));

                Assert.That(sParse.ChildCount, Is.EqualTo(jParse.getChildCount()));

                if (sParse.ChildCount > 0)
                    CheckParseChild(jParse.getChildren(), sParse.Children);
            }
        }       
    }
}