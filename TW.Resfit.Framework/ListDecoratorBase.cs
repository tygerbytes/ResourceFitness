// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListDecoratorBase.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
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

        public virtual int Count
        {
            get
            {
                return this.Items.Count;
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public virtual T this[int index]
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

        public virtual void Add(T item)
        {
            this.Items.Add(item);
        }

        public virtual void Clear()
        {
            this.Items.Clear();
        }

        public virtual bool Contains(T item)
        {
            return this.Items.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            this.Items.CopyTo(array, arrayIndex);
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        public virtual int IndexOf(T item)
        {
            return this.Items.IndexOf(item);
        }

        public virtual void Insert(int index, T item)
        {
            this.Items.Insert(index, item);
        }

        public virtual bool Remove(T item)
        {
            return this.Items.Remove(item);
        }

        public virtual void RemoveAt(int index)
        {
            this.Items.RemoveAt(index);
        }
    }
}
