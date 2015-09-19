// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormatViewModel.cs" company="None">
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
//   wrapping view model for Format.
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
namespace TemporalTwist.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using TagLib;

    using TemporalTwist.Core;
    using Model;

    public class FormatViewModel : BaseViewModel
    {
        private readonly Format original;

        private IFormat format;

        private bool isNew;

        public FormatViewModel(Format format)
            : this(format, false)
        {
        }

        public FormatViewModel(Format format, bool isNew)
        {
            this.format = (IFormat)format.Clone();
            this.original = format;
            this.TagTypes = new ObservableCollection<TagTypes>(this.original.TagTypes);

            this.isNew = isNew;
        }

        public string Extension
        {
            get
            {
                return this.format.Extension;
            }

            set
            {
                if (this.format.Extension == value)
                {
                    return;
                }

                this.format.Extension = value;
                this.RaisePropertyChanged(nameof(this.Extension));
            }
        }

        public string CustomExtension
        {
            get
            {
                return this.format.CustomExtension;
            }

            set
            {
                if (this.format.CustomExtension == value)
                {
                    return;
                }

                this.format.CustomExtension = value;
                this.RaisePropertyChanged(nameof(this.CustomExtension));
            }
        }

        public int BitRate
        {
            get
            {
                return this.format.BitRate;
            }

            set
            {
                if (this.format.BitRate == value)
                {
                    return;
                }

                this.format.BitRate = value;
                this.RaisePropertyChanged(nameof(this.BitRate));
            }
        }

        public int SampleRate
        {
            get
            {
                return this.format.SampleRate;
            }

            set
            {
                this.format.SampleRate = value;
                this.RaisePropertyChanged(nameof(this.SampleRate));
            }
        }

        public string Name
        {
            get
            {
                return this.format.Name;
            }

            set
            {
                this.format.Name = value;
                this.RaisePropertyChanged(nameof(this.Name));
            }
        }

        public ObservableCollection<TagTypes> TagTypes { get; }

        public void CancelEdit()
        {
            this.format = (IFormat)this.original.Clone();
        }

        internal bool IsEdited()
        {
            var ret = !this.isNew && this.original.BitRate == this.BitRate
                      && this.original.CustomExtension.Equals(this.CustomExtension)
                      && this.original.Extension.Equals(this.Extension) && this.original.Name.Equals(this.Name)
                      && this.original.SampleRate == this.SampleRate
                      && !CheckIfTagTypesHaveChanged(this.original.TagTypes, this.TagTypes)
                      && !CheckIfTagTypesHaveChanged(this.TagTypes, this.original.TagTypes);
            return !ret;
        }

        internal void Save()
        {
            var originalName = this.original.Name;
            this.isNew = false;
            this.original.BitRate = this.BitRate;
            this.original.CustomExtension = this.CustomExtension;
            this.original.Extension = this.Extension;
            this.original.Name = this.Name;
            this.original.SampleRate = this.SampleRate;
            this.original.TagTypes.Clear();

            foreach (var tagType in this.TagTypes)
            {
                this.original.TagTypes.Add(tagType);
            }
        }

        private static bool CheckIfTagTypesHaveChanged(IEnumerable<TagTypes> source, ICollection<TagTypes> destination)
        {
            return source.Any(tagType => !destination.Contains(tagType));
        }
    }
}