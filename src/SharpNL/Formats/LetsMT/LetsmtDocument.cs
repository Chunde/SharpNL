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

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using SharpNL.Utility;

namespace SharpNL.Formats.LetsMT {
    /// <summary>
    /// A structure to hold the letsmt document. 
    /// The documents contains sentences and depending on the source it either contains tokenized text(words) or an un-tokenized sentence string.
    /// </summary>
    /// <remarks>
    /// The format specification can be found <see href="http://project.letsmt.eu/uploads/Deliverables/D2.1%20%20Specification%20of%20data%20formats%20v1%20final.pdf">here</see>.
    /// </remarks>
    public class LetsmtDocument {

        /// <summary>
        /// Gets the document sentences.
        /// </summary>
        public List<LetsmtSentence> Sentences { get; } = new List<LetsmtSentence>();

        /*
        <s id="5">
            <chunk type="NP" id="c-1">
                <w tree="NN" tnt="NNP" lem="madam" id="w5.1">Madam</w>
                <w tree="NP" tnt="NNP" lem="President" id="w5.2">President</w>
            </chunk>
            <w tree="," tnt="," lem="," id="w5.3">,</w>
            <chunk type="PP" id="c-3">
                <w tree="IN" tnt="IN" lem="on" id="w5.4">on</w>
            </chunk>
            <chunk type="NP" id="c-4">
                <w tree="DT" tnt="DT" lem="a" id="w5.5">a</w>
                <w tree="NN" tnt="NN" lem="point" id="w5.6">point</w>
            </chunk>
            <chunk type="SBAR" id="c-5">
                <w tree="IN" tnt="IN" lem="of" id="w5.7">of</w>
            </chunk>
            <w tree="NN" tnt="NN" lem="order" id="w5.8">order</w>
            <w tree="SENT" tnt="." lem="." id="w5.9">.</w>
        </s>
        */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <exception cref="InvalidFormatException">A orphan w tag was found in the document.</exception>
        public static LetsmtDocument Parse(Stream stream) {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead) throw new ArgumentException("The stream is not readable.", nameof(stream));

            var doc = new LetsmtDocument();
            using (var reader = XmlReader.Create(stream, new XmlReaderSettings {
                IgnoreComments = true, IgnoreWhitespace = true
            })) {
                LetsmtSentence sentence = null;
                while (reader.Read()) {
                    while (reader.Read()) {
                        if (reader.NodeType == XmlNodeType.Element) {
                            switch (reader.Name) {
                                case "s":
                                    sentence = new LetsmtSentence {
                                        Id = reader.GetAttribute("id")
                                    };
                                    break;
                                case "w":
                                    if (sentence == null)
                                        throw new InvalidFormatException("A orphan w tag was found in the document.");

                                    if (reader.Read() && reader.NodeType == XmlNodeType.Text) {
                                        sentence.Tokens.Add(reader.Value);

                                        if (!reader.Read() || reader.NodeType != XmlNodeType.EndElement) {
                                            throw new InvalidDataException("Unable to read the end of the element.");
                                        }
                                    }
                                    break;
                            }
                        } else if (reader.NodeType == XmlNodeType.Text) {
                            if (sentence != null && sentence.Tokens.Count == 0) {
                                sentence.NonTokenizedText = reader.Value;
                            }
                        } else if (reader.NodeType == XmlNodeType.EndElement) {
                            if (reader.Name == "s" && sentence?.HasContent == true) {
                                doc.Sentences.Add(sentence);
                            }
                        }
                    }
                }
            }
            return doc;
        }
    }
}
