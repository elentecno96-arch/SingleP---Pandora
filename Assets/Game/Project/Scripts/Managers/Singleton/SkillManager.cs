using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Interface;
using Game.Project.Scripts.Core.Projectile.Rune;
using Game.Project.Scripts.Core.Projectile.SO;
using Game.Project.Scripts.Core.Projectile.Strategys.Mover;
using Game.Project.Scripts.Managers.Systems;
using Game.Project.Scripts.Managers.Systems.SkillSystems;
using Game.Project.Utillity.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.Project.Scripts.Managers.Systems.StateSystem;
using static UnityEditor.MaterialProperty;

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

        public void ApplySkill(ProjectileContext prototype)
        {
            if (!_isInitialized) return;
            _modifierSystem.ApplyModifiers(prototype, PlayerManager.Instance.Stats.CurrentStat);
            List<Projectile> projectiles = _spawnSystem.CreateProjectiles(prototype);
        }
    }
}
