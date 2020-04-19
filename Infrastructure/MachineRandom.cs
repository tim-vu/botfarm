using System;
using FORFarm.Application.Common.Interfaces;

namespace Infrastructure
{
    public class MachineRandom : IRandom
    {
        private static readonly Random Random = new Random();
        
        public int Next(int inclusiveMin, int exclusiveMax)
        {
            return Random.Next(inclusiveMin, exclusiveMax);
        }
    }
}