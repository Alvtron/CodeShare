using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CodeShare.Model
{
    public class SortedObservableCollection<T> : ObservableCollection<T> where T : IComparable
    {
        private readonly Func<T, IComparable> _keySelector;
        private readonly IComparer<IComparable> _comparer;
        public bool IsDescending { get; }
        public SortedObservableCollection()
        {
            _comparer = Comparer<IComparable>.Default;
        }
        public SortedObservableCollection(bool isDescending = false)
            : this()
        {
            IsDescending = isDescending;
        }
        public SortedObservableCollection(Func<T, IComparable> keySelector, bool isDescending = false)
            : this(isDescending)
        {
            _keySelector = keySelector;
        }

        protected override void InsertItem(int index, T item)
        {
            for (var i = 0; i < Count; i++)
            {
                switch (_keySelector != null
                    ? _comparer.Compare(_keySelector(this[i]), _keySelector(item))
                    : _comparer.Compare(this[i], item))
                {
                    case 0:
                    case 1:
                        if (!IsDescending)
                        {
                            base.InsertItem(i, item);
                            return;
                        }
                        else break;
                    case -1:
                        if (IsDescending)
                        {
                            base.InsertItem(i, item);
                            return;
                        }
                        else break;
                }
            }
            base.InsertItem(Count, item);
        }
    }
}
