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
using System.Collections.Generic;
using SharpNL.Utility;

namespace SharpNL.Lemmatizer {
    /// <summary>
    /// Reads data for training and testing. The format consists of: word[tab]postag[tab]lemma.
    /// </summary>
    public class LemmaSampleStream : FilterObjectStream<string, LemmaSample> {

        public LemmaSampleStream(IObjectStream<string> samples) : base(samples) {

        }

        public override LemmaSample Read() {

            var toks = new List<string>();
            var tags = new List<string>();
            var preds = new List<string>();

            for (var line = Samples.Read(); !string.IsNullOrEmpty(line); line = Samples.Read()) {

                var parts = line.Split('\t');
                if (parts.Length != 3)
                    continue; // skip corrupt line
                
                toks.Add(parts[0]);
                tags.Add(parts[1]);
                preds.Add(LemmatizerUtils.GetShortestEditScript(parts[0], parts[2]));                
            }

            return toks.Count > 0 ? new LemmaSample(toks.ToArray(), tags.ToArray(), preds.ToArray()) : null;
        }
    }
}