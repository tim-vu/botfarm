namespace FORFarm.Application.Common.Interfaces
{
    public interface IRandom
    {
        int Next(int inclusiveMin, int exclusiveMax);
    }
}