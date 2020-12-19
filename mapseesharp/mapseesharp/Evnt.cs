namespace Mapseesharp
{
    using System;

    /// <summary>
    /// An event, either site or circle.
    /// </summary>
    public abstract class Evnt : IComparable
    {
        /// <summary>
        /// Gets a value indicating whether this is a site event.
        /// </summary>
        public bool IsSiteEvent
        {
            get { return this is EvntSite; }
        }

        /// <summary>
        /// Gets a value indicating the Y position of this.
        /// </summary>
        public abstract double YToHappen { get; }

        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is Evnt)
            {
                return this.CompareTo(obj as Evnt);
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Comparison.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>Value indicating result of comparison.</returns>
        public int CompareTo(Evnt obj)
        {
            return this.YToHappen.CompareTo(obj.YToHappen);
        }
    }
}