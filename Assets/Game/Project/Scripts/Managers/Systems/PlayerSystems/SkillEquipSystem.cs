using Game.Project.Scripts.Core.Projectile.Rune;
using Game.Project.Scripts.Core.Projectile.SO;
using Game.Project.Scripts.Data.Items;
using Game.Project.Scripts.Managers.Singleton;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Project.Scripts.Player.Equip;

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
                EquipSkill(0, defaultSkill);
                Debug.Log($"기본 스킬 장착 완료: {defaultSkill.name}");
            }
        }

        /// <summary>
        /// 슬롯에 스킬을 장착
        /// </summary>
        public bool EquipSkill(int index, SkillData data)
        {
            if (index < 0 || index >= _skillSlots.Count || data == null) return false;

            //스킬 교체 시 기존 룬 해제
            _skillSlots[index].skillData = data;
            _skillSlots[index].equippedRunes.Clear();

            UpdateSkillContext(index);
            return true;
        }

        /// <summary>
        /// 스킬 슬롯에 룬을 장착
        /// </summary>
        public bool EquipRune(int index, ItemData runeItem)
        {
            if (index < 0 || index >= _skillSlots.Count || runeItem == null) return false;

            SkillSlot slot = _skillSlots[index];
            if (slot.IsEmpty)
            {
                Debug.LogWarning("스킬이 없는 슬롯에는 룬을 장착할 수 없습니다.");
                return false;
            }

            // 룬 장착 가능 개수 체크
            if (slot.equippedRunes.Count >= slot.GetMaxRuneCount())
            {
                Debug.LogWarning("룬 슬롯이 가득 찼습니다.");
                return false;
            }
            slot.equippedRunes.Add(runeItem);

            UpdateSkillContext(index);
            return true;
        }

        /// <summary>
        /// 스킬의 수치(Context)를 다시 계산
        /// </summary>
        private void UpdateSkillContext(int index)
        {
            var player = PlayerManager.Instance;
            var slot = _skillSlots[index];

            if (player != null && !slot.IsEmpty)
            {
                slot.context = SkillManager.Instance.CreateContext(slot, player.gameObject);

                List<RuneData> extractedRunes = slot.equippedRunes
                    .Where(item => item != null && item.runeData != null)
                    .Select(item => item.runeData)
                    .ToList();

                SkillManager.Instance.ApplySkill(slot.context, extractedRunes, player.StatSource);
            }

            CallUpdate();
            OnSkillChanged?.Invoke();
        }

        private void CallUpdate()
        {
            if (PlayerManager.Instance?.Combat != null)
            {
                PlayerManager.Instance.Combat.RefreshAllSkill();
            }
        }

        /// <summary>
        /// 장비 슬롯 초기화
        /// </summary>
        public void ClearAllSlots()
        {
            if (_skillSlots == null) return;

            for (int i = 0; i < _skillSlots.Count; i++)
            {
                _skillSlots[i].skillData = null;
                _skillSlots[i].equippedRunes.Clear();
                _skillSlots[i].context = null;
            }
            if (defaultSkill != null)
            {
                EquipSkill(0, defaultSkill);
            }

            CallUpdate();
            OnSkillChanged?.Invoke();
        }
    }
}
