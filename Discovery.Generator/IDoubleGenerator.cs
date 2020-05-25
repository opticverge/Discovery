namespace Discovery.Generator
{
    public interface IDoubleGenerator
    {
        double NextDouble();
        double NextDouble(double lowerBound, double upperBound);
    }
}
