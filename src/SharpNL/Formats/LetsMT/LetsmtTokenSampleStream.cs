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

namespace SharpNL.Formats.LetsMT {

    using Tokenize;
    using Utility;

    /// <summary>
    /// Represents a LetsMT! <see cref="TokenSample"/> stream. This class cannot be inherited.
    /// </summary>
    /// <remarks>This implementation ignores the sentences without tokens (<see cref="LetsmtSentence.NonTokenizedText"/>).</remarks>
    /// <inheritdoc cref="Disposable" />
    public sealed class LetsmtTokenSampleStream : Disposable, IObjectStream<TokenSample> {
        private readonly IDetokenizer detokenizer;
        private readonly LetsmtDocument document;

        private int index;

        /// <summary>
        /// Creates a new LetsMT! token sample stream with the specified document using the <see cref="F:SharpNL.Tokenize.DictionaryDetokenizer.LatinDetokenizer" />.
        /// </summary>
        /// <param name="document">The LetsMT! document</param>
        /// <inheritdoc />
        public LetsmtTokenSampleStream(LetsmtDocument document)
            : this(document, DictionaryDetokenizer.LatinDetokenizer) {

        }

        /// <summary>
        /// Creates a new letsmt token sample stream with the specified document and detokenizer.
        /// </summary>
        /// <param name="document">The LetsMT! document.</param>
        /// <param name="detokenizer">The detokenizer.</param>
        /// <exception cref="ArgumentNullException"><paramref name="document"/> or <paramref name="detokenizer"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">The sample size must be greater than one.</exception>
        public LetsmtTokenSampleStream(LetsmtDocument document, IDetokenizer detokenizer) {
            this.document = document ?? throw new ArgumentNullException(nameof(document));
            this.detokenizer = detokenizer ?? throw new ArgumentNullException(nameof(detokenizer));
        }

        /// <inheritdoc />
        public TokenSample Read() {
            while (index < document.Sentences.Count) {
                if (index >= document.Sentences.Count)
                    return null;

                var sentence = document.Sentences[++index];
                if (sentence.Tokens.Count > 0)
                    return new TokenSample(detokenizer, sentence.Tokens.ToArray());

            }
            return null;
        }

        /// <inheritdoc />
        public void Reset() {
            index = 0;
        }
    }
}
