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

            ProjectileContext ctx = new ProjectileContext
            {
                data = slot.skillData,
                owner = owner
            };
            return ctx;
        }
        public float GetCooldown(SkillData data, Stat playerStat)
        {
            ProjectileContext c = new ProjectileContext { data = data };
            _modifierSystem.ApplyModifiers(c, playerStat);
            return c.finalCooldown;
        }
        public void ApplySkill(ProjectileContext prototype)
        {
            if (!_isInitialized) return;
            _modifierSystem.ApplyModifiers(prototype, PlayerManager.Instance.Stats.CurrentStat);
            _spawnSystem.CreateProjectiles(prototype);
        }
    }
}
