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
            _statSystem = FindFirstObjectByType<StatSystem>();

            _moverFactory = new MoverFactory();
            _isInitialized = true;
        }
        public ProjectileContext CreateContext(SkillSlot slot, GameObject owner)
        {
            if (slot == null || slot.IsEmpty) return null;

            return new ProjectileContext
            {
                data = slot.skillData,
                owner = owner
            };
        }
        public float GetCooldown(SkillSlot slot, IStatSourceable statSource)
        {
            if (slot == null || slot.IsEmpty || statSource == null)
                return 0f;

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
            if (!_isInitialized || slot == null) return;

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
