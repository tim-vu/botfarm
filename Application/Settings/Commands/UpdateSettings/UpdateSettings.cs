using System;
using System.Collections.Generic;
using System.Text;
using FORFarm.Domain.Entities;
using MediatR;

namespace FORFarm.Application.Settings.Commands.UpdateSettings
{
    public class UpdateSettings : IRequest
    {
        public SettingsVm Settings { get; }

        public UpdateSettings(SettingsVm settings)
        {
            Settings = settings;
        }
    }
}