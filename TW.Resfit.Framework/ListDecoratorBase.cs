// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListDecoratorBase.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the ListDecoratorBase type, which keeps all of the noise of the delegation to IList out of the ResourceList class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Framework
{
    using System.Collections;
    using System.Collections.Generic;

    public abstract class ListDecoratorBase<T> : IList<T>
    {
        protected readonly List<T> Items = new List<T>();

        public int Count
        {
            get
            {
                return this.Items.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public T this[int index]
        {
            get
            {
                return this.Items[index];
            }

            set
            {
                this.Items[index] = value;
            }
        }

        public void Add(T item)
        {
            this.Items.Add(item);
        }

        public void Clear()
        {
            this.Items.Clear();
        }

        public bool Contains(T item)
        {
            return this.Items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.Items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return this.Items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.Items.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return this.Items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.Items.RemoveAt(index);
        }
    }
}
