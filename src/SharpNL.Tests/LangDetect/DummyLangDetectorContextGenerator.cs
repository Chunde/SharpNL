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
using SharpNL.LangDetect;
using SharpNL.NGram;
using SharpNL.Tokenize;
using SharpNL.Utility;
using SharpNL.Utility.Normalizer;

namespace SharpNL.Tests.LangDetect {

    public class DummyLangDetectorContextGenerator : DefaultLanguageDetectorContextGenerator {
        public DummyLangDetectorContextGenerator(int minLength, int maxLength, params IStringNormalizer[] normalizers) : base(minLength, maxLength, normalizers) {

        }

        public override string[] GetContext(string document) {
            var context = new List<string>(base.GetContext(document));

            document = Normalizer.Normalize(document);

            var words = SimpleTokenizer.Instance.Tokenize(document);
            var model = new NGramModel();
            if (words.Length > 0) {
                model.Add(new StringList(words), 1, 3);

                using (var en = model.GetEnumerator()) {
                    while (en.MoveNext()) {
                        if (en.Current?.Count > 0)
                            context.Add("tg=" + en.Current);
                    }
                }
                  

            }

            return context.ToArray();
        }
    }


}
