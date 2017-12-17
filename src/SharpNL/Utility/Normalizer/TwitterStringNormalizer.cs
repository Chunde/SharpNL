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

using System.Text.RegularExpressions;

namespace SharpNL.Utility.Normalizer {
    /// <summary>
    /// Normalizer for Twitter character sequences.
    /// </summary>
    /// <inheritdoc />
    public class TwitterStringNormalizer : IStringNormalizer {

        private static TwitterStringNormalizer instance;

        private static readonly Regex HashUserRegex = new Regex("[#@]\\S+", RegexOptions.Compiled);
        private static readonly Regex RtRegex = new Regex("\\b(rt[ :])+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex FaceRegex = new Regex("[:;x]-?[()dop]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex LaughRegex = new Regex("([hj])+([aieou])+(\\1+\\2+)+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Gets the static instance of <see cref="TwitterStringNormalizer"/>.
        /// </summary>
        public static TwitterStringNormalizer Instange => instance ?? (instance = new TwitterStringNormalizer());

        /// <inheritdoc />
        public string Normalize(string text) {
            if (string.IsNullOrEmpty(text))
                return text;

            text = HashUserRegex.Replace(text, " ");
            text = RtRegex.Replace(text, " ");
            text = FaceRegex.Replace(text, " ");
            text = LaughRegex.Replace(text, "$1$2$1$2");

            return text;
        }
    }
}
