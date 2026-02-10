using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.UI.Intro;
using Game.Project.Utility.Extension;
using System.Collections;
using UnityEngine;

namespace Game.Project.Scripts.Managers.UI.Intro
{
    /// <summary>
    /// 인트로 중개자
    /// </summary>
    public class IntroPresenter : MonoBehaviour
    {
        [SerializeField] private IntroView logoView;
        [SerializeField] private BlackScreenView blackView;

        private IEnumerator Start()
        {
            blackView.SetAlpha(1f);
            logoView.SetAlpha(0f);

            yield return new WaitForSeconds(1f);

            yield return StartCoroutine(logoView.Group.FadeCo(0f, 1f, 2f));
            yield return new WaitForSeconds(3f);
            yield return StartCoroutine(logoView.Group.FadeCo(1f, 0f, 2f));

            blackView.SetAlpha(1f);

            GameManager.Instance.StartGame();
        }
    }
}
