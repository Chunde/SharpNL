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
using System.IO;
using SharpNL.ML.Model;
using SharpNL.Utility;
using SharpNL.Utility.Model;

namespace SharpNL.LangDetect {

    /// <summary>
    /// A model for language detection
    /// </summary>
    /// <inheritdoc />
    public class LanguageDetectorModel : BaseModel {

        internal const string ComponentName = "LanguageDetectorME";
        internal const string ModelEntry = "langdetect.model";

        /// <summary>
        /// Creates a new instance of the language model with the specified model, entries and factory.
        /// </summary>
        /// <param name="langdetectModel">The language model.</param>
        /// <param name="manifestInfoEntries">The manifest info entries.</param>
        /// <param name="factory">The language detector factory.</param>
        /// <inheritdoc />
        public LanguageDetectorModel(
            IMaxentModel langdetectModel,
            Dictionary<string, string> manifestInfoEntries,
            LanguageDetectorFactory factory) : base(ComponentName, "und", manifestInfoEntries, factory) {
            
            artifactMap.Add(ModelEntry, langdetectModel);
            CheckArtifactMap();
        }

        /// <summary>
        /// Creates a new instance of the language model deserializing the input stream.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <inheritdoc />
        public LanguageDetectorModel(Stream inputStream) : base(ComponentName, inputStream) {
            
        }

        /// <inheritdoc />
        protected override void ValidateArtifactMap() {
            base.ValidateArtifactMap();

            if (artifactMap[ModelEntry] == null)
                throw new InvalidFormatException("Unable to find the model entry in the manifest.");

            if (!(artifactMap[ModelEntry] is AbstractModel)) 
                throw new InvalidFormatException("Invalid model type.");
            
        }

        /// <summary>
        /// Gets the language detector factory.
        /// </summary>
        public LanguageDetectorFactory Factory => (LanguageDetectorFactory)ToolFactory;

        /// <summary>
        /// Gets the maximum entropy model.
        /// </summary>
        internal IMaxentModel MaxentModel => (IMaxentModel)artifactMap[ModelEntry];

        /// <inheritdoc />
        protected override Type DefaultFactory => typeof(LanguageDetectorFactory);
    }

}

