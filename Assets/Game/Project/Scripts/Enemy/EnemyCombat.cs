using Game.Project.Data.Damage;
using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Managers.Singleton;
using UnityEngine;

namespace Game.Project.Scripts.Enemy
{
    /// <summary>
    /// 적의 공격 담당
    /// </summary>
    public class EnemyCombat : MonoBehaviour
    {
        private Enemy _owner;

        [SerializeField] private GameObject _warningIndicatorPrefab; // 장판
        [SerializeField] private GameObject _damageTextPrefab;

        public void Init(Enemy owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// 실제 공격을 수행 ( 플레이어와 같은 방법으로 스킬 매니저를 통해 스폰 )
        /// 몬스터는 슬롯이 없기 때문에 가상 슬롯 생성
        /// </summary>
        public void Attack()
        {
            var data = _owner.Data;

            if (data == null || data.skillData == null) return;

            SkillSlot virtualSlot = new SkillSlot { skillData = data.skillData, equippedRunes = null };

            ProjectileContext context = SkillManager.Instance.CreateContext(virtualSlot, _owner.gameObject);
            if (context == null) return;

            Vector3 targetDir = _owner.transform.forward;
            Vector3 spawnPos = _owner.transform.position + Vector3.up * 1.0f;

            if (_owner.Context.target != null)
            {
                Vector3 targetMidPos = _owner.Context.target.transform.position + Vector3.up * 0.5f;
                targetDir = (targetMidPos - spawnPos).normalized;
            }
            context.targetMask = _owner.GetPlayerLayerMask();
            context.firePosition = spawnPos + (targetDir * 0.5f); 
            context.direction = targetDir;
            context.target = _owner.Context.target;

            EnemyStatSource statSource = new EnemyStatSource(data, 1.0f, 0f);
            SkillManager.Instance.ApplySkill(context, virtualSlot, statSource);

            if (data.attackSfx != null)
                AudioManager.Instance.PlaySfxAtPoint(data.attackSfx, _owner.transform.position);
        }

        /// <summary>
        /// 공격이 발생하기 전 위험 표시
        /// </summary>
        public void ShowWarning(float duration, float range)
        {
            if (_warningIndicatorPrefab == null) return;

            Vector3 spawnPos = _owner.transform.position;
            spawnPos.y += 0.05f;

            Vector3 targetDir = _owner.Context.target != null ?
                (_owner.Context.target.transform.position - _owner.transform.position).normalized :
                _owner.transform.forward;

            targetDir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(targetDir);

            GameObject indicator = Instantiate(_warningIndicatorPrefab, spawnPos, rotation);

            float width = 1.5f;
            indicator.transform.localScale = new Vector3(width, 1f, range);

            Destroy(indicator, duration);
        }

        public void ShowDamageEffect(float damage, bool isCritical)
        {
            if (_damageTextPrefab == null) return;

            Vector3 spawnPos = transform.position + Vector3.up * 2.5f;
            spawnPos += new Vector3(
                Random.Range(-0.3f, 0.3f),
                0,
                Random.Range(-0.3f, 0.3f)
            );

            DamageText damageText = PoolManager.Instance.GetDamageText(_damageTextPrefab, spawnPos);

            if (damageText != null)
            {
                damageText.Setup(damage, isCritical);
            }
        }
    }
}

