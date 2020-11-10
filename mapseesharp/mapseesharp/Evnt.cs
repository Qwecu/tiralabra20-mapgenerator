namespace mapseesharp
{
    public abstract class Evnt
    {
        public bool IsSiteEvent { get; set; }
        public abstract double YToHappen { get; }
    }
}