using Game.Project.Data.Stat;
using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Rune;
using Game.Project.Scripts.Player.Equip;
using Game.Project.Scripts.Managers.Systems.PlayerSystems;
using Game.Project.Scripts.Managers.Systems.SkillSystems;
using Game.Project.Scripts.Data.Items;
using Game.Project.Utility.Generic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Singleton
{
    /// <summary>
    /// 스킬 매니저
    /// </summary>
    public class SkillManager : Singleton<SkillManager>
    {
        private ModifierSystem _modifierSystem;
        private SpawnSystem _spawnSystem;
        private StatSystem _statSystem;

        private bool _isInitialized = false;

        private MoverFactory _moverFactory;

        public void Init()
        {
            if (_isInitialized) return;

            _modifierSystem = GetComponentInChildren<ModifierSystem>();
            _spawnSystem = GetComponentInChildren<SpawnSystem>();
            _moverFactory = new MoverFactory();

            _isInitialized = true;
            Debug.Log("<color=cyan>SkillManager: 하위 시스템(Modifier, Spawn) 준비 완료</color>");
        }
        public ProjectileContext CreateContext(SkillSlot slot, GameObject owner)
        {
            //테스트하다가 주석처리한 부분 까먹었습니다 ㅠ
            if (slot == null || slot.IsEmpty) return null;

            return new ProjectileContext
            {
                data = slot.skillData,
                owner = owner
            };
        }
        private List<RuneData> ExtractRunes(SkillSlot slot)
        {
            if (slot == null || slot.equippedRunes == null) return new List<RuneData>();

            List<RuneData> runeList = new List<RuneData>();

            foreach (var item in slot.equippedRunes)
            {
                if (item != null && item.type == ItemType.Rune && item.runeData != null)
                {
                    runeList.Add(item.runeData);
                }
            }

            return runeList;
        }

        public float GetCooldown(SkillSlot slot, IStatSourceable statSource)
        {
            if (slot == null || slot.IsEmpty || statSource == null) return 0f;
            if (!_isInitialized) Init();

            ProjectileContext tempContext = new ProjectileContext { data = slot.skillData };

            _modifierSystem.ApplyModifiers(
                tempContext,
                ExtractRunes(slot), 
                statSource);

            return tempContext.finalCooldown;
        }

        /// <summary>
        /// 수치 계산만 적용
        /// </summary>
        /// <param name="prototype"></param>
        /// <param name="runes"></param>
        /// <param name="stat"></param>
        public void UpdateSkillStats(ProjectileContext prototype, List<RuneData> runes, IStatSourceable stat)
        {
            if (!_isInitialized || prototype == null || stat == null) return;
            _modifierSystem.ApplyModifiers(prototype, runes, stat);
        }

        /// <summary>
        /// 수치 계산 및 발사
        /// </summary>
        /// <param name="prototype"></param>
        /// <param name="runes"></param>
        /// <param name="stat"></param>
        public void ApplySkill(ProjectileContext prototype, List<RuneData> runes, IStatSourceable stat) 
        {
            if (!_isInitialized || prototype == null || stat == null) return;

            _modifierSystem.ApplyModifiers(prototype, runes, stat);
            _spawnSystem.CreateProjectiles(prototype);
        }
    }
}
