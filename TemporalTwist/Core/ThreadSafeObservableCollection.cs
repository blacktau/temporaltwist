// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreadSafeObservableCollection.cs" company="None">
//   Copyright (c) 2009, Sean Garrett
//   All rights reserved.
//   Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
//   following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and 
//      the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * The names of the contributors may not be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//   INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//   DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//   SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//   SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//   WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
//   USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TemporalTwist.Core
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading;

    public class ThreadSafeObservableCollection<T> : ObservableCollection<T>
    {
        private readonly SynchronizationContext synchronizationContext;

        public ThreadSafeObservableCollection()
        {
            this.synchronizationContext = SynchronizationContext.Current;

            if (this.synchronizationContext == null)
            {
                throw new InvalidOperationException("This collection must be instantiated from the UI thread.");
            }
        }

        protected override void InsertItem(int index, T item)
        {
            this.synchronizationContext.Send(this.InsertItem, new InsertItemParameter(index, item));
        }

        private void InsertItem(object parameter)
        {
            var insertItemParameter = parameter as InsertItemParameter;
            if (insertItemParameter != null)
            {
                base.InsertItem(insertItemParameter.Index, insertItemParameter.Item);
            }
        }

        private sealed class InsertItemParameter
        {
            public InsertItemParameter(int index, T item)
            {
                this.Index = index;
                this.Item = item;
            }

            public int Index { get; }

            public T Item { get; }
        }
    }
}