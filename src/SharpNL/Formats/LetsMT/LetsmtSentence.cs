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

using System.Collections.Generic;

namespace SharpNL.Formats.LetsMT {
    /// <summary>
    /// Represents a LetsMT sentence.
    /// </summary>
    public class LetsmtSentence {

        /// <summary>
        /// Gets the sentence identifier.
        /// </summary>
        public string Id { get; internal set; }

        /// <summary>
        /// Gets the tokens of the sentence.
        /// </summary>
        public List<string> Tokens { get; } = new List<string>();

        /// <summary>
        /// Gets the non tokenized text.
        /// </summary>
        /// <remarks>I think this is not described in the original specs, but opennlp uses.</remarks>
        public string NonTokenizedText { get; internal set; }

        /// <summary>
        /// Determines whether this sentence has content.
        /// </summary>
        internal bool HasContent => Tokens.Count > 0 || !string.IsNullOrEmpty(NonTokenizedText);

    }
}
