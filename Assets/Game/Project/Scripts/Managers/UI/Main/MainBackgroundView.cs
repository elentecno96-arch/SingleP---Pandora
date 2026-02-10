using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Game.Project.Scripts.Managers.UI.Main
{
    /// <summary>
    /// ¸ÞÀÎ ¾À ¹é±×¶ó¿îµå ºä
    /// </summary>
    public class MainBackgroundView : MonoBehaviour
    {
        [SerializeField] private RawImage rawImage;
        [SerializeField] private CanvasGroup backgroundCanvasGroup;
        [SerializeField] private VideoPlayer videoPlayer;

        public void InitView()
        {
            if (backgroundCanvasGroup != null) backgroundCanvasGroup.alpha = 0f;
            if (rawImage != null) rawImage.color = Color.white;

            if (videoPlayer != null)
            {
                videoPlayer.isLooping = true;
                videoPlayer.Prepare();
            }
        }

        public void PlayVideo()
        {
            if (videoPlayer != null) videoPlayer.Play();
        }

        public void SetAlpha(float alpha)
        {
            if (backgroundCanvasGroup != null) backgroundCanvasGroup.alpha = alpha;
        }
    }
}
