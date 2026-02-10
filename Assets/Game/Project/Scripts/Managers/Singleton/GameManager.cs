using Game.Project.Utility.Generic;
using UnityEngine;
using System.Collections;

namespace Game.Project.Scripts.Managers.Singleton
{
    /// <summary>
    /// 게임의 흐름을 관리하는 매니저
    /// </summary>
    public class GameManager : Singleton<GameManager> 
    {
        public enum GameState
        {
            None,
            Intro,
            Main,
            Lobby,
            Tutorial
        }

        [Header("System Readiness")]
        [SerializeField] private bool _isInitialized = false;

        private SceneManager _sceneManager;
        private PoolManager _pool;
        private EffectManager _effect;
        private SkillManager _skill;
        private UiManager _ui;
        private SpawnManager _spawn;
        private PlayerManager _player;

        private GameState _currentState = GameState.None;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            InitAllManagers();
        }
        private void Start()
        {
            ChangeState(GameState.Intro);
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

            //_audio = AudioManager.Instance;
            //_audio.Init();

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
        public void ChangeState(GameState newState)
        {
            if (_currentState == newState) return;
            _currentState = newState;

            switch (_currentState)
            {
                case GameState.Intro:
                    StartCoroutine(IntroBGMCo());
                    break;
                case GameState.Main:
                    break;
            }
        }

        private IEnumerator IntroBGMCo()
        {
            yield return new WaitForSeconds(1.0f);
            AudioManager.Instance.PlayIntroBgm();
        }

        public void StartGame()
        {
            ChangeState(GameState.Main);
            _sceneManager.LoadScene("6. Main");
        }
    }
}
