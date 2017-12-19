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
using System.Text;

namespace SharpNL.Formats.LetsMT {

    using SentenceDetector;
    using Tokenize;
    using Utility;
    /// <summary>
    /// Privide a stream of sentence samples of LetsMT! 
    /// </summary>
    /// /// <inheritdoc cref="Disposable" />
    public sealed class LetsmtSentenceStream : Disposable, IObjectStream<SentenceSample> {
        private readonly IDetokenizer detokenizer;
        private readonly LetsmtDocument document;
        private readonly int sampleSize;

        private int index;


        /// <summary>
        /// Creates a new LetsMT! sentence stream with the specified document using the <see cref="F:SharpNL.Tokenize.DictionaryDetokenizer.LatinDetokenizer" /> and sample size of 25.
        /// </summary>
        /// <param name="document">The LetsMT! document</param>
        /// <inheritdoc />
        public LetsmtSentenceStream(LetsmtDocument document) 
            : this(document, DictionaryDetokenizer.LatinDetokenizer, 25) {

        }

        /// <summary>
        /// Creates a new letsmt sentence stream with the specified document and detokenizer.
        /// </summary>
        /// <param name="document">The LetsMT! document.</param>
        /// <param name="detokenizer">The detokenizer.</param>
        /// <param name="sampleSize">The sample size.</param>
        /// <exception cref="ArgumentNullException"><paramref name="document"/> or <paramref name="detokenizer"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">The sample size must be greater than one.</exception>
        public LetsmtSentenceStream(LetsmtDocument document, IDetokenizer detokenizer, int sampleSize) {
            this.document = document ?? throw new ArgumentNullException(nameof(document));
            this.detokenizer = detokenizer ?? throw new ArgumentNullException(nameof(detokenizer));

            if (sampleSize < 1)
                throw new ArgumentOutOfRangeException(nameof(sampleSize), "The sample size must be greater than one.");

            this.sampleSize = sampleSize;
        }

        /// <inheritdoc />
        public SentenceSample Read() {
            var sb = new StringBuilder(512);
            var spans = new List<Span>(sampleSize);
            var count = 0;

            while (count <= sampleSize && index < document.Sentences.Count) {
                var sentence = document.Sentences[++index];
                var start = sb.Length;

                var text = sentence.Tokens.Count > 0
                    ? detokenizer.Detokenize(sentence.Tokens.ToArray(), null)
                    : sentence.NonTokenizedText;

                if (string.IsNullOrEmpty(text))
                    continue;

                sb.Append(text);
                spans.Add(new Span(start, sb.Length));

                sb.Append(" ");

                count++;
            }

            return count > 0
                ? new SentenceSample(sb.ToString(), spans.ToArray())
                : null;
        }
    
        /// <inheritdoc />
        public void Reset() {
            index = 0;
        }
    }
}
