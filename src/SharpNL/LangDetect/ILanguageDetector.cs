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
    /// <summary>
    /// The interface for LanguageDetector which provide the <see cref="Language"/> according to the context.
    /// </summary>
    public interface ILanguageDetector {
        /// <summary>
        /// Predicts the languages for a string content.
        /// </summary>
        /// <param name="content">The content that should be analyzed.</param>
        /// <returns>An array with predicted languages.</returns>
        Language[] PredictLanguages(string content);

        /// <summary>
        /// Gets the best language prediction for a string content.
        /// </summary>
        /// <param name="content">The content that should be analyzed.</param>
        /// <returns>The best language prediction.</returns>
        Language PredictLanguage(string content);

        /// <summary>
        /// Gets an array of supported languages.
        /// </summary>
        /// <returns>An array of supported languages.</returns>
        string[] GetSupportedLanguages();
    }
}
