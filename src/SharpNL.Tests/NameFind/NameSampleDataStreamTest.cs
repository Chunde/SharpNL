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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using SharpNL.NameFind;
using SharpNL.Utility;

namespace SharpNL.Tests.NameFind {
    [TestFixture]
    public class NameSampleDataStreamTest {
        private const string Person = "person";
        private const string Date = "date";
        private const string Location = "location";
        private const string Organization = "organization";

        private static Span CreateDefaultSpan(int start, int end) {
            return new Span(start, end, NameSample.DefaultType);
        }

        [Test]
        public void TestWithoutNameTypes() {
            using (var file = Tests.OpenFile("opennlp/tools/namefind/AnnotatedSentences.txt")) {
                var sampleStream = new NameSampleStream(new PlainTextByLineStream(file, "ISO-8859-1"));
                var expectedNames = new[] {
                    "Alan McKennedy", "Julie", "Marie Clara",
                    "Stefanie Schmidt", "Mike", "Stefanie Schmidt", "George", "Luise",
                    "George Bauer", "Alisa Fernandes", "Alisa", "Mike Sander",
                    "Stefan Miller", "Stefan Miller", "Stefan Miller", "Elenor Meier",
                    "Gina Schneider", "Bruno Schulz", "Michel Seile", "George Miller",
                    "Miller", "Peter Schubert", "Natalie"
                };

                var names = new List<string>();
                var spans = new List<Span>();
                NameSample ns;
                while ((ns = sampleStream.Read()) != null) {
                    foreach (var name in ns.Names) {                       
                        names.Add(name.GetCoveredText(ns.Sentence));
                        spans.Add(name);
                    }
                    
                }

                Assert.AreEqual(expectedNames.Length, names.Count);
                Assert.AreEqual(CreateDefaultSpan(6, 8), spans[0]);
                Assert.AreEqual(CreateDefaultSpan(3, 4), spans[1]);
                Assert.AreEqual(CreateDefaultSpan(1, 3), spans[2]);
                Assert.AreEqual(CreateDefaultSpan(4, 6), spans[3]);
                Assert.AreEqual(CreateDefaultSpan(1, 2), spans[4]);
                Assert.AreEqual(CreateDefaultSpan(4, 6), spans[5]);
                Assert.AreEqual(CreateDefaultSpan(2, 3), spans[6]);
                Assert.AreEqual(CreateDefaultSpan(16, 17), spans[7]);
                Assert.AreEqual(CreateDefaultSpan(18, 20), spans[8]);
                Assert.AreEqual(CreateDefaultSpan(0, 2), spans[9]);
                Assert.AreEqual(CreateDefaultSpan(0, 1), spans[10]);
                Assert.AreEqual(CreateDefaultSpan(3, 5), spans[11]);
                Assert.AreEqual(CreateDefaultSpan(3, 5), spans[12]);
                Assert.AreEqual(CreateDefaultSpan(10, 12), spans[13]);
                Assert.AreEqual(CreateDefaultSpan(1, 3), spans[14]);
                Assert.AreEqual(CreateDefaultSpan(6, 8), spans[15]);
                Assert.AreEqual(CreateDefaultSpan(6, 8), spans[16]);
                Assert.AreEqual(CreateDefaultSpan(8, 10), spans[17]);
                Assert.AreEqual(CreateDefaultSpan(12, 14), spans[18]);
                Assert.AreEqual(CreateDefaultSpan(1, 3), spans[19]);
                Assert.AreEqual(CreateDefaultSpan(0, 1), spans[20]);
                Assert.AreEqual(CreateDefaultSpan(2, 4), spans[21]);
                Assert.AreEqual(CreateDefaultSpan(5, 6), spans[22]);
            }
        }

        [Test]
        public void TestWithoutNameTypeAndInvalidData1() {
			Assert.Throws<InvalidOperationException> (() => {
				var sampleStream = new NameSampleStream (new GenericObjectStream<string> (
					                           "<START> <START> Name <END>"));

				sampleStream.Read ();
			});
        }

        [Test]
        public void TestWithoutNameTypeAndInvalidData2() {
			Assert.Throws<InvalidOperationException> (() => {
				var sampleStream = new NameSampleStream (new GenericObjectStream<string> (
					                           "<START> Name <END> <END>"));

				sampleStream.Read ();
			});
        }


        [Test]
        public void TestWithoutNameTypeAndInvalidData3() {
			Assert.Throws<InvalidOperationException> (() => {
				var sampleStream = new NameSampleStream (new GenericObjectStream<string> (
					                           "<START> <START> Person <END> Street <END>"));

				sampleStream.Read ();
			});
        }

        [Test]
        public void TestWithNameTypeAndInvalidData1() {
			Assert.Throws<InvalidOperationException> (() => {
				var sampleStream = new NameSampleStream (new GenericObjectStream<string> (
					                           "<START:> Name <END>"));

				sampleStream.Read ();
			});
        }

        [Test]
        public void TestWithNameTypeAndInvalidData2() {
			Assert.Throws<InvalidOperationException> (() => {
				var sampleStream = new NameSampleStream (new GenericObjectStream<string> (
					                           "<START:street> <START:person> Name <END> <END>"));

				sampleStream.Read ();
			});
        }

        [Test]
        public void TestWithNameTypes() {
            using (var file = Tests.OpenFile("opennlp/tools/namefind/voa1.train")) {
                var sampleStream = new NameSampleStream(new PlainTextByLineStream(file, "ISO-8859-1"));
                var names = new Dictionary<string, List<string>>();
                var spans = new Dictionary<string, List<Span>>();

                NameSample ns;
                while ((ns = sampleStream.Read()) != null) {
                    foreach (var nameSpan in ns.Names) {

                        if (!names.ContainsKey(nameSpan.Type)) {

                            names.Add(nameSpan.Type, new List<string>());
                            spans.Add(nameSpan.Type, new List<Span>());

                        }
                        names[nameSpan.Type].Add(nameSpan.GetCoveredText(ns.Sentence));
                        spans[nameSpan.Type].Add(nameSpan);
                    }
                }

                string[] expectedPerson = {
                    "Barack Obama", "Obama", "Obama",
                    "Lee Myung - bak", "Obama", "Obama", "Scott Snyder", "Snyder", "Obama",
                    "Obama", "Obama", "Tim Peters", "Obama", "Peters"
                };

                string[] expectedDate = {"Wednesday", "Thursday", "Wednesday"};

                string[] expectedLocation = {
                    "U . S .", "South Korea", "North Korea",
                    "China", "South Korea", "North Korea", "North Korea", "U . S .",
                    "South Korea", "United States", "Pyongyang", "North Korea",
                    "South Korea", "Afghanistan", "Seoul", "U . S .", "China"
                };

                string[] expectedOrganization = {"Center for U . S . Korea Policy"};

                Assert.AreEqual(expectedPerson.Length, names[Person].Count);
                Assert.AreEqual(expectedDate.Length, names[Date].Count);
                Assert.AreEqual(expectedLocation.Length, names[Location].Count);
                Assert.AreEqual(expectedOrganization.Length, names[Organization].Count);

                Assert.AreEqual(new Span(5, 7, Person), spans[Person][0]);
                Assert.AreEqual(expectedPerson[0], names[Person][0]);
                Assert.AreEqual(new Span(10, 11, Person), spans[Person][1]);
                Assert.AreEqual(expectedPerson[1], names[Person][1]);
                Assert.AreEqual(new Span(29, 30, Person), spans[Person][2]);
                Assert.AreEqual(expectedPerson[2], names[Person][2]);
                Assert.AreEqual(new Span(23, 27, Person), spans[Person][3]);
                Assert.AreEqual(expectedPerson[3], names[Person][3]);
                Assert.AreEqual(new Span(1, 2, Person), spans[Person][4]);
                Assert.AreEqual(expectedPerson[4], names[Person][4]);
                Assert.AreEqual(new Span(8, 9, Person), spans[Person][5]);
                Assert.AreEqual(expectedPerson[5], names[Person][5]);
                Assert.AreEqual(new Span(0, 2, Person), spans[Person][6]);
                Assert.AreEqual(expectedPerson[6], names[Person][6]);
                Assert.AreEqual(new Span(25, 26, Person), spans[Person][7]);
                Assert.AreEqual(expectedPerson[7], names[Person][7]);
                Assert.AreEqual(new Span(1, 2, Person), spans[Person][8]);
                Assert.AreEqual(expectedPerson[8], names[Person][8]);
                Assert.AreEqual(new Span(6, 7, Person), spans[Person][9]);
                Assert.AreEqual(expectedPerson[9], names[Person][9]);
                Assert.AreEqual(new Span(14, 15, Person), spans[Person][10]);
                Assert.AreEqual(expectedPerson[10], names[Person][10]);
                Assert.AreEqual(new Span(0, 2, Person), spans[Person][11]);
                Assert.AreEqual(expectedPerson[11], names[Person][11]);
                Assert.AreEqual(new Span(12, 13, Person), spans[Person][12]);
                Assert.AreEqual(expectedPerson[12], names[Person][12]);
                Assert.AreEqual(new Span(12, 13, Person), spans[Person][13]);
                Assert.AreEqual(expectedPerson[13], names[Person][13]);

                Assert.AreEqual(new Span(7, 8, Date), spans[Date][0]);
                Assert.AreEqual(expectedDate[0], names[Date][0]);
                Assert.AreEqual(new Span(27, 28, Date), spans[Date][1]);
                Assert.AreEqual(expectedDate[1], names[Date][1]);
                Assert.AreEqual(new Span(15, 16, Date), spans[Date][2]);
                Assert.AreEqual(expectedDate[2], names[Date][2]);

                Assert.AreEqual(new Span(0, 4, Location), spans[Location][0]);
                Assert.AreEqual(expectedLocation[0], names[Location][0]);
                Assert.AreEqual(new Span(10, 12, Location), spans[Location][1]);
                Assert.AreEqual(expectedLocation[1], names[Location][1]);
                Assert.AreEqual(new Span(28, 30, Location), spans[Location][2]);
                Assert.AreEqual(expectedLocation[2], names[Location][2]);
                Assert.AreEqual(new Span(3, 4, Location), spans[Location][3]);
                Assert.AreEqual(expectedLocation[3], names[Location][3]);
                Assert.AreEqual(new Span(5, 7, Location), spans[Location][4]);
                Assert.AreEqual(expectedLocation[4], names[Location][4]);
                Assert.AreEqual(new Span(16, 18, Location), spans[Location][5]);
                Assert.AreEqual(expectedLocation[5], names[Location][5]);
                Assert.AreEqual(new Span(1, 3, Location), spans[Location][6]);
                Assert.AreEqual(expectedLocation[6], names[Location][6]);
                Assert.AreEqual(new Span(5, 9, Location), spans[Location][7]);
                Assert.AreEqual(expectedLocation[7], names[Location][7]);
                Assert.AreEqual(new Span(0, 2, Location), spans[Location][8]);
                Assert.AreEqual(expectedLocation[8], names[Location][8]);
                Assert.AreEqual(new Span(4, 6, Location), spans[Location][9]);
                Assert.AreEqual(expectedLocation[9], names[Location][9]);
                Assert.AreEqual(new Span(10, 11, Location), spans[Location][10]);
                Assert.AreEqual(expectedLocation[10], names[Location][10]);
                Assert.AreEqual(new Span(6, 8, Location), spans[Location][11]);
                Assert.AreEqual(expectedLocation[11], names[Location][11]);
                Assert.AreEqual(new Span(4, 6, Location), spans[Location][12]);
                Assert.AreEqual(expectedLocation[12], names[Location][12]);
                Assert.AreEqual(new Span(10, 11, Location), spans[Location][13]);
                Assert.AreEqual(expectedLocation[13], names[Location][13]);
                Assert.AreEqual(new Span(12, 13, Location), spans[Location][14]);
                Assert.AreEqual(expectedLocation[14], names[Location][14]);
                Assert.AreEqual(new Span(5, 9, Location), spans[Location][15]);
                Assert.AreEqual(expectedLocation[15], names[Location][15]);
                Assert.AreEqual(new Span(11, 12, Location), spans[Location][16]);
                Assert.AreEqual(expectedLocation[16], names[Location][16]);

                Assert.AreEqual(new Span(7, 15, Organization), spans[Organization][0]);
                Assert.AreEqual(expectedOrganization[0], names[Organization][0]);

            }
        }


        [Test]
        public void TestClearAdaptiveData() {
            var trainingData = new StringBuilder();
            trainingData.Append("a\n");
            trainingData.Append("b\n");
            trainingData.Append("c\n");
            trainingData.Append("\n");
            trainingData.Append("d\n");

            var untokenizedLineStream = new PlainTextByLineStream(new StringReader(trainingData.ToString()));
            var trainingStream = new NameSampleStream(untokenizedLineStream);

            Assert.False(trainingStream.Read().ClearAdaptiveData);
            Assert.False(trainingStream.Read().ClearAdaptiveData);
            Assert.False(trainingStream.Read().ClearAdaptiveData);
            Assert.True(trainingStream.Read().ClearAdaptiveData);
            Assert.Null(trainingStream.Read());
        }

        [Test]
        public void TestHtmlNameSampleParsing() {
            using (var file = Tests.OpenFile("opennlp/tools/namefind/html1.train")) {
                var ds = new NameSampleStream(new PlainTextByLineStream(file));

                NameSample ns = ds.Read();

                Assert.AreEqual(1, ns.Sentence.Length);
                Assert.AreEqual("<html>", ns.Sentence[0]);

                ns = ds.Read();
                Assert.AreEqual(1, ns.Sentence.Length);
                Assert.AreEqual("<head/>", ns.Sentence[0]);

                ns = ds.Read();
                Assert.AreEqual(1, ns.Sentence.Length);
                Assert.AreEqual("<body>", ns.Sentence[0]);

                ns = ds.Read();
                Assert.AreEqual(1, ns.Sentence.Length);
                Assert.AreEqual("<ul>", ns.Sentence[0]);

                // <li> <START:organization> Advanced Integrated Pest Management <END> </li>
                ns = ds.Read();
                Assert.AreEqual(6, ns.Sentence.Length);
                Assert.AreEqual("<li>", ns.Sentence[0]);
                Assert.AreEqual("Advanced", ns.Sentence[1]);
                Assert.AreEqual("Integrated", ns.Sentence[2]);
                Assert.AreEqual("Pest", ns.Sentence[3]);
                Assert.AreEqual("Management", ns.Sentence[4]);
                Assert.AreEqual("</li>", ns.Sentence[5]);
                Assert.AreEqual(new Span(1, 5, Organization), ns.Names[0]);

                // <li> <START:organization> Bay Cities Produce Co., Inc. <END> </li>
                ns = ds.Read();
                Assert.AreEqual(7, ns.Sentence.Length);
                Assert.AreEqual("<li>", ns.Sentence[0]);
                Assert.AreEqual("Bay", ns.Sentence[1]);
                Assert.AreEqual("Cities", ns.Sentence[2]);
                Assert.AreEqual("Produce", ns.Sentence[3]);
                Assert.AreEqual("Co.,", ns.Sentence[4]);
                Assert.AreEqual("Inc.", ns.Sentence[5]);
                Assert.AreEqual("</li>", ns.Sentence[6]);
                Assert.AreEqual(new Span(1, 6, Organization), ns.Names[0]);

                ns = ds.Read();
                Assert.AreEqual(1, ns.Sentence.Length);
                Assert.AreEqual("</ul>", ns.Sentence[0]);

                ns = ds.Read();
                Assert.AreEqual(1, ns.Sentence.Length);
                Assert.AreEqual("</body>", ns.Sentence[0]);

                ns = ds.Read();
                Assert.AreEqual(1, ns.Sentence.Length);
                Assert.AreEqual("</html>", ns.Sentence[0]);

                Assert.Null(ds.Read());
            }
        }
    }
}