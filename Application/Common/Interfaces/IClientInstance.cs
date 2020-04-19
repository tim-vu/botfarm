using System;
using System.Collections.Generic;
using System.Text;

namespace FORFarm.Application.Common.Interfaces
{
    public interface IClientInstance
    {
        string Username { get; }

        string ScriptName { get; }

        Guid Tag { get; }
    }
}
