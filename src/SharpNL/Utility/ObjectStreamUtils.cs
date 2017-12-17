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
using System.Collections.Generic;

namespace SharpNL.Utility {
    /// <summary>
    /// Provides helper methods for object streams.
    /// </summary>
    public static class ObjectStreamUtils {

        /// <summary>
        /// Creates a <see cref="GenericObjectStream{T}"/> from a enumerable object.
        /// </summary>
        /// <typeparam name="T">The object stream type.</typeparam>
        /// <param name="enumerable">The enumerable object.</param>
        /// <returns>A new instance of <see cref="GenericObjectStream{T}"/>.</returns>
        public static IObjectStream<T> CreateObjectStream<T>(IEnumerable<T> enumerable) {
            return new GenericObjectStream<T>(enumerable);
        }


        /// <summary>
        /// Creates a concatenated object stream from multiple individual ObjectStreams with the same type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectStreams"></param>
        /// <returns>A new instance of <see cref="ConcatenatedObjectStream{T}"/> with the specified objects streams.</returns>
        public static IObjectStream<T> ConcatenateObjectStream<T>(params IObjectStream<T>[] objectStreams) {
            return new ConcatenatedObjectStream<T>(objectStreams);
        }

        /// <summary>
        /// Creates a concatenated object stream from multiple numerable objects.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the object stream.</typeparam>
        /// <param name="enumerables">The enumerable objects.</param>
        /// <returns>A new instance of <see cref="ConcatenatedObjectStream{T}"/> with the specified enumerables as streams.</returns>
        public static IObjectStream<T> ConcatenateObjectStream<T>(params IEnumerable<T>[] enumerables) {
            if (enumerables.Length == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(enumerables));

            var streams = new IObjectStream<T>[enumerables.Length];
            for (var i = 0; i < enumerables.Length; i++)
                streams[i] = new GenericObjectStream<T>(enumerables[i]);
            
            return  new ConcatenatedObjectStream<T>(streams);
        }
    }
}
