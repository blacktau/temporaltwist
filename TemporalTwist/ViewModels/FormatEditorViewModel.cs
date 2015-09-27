namespace TemporalTwist.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.CommandWpf;

    using Core;
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
                this.RaisePropertyChanged();
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
                    name = $"{baseName} ({counter})";
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