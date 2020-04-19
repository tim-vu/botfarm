using System.Collections.Generic;
using FORFarm.Domain.Common;
using FORFarm.Domain.Enums;

namespace FORFarm.Domain.ValueObjects
{
    public class Skill : ValueObject
    {
        public SkillType Type { get; }
        
        public int Level { get; }

        public Skill(SkillType type, int level)
        {
            Type = type;
            Level = level;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Type;
            yield return Level;
        }
    }
}