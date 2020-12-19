namespace Mapseesharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VoronoiList<T>
    {

        public int Count { get; private set; }

        private T[] items;

        public int Capacity => this.items.Length;



        public VoronoiList(int initialCapacity = 20)
        {
            this.items = new T[initialCapacity];
            this.Count = 0;
        }

        /// <summary>
        /// Indexer.
        /// </summary>
        /// <param name="i">index.</param>
        /// <returns>Element at index.</returns>
        public T this[int i]
        {
            get
            {
                if (i < this.Count)
                {
                    return this.items[i];
                }
                else
                {
                    throw new IndexOutOfRangeException("Index must be less than Count");
                }
            }

            // set { tempList[i] = value; } not needed atm
        }

        /// <summary>
        /// Adds an element to the list.
        /// </summary>
        /// <param name="toAdd">Element to be added.</param>
        public void Add(T toAdd)
        {
            if (this.Count >= this.items.Length - 2)
            {
                this.DoubleCapacity();
            }

            this.items[this.Count] = toAdd;
            this.Count++;
        }

        private void DoubleCapacity()
        {
            T[] newlist = new T[2 * this.items.Length];
            for (int i = 0; i < this.items.Length; i++)
            {
                newlist[i] = this.items[i];
            }

            this.items = newlist;
        }

        /// <summary>
        /// Returns elements of type BeachArc.
        /// </summary>
        /// <returns>List of elements of type BeachArc.</returns>
        internal VoronoiList<BeachArc> GetAllElementsOfTypeBeachArc()
        {
            List<BeachArc> res = this.items.Where(x => x != null && x.GetType().Equals(typeof(BeachArc))).Select(x => x).Cast<BeachArc>().ToList();
            var newVoronoi = new VoronoiList<BeachArc>();
            newVoronoi.items = res.ToArray<BeachArc>();
            newVoronoi.Count = res.Count;
            return newVoronoi;
        }

        /// <summary>
        /// Returns elements of type BeachHalfEdge.
        /// </summary>
        /// <returns>List of elements of type BeachHalfEdge.</returns>
        internal VoronoiList<BeachHalfEdge> GetAllElementsOfTypeBeachHalfEdge()
        {
            List<BeachHalfEdge> res = this.items.Where(x => x != null && x.GetType().Equals(typeof(BeachHalfEdge))).Select(x => x).Cast<BeachHalfEdge>().ToList();
            var newVoronoi = new VoronoiList<BeachHalfEdge>();
            newVoronoi.items = res.ToArray<BeachHalfEdge>();
            newVoronoi.Count = res.Count;
            return newVoronoi;
        }

        /*public IEnumerator GetEnumerator()
        {
            return tempList.GetEnumerator();
        }*/



        /// <summary>
        /// Returns first index of item or -1 if not found.
        /// </summary>
        /// <param name="item">Item to be searched.</param>
        /// <returns>Index of first occurrence.</returns>
        public int IndexOf(T item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this.items[i].Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Removes the first occurrence of the item from the list.
        /// </summary>
        /// <param name="item">Item to be removed.</param>
        /// <returns>True if something was removed.</returns>
        public bool Remove(T item)
        {
            bool found = false;

            for (int i = 0; i < this.Count; i++)
            {
                if (found)
                {
                    this.items[i] = this.items[i + 1];
                }
                else if (this.items[i].Equals(item))
                {
                    this.items[i] = this.items[i + 1];
                    found = true;
                }
            }

            if (found)
            {
                this.Count--;
            }

            return found;
        }

        /// <summary>
        /// Adds a range of items to the list.
        /// </summary>
        /// <param name="index">Index where the items are added.</param>
        /// <param name="array">Array of items to add.</param>
        public void InsertRange(int index, T[] array)
        {
            T[] newlist = new T[this.Capacity + array.Length];
            for (int i = 0; i < this.Count + array.Length; i++)
            {
                if (i < index)
                {
                    newlist[i] = this.items[i];
                }
                else if (i < index + array.Length)
                {
                    newlist[i] = array[i - index];
                }
                else
                {
                    newlist[i] = this.items[i - array.Length];
                }
            }

            this.items = newlist;
            this.Count += array.Length;
        }

        /// <summary>
        /// Inserts an elemet to the list at given index.
        /// </summary>
        /// <param name="index">The given index.</param>
        /// <param name="element">Element to be added.</param>
        public void Insert(int index, T element)
        {
            if (this.Count >= this.items.Length - 2)
            {
                this.DoubleCapacity();
            }

            T addedNext = element;

            for (int i = index; i < this.Count + 1; i++)
            {
                T toadd = addedNext;
                addedNext = this.items[i];
                this.items[i] = toadd;
            }

            this.Count++;
        }

        /// <summary>
        /// Empties the list.
        /// </summary>
        internal void Clear()
        {
            this.items = new T[this.items.Length];
            this.Count = 0;
        }
    }
}