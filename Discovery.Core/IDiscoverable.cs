namespace Discovery.Core
{
    public interface IDiscoverable<out T>
    {
        T Generate();
        T Mutate(double? probability = 0.05);
    }
}
