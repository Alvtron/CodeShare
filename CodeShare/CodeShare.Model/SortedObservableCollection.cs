// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="SortedObservableCollection.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CodeShare.Model
{
    /// <summary>
    /// Class SortedObservableCollection.
    /// Implements the <see cref="System.Collections.ObjectModel.ObservableCollection{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.ObjectModel.ObservableCollection{T}" />
    public class SortedObservableCollection<T> : ObservableCollection<T> where T : IComparable
    {
        /// <summary>
        /// The key selector
        /// </summary>
        private readonly Func<T, IComparable> _keySelector;
        /// <summary>
        /// The comparer
        /// </summary>
        private readonly IComparer<IComparable> _comparer;
        /// <summary>
        /// Gets a value indicating whether this instance is descending.
        /// </summary>
        /// <value><c>true</c> if this instance is descending; otherwise, <c>false</c>.</value>
        public bool IsDescending { get; }
        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableCollection{T}"/> class.
        /// </summary>
        public SortedObservableCollection()
        {
            _comparer = Comparer<IComparable>.Default;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
        public SortedObservableCollection(bool isDescending = false)
            : this()
        {
            IsDescending = isDescending;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
        public SortedObservableCollection(Func<T, IComparable> keySelector, bool isDescending = false)
            : this(isDescending)
        {
            _keySelector = keySelector;
        }

        /// <summary>
        /// Inserts the item.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
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
