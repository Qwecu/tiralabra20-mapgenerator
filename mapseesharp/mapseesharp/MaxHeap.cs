namespace Mapseesharp
{
    using System;

    /// <summary>
    /// Max heap.
    /// </summary>
    /// <typeparam name="T">Type of the elements in the heap.</typeparam>
    public class MaxHeap<T>
        where T : Evnt
    {
        /// <summary>
        /// Values of the heap.
        /// </summary>
        private Evnt[] heap;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxHeap{T}"/> class.
        /// </summary>
        public MaxHeap()
        {
            this.Count = 0;
            this.heap = new Evnt[400];
        }

        /// <summary>
        /// Gets count of elements in the heap.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Adds an element to the heap.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public void Add(Evnt element)
        {
            this.Test("Add begins " + element);
            this.Count++;
            this.heap[this.Count] = element;

            int current = this.Count;
            while (this.ParentIndex(current) > 0 && (this.heap[current].YToHappen > this.heap[this.ParentIndex(current)].YToHappen))
            {
                this.Swap(current, this.ParentIndex(current));
                current = this.ParentIndex(current);
            }

            if (this.Count > this.heap.Length - 2)
            {
                var newheap = new Evnt[this.heap.Length * 2];
                for (int i = 0; i < this.heap.Length; i++)
                {
                    newheap[i] = this.heap[i];
                }

                this.heap = newheap;
            }

            this.Test("Add " + element);
        }

        /// <summary>
        /// Returns the max element and removes it from the heap.
        /// </summary>
        /// <returns>The max element.</returns>
        public Evnt PopMax()
        {
            Evnt popped = this.heap[1];
            this.Test("Pop begins " + popped);
            this.heap[1] = this.heap[this.Count];
            this.heap[this.Count] = null;

            this.Count--;
            this.MaxHeapify(1);

            this.Test("Pop " + popped);

            return popped;
        }

        /// <summary>
        /// Returns the max element without removing it.
        /// </summary>
        /// <returns>The max element.</returns>
        public Evnt PeakMax()
        {
            Evnt peaked = this.heap[1];
            return peaked;
        }

        /// <summary>
        /// Returns the whole heap as an array.
        /// </summary>
        /// <returns>The array.</returns>
        public Evnt[] GetAllAsArray()
        {
            Evnt[] res = new Evnt[this.Count];
            for (int i = 0; i < this.Count; i++)
            {
                res[i] = this.heap[i + 1];
            }

            return res;
        }

        /// <summary>
        /// Tests that the heap rules are followed.
        /// </summary>
        /// <param name="operation">Operation from which the method is called.</param>
        public void Test(string operation)
        {
#if DEBUG
            for (int i = 1; i < this.Count + 1; i++)
            {
                if (!this.HasRightChild(i) && !this.HasLeftChild(i))
                {
                    return;
                }

                if (this.HasLeftChild(i) && this.heap[i].YToHappen < this.LeftChild(i).YToHappen)
                {
                    throw new Exception("VIOLATION of heap rules: " + operation);
                }

                if (this.HasRightChild(i) && this.heap[i].YToHappen < this.RightChild(i).YToHappen)
                {
                    throw new Exception("VIOLATION of heap rules: " + operation);
                }
            }
#endif
        }

        private int ParentIndex(int pos)
        {
            return pos / 2;
        }

        private int LeftChildIndex(int pos)
        {
            return 2 * pos;
        }

        private int RightChildIndex(int pos)
        {
            return (2 * pos) + 1;
        }

        private Evnt LeftChild(int pos)
        {
            return this.heap[2 * pos];
        }

        private Evnt RightChild(int pos)
        {
            return this.heap[(2 * pos) + 1];
        }

        private bool IsLeaf(int pos)
        {
            if (pos > (this.Count / 2) && pos <= this.Count)
            {
                return true;
            }

            return false;
        }

        private bool HasLeftChild(int pos)
        {
            return 2 * pos <= this.Count;
        }

        private bool HasRightChild(int pos)
        {
            return (2 * pos) + 1 <= this.Count;
        }

        private void Swap(int fpos, int spos)
        {
            Evnt tmp;
            tmp = this.heap[fpos];
            this.heap[fpos] = this.heap[spos];
            this.heap[spos] = tmp;
        }

        private void MaxHeapify(int pos)
        {
            if (this.IsLeaf(pos))
            {
                return;
            }

            if (
                (this.HasLeftChild(pos) && this.heap[pos].YToHappen < this.heap[this.LeftChildIndex(pos)].YToHappen)
                ||
               (this.HasRightChild(pos) && this.heap[pos].YToHappen < this.heap[this.RightChildIndex(pos)].YToHappen))
            {
                if (this.heap[this.LeftChildIndex(pos)].YToHappen > (this.heap[this.RightChildIndex(pos)]?.YToHappen ?? double.MinValue))
                {
                    this.Swap(pos, this.LeftChildIndex(pos));
                    this.MaxHeapify(this.LeftChildIndex(pos));
                }
                else
                {
                    this.Swap(pos, this.RightChildIndex(pos));
                    this.MaxHeapify(this.RightChildIndex(pos));
                }
            }
        }
    }
}
