// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobItem.cs" company="None">
//   Copyright (c) 2009, Sean Garrett
//   All rights reserved.
//
//   Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
//   following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and 
//      the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * The names of the contributors may not be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//   INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//   DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//   SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//   SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//   WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
//   USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <remarks>
//   Container for an item (or source file) to be processed
// </remarks>
// --------------------------------------------------------------------------------------------------------------------

namespace TemporalTwist.Model
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Core;




    internal class JobItem : ObservableObject, IEquatable<IJobItem>, IJobItem
    {



        public JobItem()
        {
            State = JobItemState.None;
            TemporaryFiles = new List<string>();
        }

        private string _sourceFile;

        private double _progress;

        private bool _isBeingProcessed;

        private JobItemState _state;





        public string DestinationFile { get; set; }





        public bool IsBeingProcessed
        {
            get
            {
                return _isBeingProcessed;
            }

            set
            {
                if (_isBeingProcessed == value)
                {
                    return;
                }

                _isBeingProcessed = value;
                RaisePropertyChanged("IsBeingProcessed");
            }
        }





        public string LastFile
        {
            get
            {
                return TemporaryFiles[TemporaryFiles.Count - 1];
            }
        }





        public double Progress
        {
            get
            {
                return _progress;
            }

            set
            {
                if (_progress == value)
                {
                    return;
                }

                _progress = value;
                RaisePropertyChanged("Progress");
            }
        }





        public JobItemState State
        {
            get
            {
                return _state;
            }

            set
            {
                if (_state == value)
                {
                    return;
                }

                _state = value;
                RaisePropertyChanged("State");
            }
        }





        public string SourceFile
        {
            get
            {
                return _sourceFile;
            }

            set
            {
                if (_sourceFile != value)
                {
                    _sourceFile = value;
                    RaisePropertyChanged("SourceFile");
                }
            }
        }





        public List<string> TemporaryFiles { get; private set; }








        public bool Equals(IJobItem other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.SourceFile, SourceFile);
        }










        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var inter = obj.GetType().GetInterface(typeof(IJobItem).Name);
            if (inter == null)
            {
                return false;
            }

            return Equals((IJobItem)obj);
        }








        public override int GetHashCode()
        {
            return SourceFile != null ? SourceFile.GetHashCode() : 0;
        }




        public void Reset()
        {
            State = JobItemState.None;
            Progress = 0;
            foreach (var file in TemporaryFiles)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }
    }
}
