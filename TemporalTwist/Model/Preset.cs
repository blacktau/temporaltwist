namespace TemporalTwist.Model
{
    using System.Collections.Generic;

    using TagLib;

    public class Preset : IPreset
    {
        private string customExtension;

        public Preset()
            : this("New Preset")
        {
        }

        public Preset(string name)
        {
            this.TagTypes = new List<TagTypes>(0);
            this.BitRate = 64000;
            this.SampleRate = 44100;
            this.Extension = "None";
            this.Name = name;
        }

        public string Extension { get; set; }

        public string CustomExtension
        {
            get
            {
                return string.IsNullOrEmpty(this.customExtension) ? this.Extension : this.customExtension;
            }

            set
            {
                this.customExtension = value;
            }
        }

        public int BitRate { get; set; }

        public int SampleRate { get; set; }

        public string Name { get; set; }

        public IList<TagTypes> TagTypes { get; set; }

        public bool Equals(IPreset other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Name.Equals( this.Name);
        }

        public object Clone()
        {
            var newFormat = (Preset)this.MemberwiseClone();
            newFormat.TagTypes = new List<TagTypes>();
            foreach (var tagType in this.TagTypes)
            {
                newFormat.TagTypes.Add(tagType);
            }

            return newFormat;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Preset);
        }

        public override int GetHashCode()
        {
            var name = this.Name;
            return name?.GetHashCode() ?? 0;
        }
    }
}