using Game.Project.Scripts.Core.Projectile.Rune;
using Game.Project.Scripts.Core.Projectile.SO;
using UnityEngine;

namespace Game.Project.Scripts.Data.Items
{
    public enum ItemType { SkillBook, Rune, EnhanceStone, Gold }

    /// <summary>
    /// 아이템 SO
    /// </summary>
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Project/Item Data")]
    public class ItemData : ScriptableObject
    {
        public string id;           // ID
        public string itemName;     // 이름
        public ItemType type;       // 종류
        public Sprite icon;         // 아이콘
        [TextArea] public string description;
        public int maxStack = 99;   // 최대 중첩수

        public int goldcAmount;
        public SkillData skillData; 
        public RuneData runeData;
    }
}
