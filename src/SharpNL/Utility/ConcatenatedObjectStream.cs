using System;
using System.Collections.Generic;

namespace SharpNL.Utility {
    /// <inheritdoc cref="Disposable" />
    public class ConcatenatedObjectStream<T> : Disposable, IObjectStream<T> {

        private int index;
        private readonly IObjectStream<T>[] streams;

        /// <summary>
        /// Create a new instance using the specified stram objects.
        /// </summary>
        /// <param name="streams">The streams to be merged.</param>
        public ConcatenatedObjectStream(IObjectStream<T>[] streams) {
            if (streams.Length == 0)
                throw new ArgumentOutOfRangeException("streams");

            this.streams = streams;
        }

        /// <inheritdoc />
        public T Read() {
            while (index < streams.Length) {
                var read = streams[index].Read();
                if (!EqualityComparer<T>.Default.Equals(default(T), read))
                    return read;

                index++;
            }

            return default(T);
        }

        /// <inheritdoc />
        public void Reset() {
            foreach (var objectStream in streams) {
                objectStream.Reset();
            }
            index = 0;
        }
    }
}
