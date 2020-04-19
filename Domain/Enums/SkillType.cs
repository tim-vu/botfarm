using System;
using System.Collections.Generic;
using FORFarm.Domain.ValueObjects;

namespace FORFarm.Domain.Enums
{
    public enum SkillType
    {
        Agility = 1,
        Attack,
        Construction,
        Cooking,
        Crafting,
        Defence,
        Farming,
        Firemaking,
        Fishing,
        Fletching,
        Herblore,
        Hitpoints,
        Hunter,
        Magic,
        Mining,
        Prayer,
        Ranged,
        Runecrafting,
        Slayer,
        Smithing,
        Strength,
        Thieving,
        Woodcutting
    }

    public class Skills
    {
        public static ICollection<Skill> CreateNewDefaultSkills()
        {
            var skills = new List<Skill>();

            foreach (var skill in DefaultSkills)
            {
                skills.Add(new Skill(skill.Type, skill.Level));
            }

            return skills;
        }
        
        public static readonly ICollection<Skill> DefaultSkills = new List<Skill>
        {
            new Skill(SkillType.Agility, 1),
            new Skill(SkillType.Attack, 1),
            new Skill(SkillType.Construction, 1),
            new Skill(SkillType.Cooking, 1),
            new Skill(SkillType.Crafting, 1),
            new Skill(SkillType.Defence, 1),
            new Skill(SkillType.Farming, 1),
            new Skill(SkillType.Firemaking, 1),
            new Skill(SkillType.Fishing, 1),
            new Skill(SkillType.Fletching, 1),
            new Skill(SkillType.Herblore, 1),
            new Skill(SkillType.Hitpoints, 10),
            new Skill(SkillType.Hunter, 1),
            new Skill(SkillType.Magic, 1),
            new Skill(SkillType.Mining, 1),
            new Skill(SkillType.Prayer, 1),
            new Skill(SkillType.Ranged, 1),
            new Skill(SkillType.Runecrafting, 1),
            new Skill(SkillType.Slayer, 1),
            new Skill(SkillType.Smithing, 1),
            new Skill(SkillType.Strength, 1),
            new Skill(SkillType.Thieving, 1),
            new Skill(SkillType.Woodcutting, 1)
        };

        public const int MaxLevel = 99;
    }
}
