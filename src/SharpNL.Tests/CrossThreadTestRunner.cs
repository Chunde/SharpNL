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
using System.Reflection;
using System.Security.Permissions;
using System.Threading;

namespace SharpNL.Tests {

    /// <seealso href="http://blog.jerometerry.com/2014/06/multi-threaded-nunit-tests.html"/>
    /// <remarks>
    /// Created by: Jerome Terry 
    /// </remarks>
    internal class CrossThreadTestRunner {
        private const string RemoteStackTraceFieldName = "_remoteStackTraceString";
        private static readonly FieldInfo RemoteStackTraceField = typeof (Exception).GetField(RemoteStackTraceFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        private readonly ThreadStart start;
        private readonly Thread thread;
        private Exception lastException;

        public CrossThreadTestRunner(ThreadStart start) {
            this.start = start;
            thread = new Thread(Run);
            thread.SetApartmentState(ApartmentState.STA);
        }

        public void Start() {
            lastException = null;
            thread.Start();
        }

        public void Join() {
            thread.Join();

            if (lastException != null) {
                ThrowExceptionPreservingStack(lastException);
            }
        }

        private void Run() {
            try {
                start.Invoke();
            } catch (Exception e) {
                lastException = e;
            }
        }

        [ReflectionPermission(SecurityAction.Demand)]
        private static void ThrowExceptionPreservingStack(Exception exception) {
            if (RemoteStackTraceField != null) {
                RemoteStackTraceField.SetValue(exception, exception.StackTrace + Environment.NewLine);
            }
            throw exception;
        }
    }
}