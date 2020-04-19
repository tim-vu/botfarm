using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Bogus;
using FORFarm.Domain.Entities;
using FORFarm.Domain.Enums;
using FORFarm.Domain.ValueObjects;

namespace Application.UnitTests.Common
{
    public class BogusData
    {
        public static Faker<Account> CreateFaker(bool mule, bool member, bool banned)
        {
            return new Faker<Account>().Rules((f, o) =>
            {
                o.Username = f.Internet.UserName();
                o.Password = f.Internet.Password();
                o.Mule = mule;
                o.MemberExpirationDate = member ? DateTime.UtcNow.AddDays(5) : DateTime.MinValue;
                o.Banned = banned;
                o.Skills = GenerateRandomSkills();
            });
        }

        public static readonly Faker<Account> MixedAccounts = new Faker<Account>().Rules((f, o) =>
            {
                o.Username = f.Internet.UserName();
                o.Password = f.Internet.Password();
                o.Mule = f.Random.Bool(0.2f);
                o.Banned = f.Random.Bool(0.1f);
                o.MemberExpirationDate = DateTime.UtcNow.AddDays(5);
                o.Skills = GenerateRandomSkills();
            });

        public static readonly Faker<Account> ValidBotAccounts = CreateFaker(false, true, false);
        public static readonly Faker<Account> ValidMuleAccounts = CreateFaker(true, true, false);

        public static readonly Faker<Mule> Mules = new Faker<Mule>().Rules((f, m) =>
        {
            m.Account = ValidMuleAccounts.Generate();
            m.StartTime = f.Date.Between(DateTime.UtcNow.AddHours(-2), DateTime.UtcNow);
            m.LastUpdate = DateTime.UtcNow;
            m.Gold = f.Random.Number(10000, 5000000);
            m.Tag = Guid.NewGuid();
            m.DisplayName = f.Internet.UserName();
            m.Position = Position.Generate();
            m.World = f.Random.Int(1, 500);
        });
        
        public static readonly Faker<Bot> Bots = new Faker<Bot>().Rules((f, b) =>
            {
                b.Account = ValidBotAccounts.Generate();
                b.StartTime = f.Date.Between(DateTime.UtcNow.AddHours(-2), DateTime.UtcNow);
                b.LastUpdate = DateTime.UtcNow;
                b.GoldEarned = f.Random.Number(10000, 5000000);
                b.Tag = Guid.NewGuid();
                b.DisplayName = f.Internet.UserName();
            });
        
        public static readonly Faker<Proxy> Proxies = new Faker<Proxy>().Rules((f, o) =>
        {
            o.Ip = f.Internet.Ip();
            o.Port = f.Random.Int(MinimumPort, MaximumPort);
            o.Username = f.Internet.UserName();
            o.Password = f.Internet.Password();
        });

        public static readonly Faker<Position> Position = new Faker<Position>()
            .RuleFor(p => p.X, f => f.Random.Int())
            .RuleFor(p => p.Y, f => f.Random.Int())
            .RuleFor(p => p.Z, f => f.Random.Int());
        
        private const int MinimumPort = 1;
        private const int MaximumPort = 65535;

        public static ICollection<Skill> GenerateRandomSkills()
        {
            var faker = new Faker();

            var skills = Enum.GetValues(typeof(SkillType)).Cast<SkillType>().Select(s =>
                new Skill(s, faker.Random.Int(s == SkillType.Hitpoints ? 10 : 1, Skills.MaxLevel))).ToList();

            return skills;
        }
    }
}
