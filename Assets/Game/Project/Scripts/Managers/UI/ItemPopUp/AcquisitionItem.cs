using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.Project.Scripts.Managers.UI.ItemPopUp
{
    /// <summary>
    /// ¾ÆÀÌÅÛ È¹µæ ÆË¾÷ UI
    /// </summary>
    public class DuopItemsInfoPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI infoText; 
        [SerializeField] private CanvasGroup canvasGroup;  

        public void Setup(Game.Project.Scripts.Data.Items.ItemData item, int amount)
        {
            if (infoText != null)
            {
                if (item.type == Game.Project.Scripts.Data.Items.ItemType.Gold)
                {
                    infoText.text = $"{item.goldcAmount * amount} °ñµå È¹µæ!";
                }
                else
                {
                    infoText.text = $"{item.itemName} x{amount} È¹µæ!";
                }
            }

            StartCoroutine(FadeAndDestroy());
        }

        private IEnumerator FadeAndDestroy()
        {
            yield return new WaitForSeconds(1.5f);

            float duration = 0.5f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                }
                yield return null;
            }

            Destroy(gameObject); //ÃßÈÄ Ç®¸µ ¿¹Á¤
        }
    }
}
