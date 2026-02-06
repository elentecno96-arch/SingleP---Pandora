using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Interface;
using Game.Project.Scripts.Core.Projectile.Rune;
using Game.Project.Scripts.Core.Projectile.SO;
using Game.Project.Scripts.Core.Projectile.Strategys.Mover;
using Game.Project.Scripts.Managers.Systems.SkillSystems;
using Game.Project.Utillity.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Singleton
{
    /// <summary>
    /// 스킬 매니저
    /// </summary>
    public class SkillManager : Singleton<SkillManager>
    {
        [SerializeField] private List<SynergyData> synergyDatabase;

        private ModifierSystem _modifierSystem;
        private SynergySystem _synergySystem;
        private SpawnSystem _spawnSystem;

        private bool _isInitialized = false;

        public void Init()
        {
            if (_isInitialized) return;
            _modifierSystem = GetComponentInChildren<ModifierSystem>();
            _spawnSystem = GetComponentInChildren<SpawnSystem>();

            _synergySystem = new SynergySystem(synergyDatabase);

            _isInitialized = true;
        }

        public void Fire(ProjectileContext baseContext)
        {
            if (baseContext.data == null || baseContext.data.projectilePrefab == null) return;
            _modifierSystem.InitContextStats(baseContext);
            _modifierSystem.ApplyRunes(baseContext);
            _synergySystem.CheckSynergy(baseContext);
            _spawnSystem.Spawn(baseContext);
        }

    }
}
