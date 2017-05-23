// 
//  Copyright 2014 Gustavo J Knuppe (https://github.com/knuppe)
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
using System.IO;
using System.Reflection;
using System.Text;

namespace SharpNL.Tests {
    internal static class Tests {
        internal static string ResourcesPath = @"../../../../resources/";

        public static FileStream OpenFile(string fileName) {
            var path = GetFullPath(fileName);

            if (!File.Exists(path)) {
                throw new FileNotFoundException("File not found :(", path);
            }

            return new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        public static StreamReader OpenFile(string fileName, Encoding encoding) {
            return new StreamReader(OpenFile(fileName), encoding);
        }

        public static string GetFullPath(string fileName) {
            var codeBase = new Uri(typeof (Tests).Assembly.CodeBase);

            var path = Path.GetDirectoryName(codeBase.LocalPath);

            if (path == null)
                throw new FileNotFoundException();

            path = Path.Combine(path, ResourcesPath, fileName.TrimStart('\\', '/'));
            return Path.GetFullPath(path);
        }

        public static string GetCodeBasePath() {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}