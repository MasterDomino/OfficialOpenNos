using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class SkillMapper
    {
        public SkillMapper()
        {
        }

        public void ToSkillDTO(Skill input, SkillDTO output)
        {
            output.AttackAnimation = input.AttackAnimation;
            output.CastAnimation = input.CastAnimation;
            output.CastEffect = input.CastEffect;
            output.CastId = input.CastId;
            output.CastTime = input.CastTime;
            output.Class = input.Class;
            output.Cooldown = input.Cooldown;
            output.CPCost = input.CPCost;
            output.Duration = input.Duration;
            output.Effect = input.Effect;
            output.Element = input.Element;
            output.HitType = input.HitType;
            output.ItemVNum = input.ItemVNum;
            output.Level = input.Level;
            output.LevelMinimum = input.LevelMinimum;
            output.MinimumAdventurerLevel = input.MinimumAdventurerLevel;
            output.MinimumArcherLevel = input.MinimumArcherLevel;
            output.MinimumMagicianLevel = output.MinimumMagicianLevel;
            output.MinimumSwordmanLevel = input.MinimumSwordmanLevel;
            output.MpCost = input.MpCost;
            output.Name = input.Name;
            output.Price = input.Price;
            output.Range = input.Range;
            output.SkillType = input.SkillType;
            output.SkillVNum = input.SkillVNum;
            output.TargetRange = input.TargetRange;
            output.TargetType = input.TargetType;
            output.Type = input.Type;
            output.UpgradeSkill = input.UpgradeSkill;
            output.UpgradeType = input.UpgradeType;
        }

        public void ToSkill(SkillDTO input, Skill output)
        {
            output.AttackAnimation = input.AttackAnimation;
            output.CastAnimation = input.CastAnimation;
            output.CastEffect = input.CastEffect;
            output.CastId = input.CastId;
            output.CastTime = input.CastTime;
            output.Class = input.Class;
            output.Cooldown = input.Cooldown;
            output.CPCost = input.CPCost;
            output.Duration = input.Duration;
            output.Effect = input.Effect;
            output.Element = input.Element;
            output.HitType = input.HitType;
            output.ItemVNum = input.ItemVNum;
            output.Level = input.Level;
            output.LevelMinimum = input.LevelMinimum;
            output.MinimumAdventurerLevel = input.MinimumAdventurerLevel;
            output.MinimumArcherLevel = input.MinimumArcherLevel;
            output.MinimumMagicianLevel = output.MinimumMagicianLevel;
            output.MinimumSwordmanLevel = input.MinimumSwordmanLevel;
            output.MpCost = input.MpCost;
            output.Name = input.Name;
            output.Price = input.Price;
            output.Range = input.Range;
            output.SkillType = input.SkillType;
            output.SkillVNum = input.SkillVNum;
            output.TargetRange = input.TargetRange;
            output.TargetType = input.TargetType;
            output.Type = input.Type;
            output.UpgradeSkill = input.UpgradeSkill;
            output.UpgradeType = input.UpgradeType;
        }
    }
}
