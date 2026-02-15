using Game.Project.Data.Stat;
using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.SO;
using Game.Project.Scripts.Managers.Systems.SkillSystems;
using Game.Project.Utility.Generic;
using UnityEngine;
using Game.Project.Scripts.Managers.Systems.PlayerSystems;

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
        public float GetCooldown(SkillSlot slot, IStatSourceable statSource)
        {
            if (slot == null || slot.IsEmpty || statSource == null) return 0f;

            if (!_isInitialized) Init();

            if (_modifierSystem == null)
            {
                Debug.LogError("ModifierSystem missing in SkillManager children!");
                return slot.skillData.cooldown;
            }

            ProjectileContext c = new ProjectileContext
            {
                data = slot.skillData
            };

            _modifierSystem.ApplyModifiers(
                c,
                slot.equippedRunes,
                statSource);

            return c.finalCooldown;
        }

        public void ApplySkill(ProjectileContext prototype, SkillSlot slot,IStatSourceable stat)
        {
            //중복 예외처리 주석 처리
            //if (!_isInitialized || slot == null) return;

            if (!_isInitialized || slot == null || stat == null)
                return;

            _modifierSystem.ApplyModifiers(
                prototype,
                slot.equippedRunes,
                stat);

            _spawnSystem.CreateProjectiles(prototype);
        }
    }
}
