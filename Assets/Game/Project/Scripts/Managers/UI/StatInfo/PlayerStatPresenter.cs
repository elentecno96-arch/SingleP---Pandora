using Game.Project.Scripts.Managers.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatPresenter : MonoBehaviour
{
    [SerializeField] private GameObject _statPanel;
    [SerializeField] private StatDisplayView _statView;

    private Game.Project.Scripts.Managers.Systems.PlayerSystems.StatSystem _statSystem;
    private bool _isOpened = false;

    private IEnumerator Start() 
    {
        while (PlayerManager.Instance == null || PlayerManager.Instance.Stats == null)
        {
            yield return null;
        }

        _statSystem = PlayerManager.Instance.Stats;

        if (_statSystem != null)
        {
            _statSystem.OnStatChanged += RefreshUI;
        }

        ClosePanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_isOpened) ClosePanel();
            else OpenPanel();
        }

        if (_isOpened && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    private void OpenPanel()
    {
        _isOpened = true;
        _statPanel.SetActive(true);

        if (_statSystem == null && PlayerManager.Instance != null)
        {
            _statSystem = PlayerManager.Instance.Stats;
            if (_statSystem != null) _statSystem.OnStatChanged += RefreshUI;
        }

        RefreshUI();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ClosePanel()
    {
        _isOpened = false;
        _statPanel.SetActive(false);
    }

    private void RefreshUI()
    {
        Debug.Log($"[Presenter] RefreshUI »£√‚µ . View: {_statView != null}, System: {_statSystem != null}");
        if (_statView != null && _statSystem != null)
        {
            _statView.UpdateAllStats(_statSystem.CurrentStat);
        }
    }
}
