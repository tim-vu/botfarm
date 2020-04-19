using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using FORFarm.Application.Settings;
using FORFarm.Domain.Entities;

namespace FORFarm.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
            
            AddAdditionalMaps();
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes();
            var methodName = nameof(IMapFrom<object>.Mapping);
            var argumentTypes = new[] { typeof(Profile) };

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)).ToList();

                if (interfaces.Count == 0)
                    continue;

                var instance = Activator.CreateInstance(type);

                foreach (var interf in interfaces)
                {
                    var methodInfo = interf.GetMethod(methodName, argumentTypes);
                    methodInfo?.Invoke(instance, new object[] {this});
                }
            }
        }

        private void AddAdditionalMaps()
        {
            CreateMap<SettingsVm, FarmSettings>()
                .ForMember(d => d.MuleInterval, opt => opt.MapFrom(s => TimeSpan.FromMinutes(s.MuleIntervalMinutes)));
        }
    }


}
