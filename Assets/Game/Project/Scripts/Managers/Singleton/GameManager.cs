using Game.Project.Scripts.Managers.Systems;
using Game.Project.Utillity.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Singleton
{
    public class GameManager : Singleton<GameManager> 
    {
        [Header("System Readiness")]
        [SerializeField] private bool _isInitialized = false;

        private PoolManager _pool;
        private EffectManager _effect;
        private SkillManager _skill;
        private AudioManager _audio;
        private UiManager _ui;
        private SpawnManager _spawn;
        private PlayerManager _player;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            InitAllManagers();
        }

        private void InitAllManagers()
        {
            if (_isInitialized) return;

            Debug.Log("=== 각 매니저 초기화 시작 ===");
            _pool = PoolManager.Instance;
            _pool.Init();
            _effect = EffectManager.Instance;
            _effect.Init();

            _audio = AudioManager.Instance;
            _audio.Init();

            _skill = SkillManager.Instance;
            _skill.Init();

            _spawn = SpawnManager.Instance;
            _spawn.Init();

            _player = PlayerManager.Instance;
            _player.Init();

            _ui = UiManager.Instance;
            _ui.Init();

            _isInitialized = true;
            Debug.Log("=== 각 매니저 초기화 완료 ===");
        }
    }
}
