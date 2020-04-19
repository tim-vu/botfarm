using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FORFarm.Domain.Entities;

namespace FORFarm.Application.Common.Interfaces
{
    public interface IInstanceService
    {

        Expression<Func<Instance, bool>> IsConnected();
            
        Task<bool> LaunchInstance(Instance instance);
        
        
    }
}