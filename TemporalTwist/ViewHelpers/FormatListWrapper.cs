namespace TemporalTwist.ViewHelpers
{
    using System.Collections;
    using System.Collections.Generic;

    using Model;

    internal class FormatListWrapper : IList<Preset>
    {
        public int Count => FormatList.Instance.Count;

        public bool IsReadOnly => false;

        public Preset this[int index]
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

        IEnumerator<Preset> IEnumerable<Preset>.GetEnumerator()
        {
            return FormatList.Instance.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return FormatList.Instance.GetEnumerator();
        }

        public void Add(Preset item)
        {
            FormatList.Instance.Add(item);
        }

        public void Clear()
        {
            FormatList.Instance.Clear();
        }

        public bool Contains(Preset item)
        {
            return FormatList.Instance.Contains(item);
        }

        public void CopyTo(Preset[] array, int arrayIndex)
        {
            FormatList.Instance.CopyTo(array, arrayIndex);
        }

        public bool Remove(Preset item)
        {
            return FormatList.Instance.Remove(item);
        }

        public int IndexOf(Preset item)
        {
            return FormatList.Instance.IndexOf(item);
        }

        public void Insert(int index, Preset item)
        {
            FormatList.Instance.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            FormatList.Instance.RemoveAt(index);
        }
    }
}