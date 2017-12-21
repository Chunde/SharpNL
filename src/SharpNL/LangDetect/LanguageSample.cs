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

namespace SharpNL.LangDetect {

    /// <summary>
    /// Class which holds a classified document and its <see cref="SharpNL.LangDetect.Language" />
    /// </summary>
    /// <inheritdoc />
    [Serializable]
    public sealed class LanguageSample : IEquatable<LanguageSample> {
        /// <summary>
        /// Gets the sample language object.
        /// </summary>
        public Language Language { get; }

        /// <summary>
        /// Gets the sample context.
        /// </summary>
        public string Context { get; }

        /// <summary>
        /// Creates a new language sample with the specified language and context;
        /// </summary>
        /// <param name="language"></param>
        /// <param name="context"></param>
        public LanguageSample(Language language, string context) {
            Language = language ?? throw new ArgumentNullException(nameof(language));
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{Language.Lang}\t{Context}";
        }

        /// <inheritdoc />
        public bool Equals(LanguageSample other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Language, other.Language) && string.Equals(Context, other.Context);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is LanguageSample sample && Equals(sample);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            unchecked {
                return ((Language != null ? Language.GetHashCode() : 0) * 397) ^ (Context != null ? Context.GetHashCode() : 0);
            }
        }
    }
}
