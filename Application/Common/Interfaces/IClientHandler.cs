using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FORFarm.Application.Common.Models;
using FORFarm.Application.Common.Models.Farm;

namespace FORFarm.Application.Common.Interfaces
{
    public interface IClientHandler
    {
        Task<IEnumerable<ILauncher>> GetLaunchers(string apiKey);

        Task<IEnumerable<IClientInstance>> GetLaunchedClients(string apiKey);

        Task<bool> StartClient(Guid socketAddress, ClientStartArgs clientStartArgs, string apiKey);

        Task<bool> KillClient(Guid clientTag, string apiKey);
    }
}
