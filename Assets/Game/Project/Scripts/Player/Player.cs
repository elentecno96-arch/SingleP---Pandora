using Game.Project.Data.Damage;
using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Managers.Singleton;
using UnityEngine;

namespace Game.Project.Scripts.Player
{
    /// <summary>
    /// 플레이어 객체
    /// </summary>
    public class Player : MonoBehaviour, IDamageable
    {
        private PlayerMovement movement;
        private PlayerCombat combat;

        private PlayerManager _manager;
        private bool _isInitialized = false;

        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            combat = GetComponent<PlayerCombat>();
        }

        private void Update()
        {
            if (!_isInitialized) return;
            MoveInput();
        }

        public void Init()
        {
            if (_isInitialized) return;

            _manager = PlayerManager.Instance;

            if (_manager == null)
            {
                Debug.LogError("Player: PlayerManager 없음");
                return;
            }

            var stats = _manager.Stats;
            if (stats == null)
            {
                Debug.LogError("Player: Stats 없음");
                return;
            }

            movement.Init(stats.CurrentStat.maxMoveSpeed);

            SetupCameraFollow();

            _isInitialized = true;
        }

        /// <summary>
        /// 플레이어가 2중 피격이 되어 주석 처리, 추후 개선 필요
        /// </summary>
        /// <param name="other"></param>
        //private void OnTriggerEnter(Collider other)
        //{
        //    if (!_isInitialized) return;

        //    if (other.TryGetComponent(out Projectile projectile))
        //    {
        //        TakeDamage(projectile.Context);
        //    }
        //}

        /// <summary>
        /// 플레이어 피해 처리
        /// </summary>
        /// <param name="context"></param>
        public void TakeDamage(ProjectileContext context)
        {
            var state = _manager?.State;
            if (state != null)
            {
                state.TakeDamage(context);
            }
        }

        void MoveInput() //추후 인풋 매니저로 관리 예정
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            movement.SetInput(new Vector2(h, v));
        }

        private void SetupCameraFollow()
        {
            var vcam = FindFirstObjectByType<Cinemachine.CinemachineVirtualCamera>();

            if (vcam != null)
            {
                vcam.Follow = transform;
            }
            else
            {
                Debug.LogWarning("Player: VirtualCamera를 찾지 못했습니다.");
            }
        }

        private void OnDestroy()
        {
            if (PlayerManager.HasInstance)
            {
                PlayerManager.Instance.UnregisterPlayer(gameObject);
            }
        }
    }
}
