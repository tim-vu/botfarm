using System;
using System.Collections.Generic;
using System.Text;

namespace FORFarm.Application.Common.Interfaces
{
    public interface ILauncher
    {
        string Hostname { get; }

        Guid SocketAddress { get; }
    }
}
