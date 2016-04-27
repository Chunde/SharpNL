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
using System.Text;

namespace SharpNL.Lemmatizer {
    /// <summary>
    ///     Represents an lemmatized sentence.
    /// </summary>
    public class LemmaSample {
        public LemmaSample(string[] tokens, string[] tags, string[] lemmas) {
            if (tokens == null)
                throw new ArgumentNullException("tokens");

            if (tags == null)
                throw new ArgumentNullException("tags");

            if (lemmas == null)
                throw new ArgumentNullException("tags");

            if (tokens.Length != tags.Length || tags.Length != lemmas.Length)
                throw new ArgumentException("All the arguments must have the same length.");

            Tokens = tokens;
            Tags = tags;
            Lemmas = lemmas;
        }

        /// <summary>
        ///     Gets the tokens of the sample.
        /// </summary>
        public string[] Tokens { get; private set; }

        /// <summary>
        ///     Gets the POS tags of the tokens.
        /// </summary>
        public string[] Tags { get; private set; }

        /// <summary>
        ///     Gets the lemmas in the sample
        /// </summary>
        public string[] Lemmas { get; private set; }


        public int Length {
            get { return Lemmas.Length; }
        }

        public override string ToString() {
            var sb = new StringBuilder();

            for (var i = 0; i < Lemmas.Length; i++) {
                sb.AppendFormat("{0} {1} {2}\n", Tokens[i], Tags[i], Lemmas[i]);
            }

            return sb.ToString();
        }

        protected bool Equals(LemmaSample other) {
            return Equals(Tokens, other.Tokens) && Equals(Tags, other.Tags) && Equals(Lemmas, other.Lemmas);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((LemmaSample) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = Tokens != null ? Tokens.GetHashCode() : 0;
                hashCode = (hashCode*397) ^ (Tags != null ? Tags.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Lemmas != null ? Lemmas.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}