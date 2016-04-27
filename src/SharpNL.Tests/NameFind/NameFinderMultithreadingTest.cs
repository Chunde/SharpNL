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
using NUnit.Framework;
using SharpNL.NameFind;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpNL.Tokenize;

namespace SharpNL.Tests.NameFind {

    [TestFixture]
    public class NameFinderMultithreadingTest {

        internal const string fileName = "opennlp/models/en-ner-person.bin";

        private TokenNameFinderModel modelFile;

        [Test]
        public void MultithreadingTest() {

            const int threadCount = 100;

            // The expensive part of the code is to load the model!
            // but the model file can be shared.
           
            var fileStream = Tests.OpenFile(fileName); 
            modelFile = new TokenNameFinderModel(fileStream);

            var fileContents = File.ReadAllText(Tests.GetFullPath("/opennlp/tools/sentdetect/Sentences.txt"));
            var sentences = fileContents.Split(new [] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            var rnd = new Random();
            
            var count = 0;

            var delegates = new List<ThreadStart>(threadCount);
            for (var i = 0; i < threadCount; i++)
                delegates.Add(() => {

                    // Use ONE NameFinderME instance per thread !

                    var nameFinder = new NameFinderME(modelFile);
                    var tokens = WhitespaceTokenizer.Instance.Tokenize(sentences[rnd.Next(0, sentences.Length - 1)]);

                    Thread.Sleep(rnd.Next(100, 300));

                    var names = nameFinder.Find(tokens);

                    count += names.Length;
                });
            
            var threads = delegates.Select(d => new CrossThreadTestRunner(d)).ToList();
            foreach (var thread in threads)
                thread.Start();

            foreach (var thread in threads) 
                thread.Join();

            Assert.That(count, Is.GreaterThan(0));

        }
    }
}