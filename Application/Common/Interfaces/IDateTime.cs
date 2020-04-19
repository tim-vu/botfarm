using System;
using System.Collections.Generic;
using System.Text;

namespace FORFarm.Application.Common.Interfaces
{
    public interface IDateTime
    {
        DateTime UtcNow { get; }
    }
}
