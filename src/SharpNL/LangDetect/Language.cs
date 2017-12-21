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
using System.Globalization;

namespace SharpNL.LangDetect {

    /// <summary>
    /// Class for holding the document language and its confidence.
    /// </summary>
    /// <inheritdoc />
    [Serializable]
    public sealed class Language : IEquatable<Language> {

        /// <summary>
        /// Creates a new instance of language with the specified identifier.
        /// </summary>
        /// <param name="lang">The language identifier.</param>
        /// <inheritdoc />
        public Language(string lang) : this(lang, 0) {
            
        }

        /// <summary>
        /// Creates a new instance of language with the specified language identifier and confidence.
        /// </summary>
        /// <param name="lang">The language identifier.</param>
        /// <param name="confidence">The language confidence.</param>
        public Language(string lang, double confidence) {
            if (string.IsNullOrEmpty(lang))
                throw new ArgumentNullException(nameof(lang));

            Lang = lang;
            Confidence = confidence;
        }

        /// <summary>
        /// Gets the language identifier.
        /// </summary>
        public string Lang { get; }

        /// <summary>
        /// Gets the language confidence.
        /// </summary>
        public double Confidence { get; }

        /// <inheritdoc />
        public override string ToString() {
            return $"{Lang} ({Confidence.ToString(CultureInfo.InvariantCulture)})";
        }

        /// <inheritdoc />
        public bool Equals(Language other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Lang, other.Lang);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Language) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            unchecked {
                return ((Lang != null ? Lang.GetHashCode() : 0) * 397) ^ Confidence.GetHashCode();
            }
        }
    }
}
