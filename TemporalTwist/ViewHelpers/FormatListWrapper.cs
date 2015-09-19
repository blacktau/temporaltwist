// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormatListWrapper.cs" company="None">
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
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
namespace TemporalTwist.ViewHelpers
{
    using System.Collections;
    using System.Collections.Generic;

    using Model;

    internal class FormatListWrapper : IList<Format>
    {
        public int Count => FormatList.Instance.Count;

        public bool IsReadOnly => false;

        public Format this[int index]
        {
            get
            {
                return FormatList.Instance[index];
            }

            set
            {
                FormatList.Instance[index] = value;
            }
        }

        IEnumerator<Format> IEnumerable<Format>.GetEnumerator()
        {
            return FormatList.Instance.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return FormatList.Instance.GetEnumerator();
        }

        public void Add(Format item)
        {
            FormatList.Instance.Add(item);
        }

        public void Clear()
        {
            FormatList.Instance.Clear();
        }

        public bool Contains(Format item)
        {
            return FormatList.Instance.Contains(item);
        }

        public void CopyTo(Format[] array, int arrayIndex)
        {
            FormatList.Instance.CopyTo(array, arrayIndex);
        }

        public bool Remove(Format item)
        {
            return FormatList.Instance.Remove(item);
        }

        public int IndexOf(Format item)
        {
            return FormatList.Instance.IndexOf(item);
        }

        public void Insert(int index, Format item)
        {
            FormatList.Instance.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            FormatList.Instance.RemoveAt(index);
        }
    }
}