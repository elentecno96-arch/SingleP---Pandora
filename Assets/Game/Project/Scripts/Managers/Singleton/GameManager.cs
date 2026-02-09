using Game.Project.Utility.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Project.Scripts.Managers.Singleton
{
    public class GameManager : Singleton<GameManager> 
    {
        [Header("System Readiness")]
        [SerializeField] private bool _isInitialized = false;

        private SceneManager _sceneManager;
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
            _sceneManager = SceneManager.Instance;
            _sceneManager.Init();

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
        public void StartGame()
        {
            _sceneManager.LoadScene("6. Main");
        }
    }
}
