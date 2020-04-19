using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace FORFarm.Application.Common.Mapping
{
    interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
