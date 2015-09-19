// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormatEditorViewModel.cs" company="None">
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
//   The View Model for the Format Editor
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
namespace TemporalTwist.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using TemporalTwist.Core;
    using Model;

    public class FormatEditorViewModel : BaseViewModel
    {
        private IList<Format> originalFormats;

        private FormatViewModel currentFormat;

        public FormatEditorViewModel()
        {
            this.InitialiseFormats();

            this.NewCommand = new RelayCommand(this.AddNewFormat);
            this.SaveCommand = new RelayCommand(this.SaveSelectedItem, () => this.IsCurrentFormatEdited);
            this.CancelCommand = new RelayCommand(this.CancelEdit, () => this.IsCurrentFormatEdited);
        }


        public FormatViewModel SelectedItem
        {
            get
            {
                return this.currentFormat;
            }

            set
            {
                if (this.currentFormat == value)
                {
                    return;
                }

                this.currentFormat = value;
                this.RaisePropertyChanged(nameof(this.SelectedItem));
            }
        }

        public bool IsCurrentFormatEdited => this.SelectedItem != null && this.SelectedItem.IsEdited();

        public ObservableCollection<FormatViewModel> Formats { get; set; }

        public ICommand NewCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }
        
        private static string GenerateUniqueFormatName(IEnumerable<FormatViewModel> formats, string baseName)
        {
            var name = baseName;
            var uniqueNameFound = false;
            var counter = 1;

            var formatViewModels = formats as IList<FormatViewModel> ?? formats.ToList();

            while (!uniqueNameFound)
            {
                uniqueNameFound = formatViewModels.All(format => !format.Name.Equals(name));

                if (!uniqueNameFound)
                {
                    name = string.Format("{0} ({1})", baseName, counter);
                    counter++;
                }
            }

            return name;
        }
        
        private void InitialiseFormats()
        {
            this.originalFormats = FormatList.Instance;
            var formatViews = FormatList.Instance.Select(format => new FormatViewModel(format)).ToList();

            this.Formats = new ObservableCollection<FormatViewModel>(formatViews);
        }

        private void AddNewFormat()
        {
            var formatName = GenerateUniqueFormatName(this.Formats, "New Format");
            var format = new Format(formatName);
            this.originalFormats.Add(format);
            var viewModel = new FormatViewModel(format, true);
            this.Formats.Add(viewModel);
        }

        private void CancelEdit()
        {
            this.SelectedItem?.CancelEdit();
        }

        private void SaveSelectedItem()
        {
            this.SelectedItem?.Save();
        }
    }
}