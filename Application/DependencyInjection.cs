using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AutoMapper;
using FORFarm.Application.Common.Behaviors;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Farm;
using FORFarm.Application.Farm.FarmManager;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FORFarm.Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));
            services.AddScoped<IFarmService, FarmService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IFarmBuilder, FarmBuilder>();
            services.AddScoped<IInstanceService, InstanceService>();

            services.AddHostedService<FarmTaskExecutor>();

            return services;
        }

    }
}
