namespace Discovery.Generator
{
    public interface IIntGenerator : IDoubleGenerator
    {
        int NextInt();
        int NextInt(int min, int max);
    }
}