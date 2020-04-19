using System.Collections.Generic;
using FORFarm.Application.Accounts.Queries;
using FORFarm.Domain.Entities;
using FORFarm.Domain.Enums;
using MediatR;

namespace FORFarm.Application.Accounts.Commands.UpdateGameDetails
{
    public class UpdateGameDetails : IRequest
    {
        public int ID { get; set; }

        public int MembershipDaysRemaining { get; set; }

        public IReadOnlyDictionary<SkillType, int> Skills { get; set; }

    }
}
