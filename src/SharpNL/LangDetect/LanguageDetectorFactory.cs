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

namespace SharpNL.LangDetect {
    using Utility;
    using Utility.Normalizer;

    /// <summary>
    /// Default factory used by Language Detector. Extend this class to change the Language Detector 
    /// behaviour, such as the <see cref="DefaultLanguageDetectorContextGenerator"/>.
    /// </summary>
    /// <remarks>
    /// The default {@link DefaultLanguageDetectorContextGenerator} will use char n-grams of
    /// size 1 to 3 and the following normalizers: 
    /// <see cref="EmojiStringNormalizer"/>
    /// <see cref="UrlStringNormalizer"/>
    /// <see cref="TwitterStringNormalizer"/>
    /// <see cref="NumberStringNormalizer"/>
    /// <see cref="ShrinkStringNormalizer"/>
    /// </remarks>
    [TypeClass("opennlp.tools.langdetect.LanguageDetectorFactory")]
    public class LanguageDetectorFactory : BaseToolFactory {

        /// <summary>
        /// Gets the default language context generator.
        /// </summary>
        /// <returns>The default language context generator.</returns>
        public virtual ILanguageDetectorContextGenerator GetContextGenerator() {
            return new DefaultLanguageDetectorContextGenerator(1, 3, 
                new EmojiStringNormalizer(),
                new UrlStringNormalizer(),
                new TwitterStringNormalizer(), 
                new NumberStringNormalizer(),
                new ShrinkStringNormalizer());
        }

    }
}
