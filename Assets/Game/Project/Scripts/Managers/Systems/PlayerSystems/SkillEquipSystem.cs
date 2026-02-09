using Game.Project.Scripts.Core.Projectile.SO;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems.PlayerSystems
{
    public class SkillEquipSystem : MonoBehaviour
    {
        [SerializeField] private List<SkillSlot> _skillSlots;
        [SerializeField] private SkillData defaultSkill;

        public List<SkillSlot> GetSkillSlots() => _skillSlots;

        public System.Action OnSkillChanged;
        public void init()
        {
            _skillSlots = new List<SkillSlot>
            {
                 new SkillSlot(),
                 new SkillSlot(),
                 new SkillSlot()
            };
            if (defaultSkill != null)
            {
                _skillSlots[0].skillData = defaultSkill;
                Debug.Log($"기본 스킬 장착 완료: {defaultSkill.name}");
            }
        }
        /// <summary>
        /// 특정 슬롯에 스킬을 장착합니다.
        /// </summary>
        public void EquipSkill(int index, SkillData data)
        {
            if (index < 0 || index >= _skillSlots.Count) return;

            _skillSlots[index].skillData = data;

            NotifyUpdate();
            OnSkillChanged?.Invoke();
        }

        private void NotifyUpdate()
        {
            // PlayerManager를 통해 Combat의 Refresh 호출 예정
            Debug.Log("스킬 장착 정보가 변경되었습니다.");
        }
    }
}
