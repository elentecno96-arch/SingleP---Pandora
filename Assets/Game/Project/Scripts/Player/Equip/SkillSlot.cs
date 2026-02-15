using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Rune;
using Game.Project.Scripts.Core.Projectile.SO;
using System.Collections.Generic;

[System.Serializable]
public class SkillSlot
{
    public SkillData skillData;
    public ProjectileContext context;
    public List<RuneData> equippedRunes = new List<RuneData>();
    public bool IsEmpty => skillData == null;

    public float currentCooldown; 

    public void UpdateCooldown(float deltaTime)
    {
        if (currentCooldown > 0)
            currentCooldown -= deltaTime;
    }

    /// <summary>
    /// 스킬 등급에 따른 룬 장착 개수
    /// </summary>
    public int GetMaxRuneCount()
    {
        if (IsEmpty) return 0;
        return skillData.rarity switch
        {
            SkillRarity.Normal => 2,
            SkillRarity.Unique => 4,
            SkillRarity.Legend => 6,
            _ => 0
        };
    }
}
