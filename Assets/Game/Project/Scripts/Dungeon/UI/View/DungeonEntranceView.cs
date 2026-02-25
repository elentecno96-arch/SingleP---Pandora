using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Game.Project.Scripts.Dungeon.UI.View
{
    /// <summary>
    /// 던전 입장 UI
    /// </summary>
    public class DungeonEntranceView : MonoBehaviour
    {
        [SerializeField] private GameObject contentGroup;
        [SerializeField] private List<GameObject> pages;

        [SerializeField] private Button nextButton;
        [SerializeField] private Button prevButton;
        [SerializeField] private Button enterButton;

        public Action OnNextPageRequested;
        public Action OnPrevPageRequested;
        public Action OnEnterRequested;

        private void Awake()
        {
            nextButton?.onClick.AddListener(() => OnNextPageRequested?.Invoke());
            prevButton?.onClick.AddListener(() => OnPrevPageRequested?.Invoke());
            enterButton?.onClick.AddListener(() => OnEnterRequested?.Invoke());
        }

        public void Show(bool isShow)
        {
            contentGroup.SetActive(isShow);
        }

        public void UpdatePage(int index)
        {
            for (int i = 0; i < pages.Count; i++)
            {
                if (pages[i] != null)
                    pages[i].SetActive(i == index);
            }

            bool isFirst = index == 0;
            bool isLast = index == pages.Count - 1;

            if (prevButton != null)
                prevButton.gameObject.SetActive(!isFirst);

            if (nextButton != null)
                nextButton.gameObject.SetActive(!isLast);

            if (enterButton != null)
                enterButton.gameObject.SetActive(isLast);
        }

        public int PageCount => pages.Count;
    }
}
