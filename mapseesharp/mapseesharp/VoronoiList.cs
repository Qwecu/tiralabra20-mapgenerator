using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapseesharp
{
    public class VoronoiList<T>
    {

        public int Count => tempList.Count;

        private List<T> tempList;


        public VoronoiList()
        {
            tempList = new List<T>();
        }

        /// <summary>
        /// Indexer.
        /// </summary>
        /// <param name="i">index.</param>
        /// <returns>Element at index.</returns>
        public T this[int i]
        {
            get { return tempList[i]; }

            // set { tempList[i] = value; } not needed atm
        }

        /// <summary>
        /// Adds an element to the list.
        /// </summary>
        /// <param name="toAdd">Element to be added.</param>
        public void Add(T toAdd)
        {
            tempList.Add(toAdd);
        }

        /// <summary>
        /// Returns elements of type BeachArc.
        /// </summary>
        /// <returns>List of elements of type BeachArc.</returns>
        internal VoronoiList<BeachArc> GetElementsOfTypeBeachArc()
        {
            List<BeachArc> items = this.tempList.Where(x => x.GetType().Equals(typeof(BeachArc))).Select(x => x).Cast<BeachArc>().ToList();
            var newVoronoi = new VoronoiList<BeachArc>();
            newVoronoi.tempList = items;
            return newVoronoi;
        }

        /// <summary>
        /// Returns elements of type BeachHalfEdge.
        /// </summary>
        /// <returns>List of elements of type BeachHalfEdge.</returns>
        internal VoronoiList<BeachHalfEdge> GetElementsOfTypeBeachHalfEdge()
        {
            List<BeachHalfEdge> items = this.tempList.Where(x => x.GetType().Equals(typeof(BeachHalfEdge))).Select(x => x).Cast<BeachHalfEdge>().ToList();
            var newVoronoi = new VoronoiList<BeachHalfEdge>();
            newVoronoi.tempList = items;
            return newVoronoi;
        }

        public IEnumerator GetEnumerator()
        {
            return tempList.GetEnumerator();
        }

        /// <summary>
        /// Returns first index of item or -1 if not found.
        /// </summary>
        /// <param name="item">Item to be searched.</param>
        /// <returns>Index of first occurrence.</returns>
        internal int IndexOf(T item)
        {
            return tempList.IndexOf(item);
        }

        /// <summary>
        /// Removes the first occurrence of the item from the list.
        /// </summary>
        /// <param name="item">Item to be removed.</param>
        internal void Remove(T item)
        {
            tempList.Remove(item);
        }

        internal void InsertRange(int index, List<T> list)
        {
            tempList.InsertRange(index, list);
        }

        internal void InsertRange(int index, T[] array)
        {
            tempList.InsertRange(index, array);
        }

        internal void Insert(int index, T element)
        {
            tempList.Insert(index, element);
        }
    }
}