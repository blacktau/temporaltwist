namespace TemporalTwist.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using TemporalTwist.Model;

    public class FormatEditorViewModel : ViewModelBase
    {
        private PresetViewModel currentPreset;

        private IList<Preset> originalFormats;

        public FormatEditorViewModel()
        {
            this.InitialiseFormats();

            this.NewCommand = new RelayCommand(this.AddNewFormat);
            this.SaveCommand = new RelayCommand(this.SaveSelectedItem, () => this.IsCurrentFormatEdited);
            this.CancelCommand = new RelayCommand(this.CancelEdit, () => this.IsCurrentFormatEdited);
        }

        public PresetViewModel SelectedItem
        {
            get
            {
                return this.currentPreset;
            }

            set
            {
                if (this.currentPreset == value)
                {
                    return;
                }

                this.currentPreset = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsCurrentFormatEdited => this.SelectedItem != null && this.SelectedItem.IsEdited();

        public ObservableCollection<PresetViewModel> Formats { get; set; }

        public ICommand NewCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        private static string GenerateUniqueFormatName(IEnumerable<PresetViewModel> formats, string baseName)
        {
            var name = baseName;
            var uniqueNameFound = false;
            var counter = 1;

            var formatViewModels = formats as IList<PresetViewModel> ?? formats.ToList();

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
            var formatViews = FormatList.Instance.Select(format => new PresetViewModel(format)).ToList();

            this.Formats = new ObservableCollection<PresetViewModel>(formatViews);
        }

        private void AddNewFormat()
        {
            var formatName = GenerateUniqueFormatName(this.Formats, "New Preset");
            var format = new Preset(formatName);
            this.originalFormats.Add(format);
            var viewModel = new PresetViewModel(format, true);
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