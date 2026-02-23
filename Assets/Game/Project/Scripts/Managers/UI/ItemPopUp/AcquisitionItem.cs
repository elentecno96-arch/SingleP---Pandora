using Game.Project.Scripts.Data.Items;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.Project.Scripts.Managers.UI.ItemPopUp
{
    /// <summary>
    /// æ∆¿Ã≈€ »πµÊ ∆Àæ˜ UI
    /// </summary>
    public class DuopItemsInfoPopup : MonoBehaviour
    {
        [SerializeField] private GameObject goldGroup;      
        [SerializeField] private TextMeshProUGUI goldText;

        [SerializeField] private GameObject itemGroup;     
        [SerializeField] private TextMeshProUGUI itemText;

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float displayDuration = 1.5f;
        [SerializeField] private float fadeDuration = 0.5f;

        public Action<DuopItemsInfoPopup> OnReturnToPool; //«Æ∏µ ¿Ã∫•∆Æ

        public void Setup(ItemData item, int amount)
        {
            if (goldGroup != null) goldGroup.SetActive(false);
            if (itemGroup != null) itemGroup.SetActive(false);

            if (item.type == ItemType.Gold)
            {
                if (goldGroup != null && goldText != null)
                {
                    goldText.text = $"{item.goldcAmount * amount:N0} ∞ÒµÂ »πµÊ!";
                    goldGroup.SetActive(true);
                }
            }
            else
            {
                if (itemGroup != null && itemText != null)
                {
                    itemText.text = $"{item.itemName} x{amount} »πµÊ!";
                    itemGroup.SetActive(true);
                }
            }

            // 3. ø¨√‚ Ω√¿€
            gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(FadeAndDestroy());
        }

        private IEnumerator FadeAndDestroy()
        {
            if (canvasGroup != null) canvasGroup.alpha = 1f;
            yield return new WaitForSeconds(displayDuration);

            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                if (canvasGroup != null)
                    canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                yield return null;
            }

            OnReturnToPool?.Invoke(this);
            //Destroy(gameObject); //√ﬂ»ƒ «Æ∏µ øπ¡§
        }
    }
}
