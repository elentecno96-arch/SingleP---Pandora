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
        //[SerializeField] private bool _isInitialized = false;

        private SceneManager _sceneManager;
        private PoolManager _pool;
        private EffectManager _effect;
        private SkillManager _skill;
        private UiManager _ui;
        private SpawnManager _spawn;
        private PlayerManager _player;

        private GameState _currentState = GameState.None;

        private Coroutine _bgmCo;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            _sceneManager = SceneManager.Instance;
            _skill = SkillManager.Instance;
            _player = PlayerManager.Instance;
            _pool = PoolManager.Instance;
            _effect = EffectManager.Instance;
            _spawn = SpawnManager.Instance;
            _ui = UiManager.Instance;
        }

        private void Start()
        {
            Debug.Log("=== [Phase 1] 기초 시스템 초기화 ===");
            _skill.Init(); 
            _pool.Init();
            _effect.Init();

            _player.Init();

            Debug.Log("=== [Phase 3] 연결 및 UI ===");
            _ui.Init();
            //var hud = _ui.GetCombatHUD();
            //if (hud != null && _player.State != null)
            //{
            //    hud.Bind(_player.State);
            //    hud.BindSkills(_player.skillEquip);
            //}

            _sceneManager.Init();
            _spawn.Init();

            Debug.Log("=== 모든 매니저 초기화 완료 ===");

            ChangeState(GameState.Intro);
        }

        public void ChangeState(GameState newState)
        {
            if (_currentState == newState) return;
            
            if (_bgmCo != null)
            {
                StopCoroutine(_bgmCo);
                _bgmCo = null;
            }

            _currentState = newState;

            switch (_currentState)
            {
                case GameState.Intro:
                    StartCoroutine(IntroBGMCo());
                    break;
                case GameState.Main:
                    break;
                case GameState.Tutorial:
                    //튜토리얼 BGM
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

        public void StartTutorial()
        {
            ChangeState(GameState.Tutorial);
            StartCoroutine(StartTutorialCo());
        }

        private IEnumerator StartTutorialCo()
        {
            yield return StartCoroutine(AudioManager.Instance.FadeOutBgmCo(1f));

            _sceneManager.LoadScene("1. Tutorial");
        }

        public void PlayerSpawn(Vector3 position)
        {
            if (_player == null) _player = PlayerManager.Instance;

            _player.SpawnPlayer(position);

            BindHUD();
        }

        private void BindHUD()
        {
            if (!UiManager.HasInstance || !PlayerManager.HasInstance) return;

            var hud = UiManager.Instance.GetCombatHUD();
            var pm = PlayerManager.Instance;

            if (hud != null)
            {
                if (pm.State != null) hud.Bind(pm.State);
                if (pm.skillEquip != null) hud.BindSkills(pm.skillEquip);

                if (pm.levelSystem != null)
                {
                    hud.BindLevel(pm.levelSystem);
                }
            }
        }
    }
}
