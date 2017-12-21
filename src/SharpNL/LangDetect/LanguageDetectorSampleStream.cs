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

using SharpNL.Utility;

namespace SharpNL.LangDetect {
    /// <summary>
    /// This class reads in string encoded training samples, parses them and outputs <see cref="LanguageSample"/>.
    /// </summary>
    /// <remarks>
    /// <para>Format:</para>
    /// <para>Each line contains one sample document.</para>
    /// <para>The language is the first string in the line followed by a tab and the document content.</para>
    /// <para>Sample line: category-string tab-char document line-break-char(s)</para>
    /// </remarks>
    /// <inheritdoc />
    public class LanguageDetectorSampleStream : FilterObjectStream<string, LanguageSample> { 
        /// <inheritdoc />
        public LanguageDetectorSampleStream(IObjectStream<string> samples) : base(samples) {

        }

        /// <inheritdoc />
        public override LanguageSample Read() {
            string sampleString;
            while ((sampleString = Samples.Read()) != null) {

                var tabIndex = sampleString.IndexOf('\t');
                if (tabIndex == -1) continue;

                var lang = sampleString.Substring(0, tabIndex);
                var context = sampleString.Substring(tabIndex + 1);

                return new LanguageSample(new Language(lang), context);
            }
            return null;
        }        
    }
}
