// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="ObservableStack.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CodeShare.Model
{
    /// <summary>
    /// Class ObservableStack.
    /// Implements the <see cref="System.Collections.Generic.Stack{T}" />
    /// Implements the <see cref="System.Collections.Specialized.INotifyCollectionChanged" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.Stack{T}" />
    /// <seealso cref="System.Collections.Specialized.INotifyCollectionChanged" />
    public class ObservableStack<T> : Stack<T>, INotifyCollectionChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableStack{T}"/> class.
        /// </summary>
        public ObservableStack()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableStack{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection to copy elements from.</param>
        public ObservableStack(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                base.Push(item);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableStack{T}"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        public ObservableStack(IReadOnlyCollection<T> list)
        {
            foreach (var item in list)
            {
                base.Push(item);
            }
        }

        /// <summary>
        /// Removes all objects from the <see cref="T:System.Collections.Generic.Stack`1"></see>.
        /// </summary>
        public new virtual void Clear()
        {
            base.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Removes and returns the object at the top of the <see cref="T:System.Collections.Generic.Stack`1"></see>.
        /// </summary>
        /// <returns>The object removed from the top of the <see cref="T:System.Collections.Generic.Stack`1"></see>.</returns>
        public new virtual T Pop()
        {
            var item = base.Pop();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, Count));
            return item;
        }

        /// <summary>
        /// Inserts an object at the top of the <see cref="T:System.Collections.Generic.Stack`1"></see>.
        /// </summary>
        /// <param name="item">The object to push onto the <see cref="T:System.Collections.Generic.Stack`1"></see>. The value can be null for reference types.</param>
        public new virtual void Push(T item)
        {
            base.Push(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        /// <returns></returns>
        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Handles the <see cref="E:CollectionChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            RaiseCollectionChanged(e);
        }

        /// <summary>
        /// Raises the collection changed.
        /// </summary>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }
    }
}
