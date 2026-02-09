using Game.Project.Scripts.Core.Projectile.SO;

[System.Serializable]
public class SkillSlot
{
    public SkillData skillData;

    public bool IsEmpty => skillData == null;

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
