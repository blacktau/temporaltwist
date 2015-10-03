namespace TemporalTwist.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using GalaSoft.MvvmLight;

    using TagLib;

    using TemporalTwist.Model;

    public class PresetViewModel : ViewModelBase
    {
        private readonly Preset original;

        private bool isNew;

        private IPreset preset;

        public PresetViewModel(Preset preset)
            : this(preset, false)
        {
        }

        public PresetViewModel(Preset preset, bool isNew)
        {
            this.preset = (IPreset)preset.Clone();
            this.original = preset;
            this.TagTypes = new ObservableCollection<TagTypes>(this.original.TagTypes);

            this.isNew = isNew;
        }

        public string Extension
        {
            get
            {
                return this.preset.Extension;
            }

            set
            {
                if (this.preset.Extension == value)
                {
                    return;
                }

                this.preset.Extension = value;
                this.RaisePropertyChanged();
            }
        }

        public string CustomExtension
        {
            get
            {
                return this.preset.CustomExtension;
            }

            set
            {
                if (this.preset.CustomExtension == value)
                {
                    return;
                }

                this.preset.CustomExtension = value;
                this.RaisePropertyChanged();
            }
        }

        public int BitRate
        {
            get
            {
                return this.preset.BitRate;
            }

            set
            {
                if (this.preset.BitRate == value)
                {
                    return;
                }

                this.preset.BitRate = value;
                this.RaisePropertyChanged();
            }
        }

        public int SampleRate
        {
            get
            {
                return this.preset.SampleRate;
            }

            set
            {
                this.preset.SampleRate = value;
                this.RaisePropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return this.preset.Name;
            }

            set
            {
                this.preset.Name = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<TagTypes> TagTypes { get; }

        public void CancelEdit()
        {
            this.preset = (IPreset)this.original.Clone();
        }

        internal bool IsEdited()
        {
            var ret = !this.isNew && this.original.BitRate == this.BitRate && this.original.CustomExtension.Equals(this.CustomExtension) && this.original.Extension.Equals(this.Extension) && this.original.Name.Equals(this.Name)
                      && this.original.SampleRate == this.SampleRate && !CheckIfTagTypesHaveChanged(this.original.TagTypes, this.TagTypes) && !CheckIfTagTypesHaveChanged(this.TagTypes, this.original.TagTypes);
            return !ret;
        }

        internal void Save()
        {
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