namespace Projections.Core.Models
{
    public class BoundedValue<T>
    {
        public T Upper { get; }
        public T Lower { get; }

        public BoundedValue(T lower, T upper)
        {
            Upper = upper;
            Lower = lower;
        }
    }
}