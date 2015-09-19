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