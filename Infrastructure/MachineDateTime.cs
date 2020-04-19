using System;
using FORFarm.Application.Common.Interfaces;

namespace Infrastructure
{
    public class MachineDateTime : IDateTime
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}