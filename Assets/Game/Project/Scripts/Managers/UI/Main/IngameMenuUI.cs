using System.Collections;
using System.Collections.Generic;
using Game.Project.Scripts.Managers.Singleton;
using UnityEngine;


public class IngameMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject menuRoot;    
    [SerializeField] private GameObject mainPanel;    
    [SerializeField] private GameObject optionPanel;  
    [SerializeField] private GameObject confirmPopup; 

    private bool _isPaused = false;

    void Update()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (currentScene == "Intro" || currentScene == "Main")
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0f; 
        menuRoot.SetActive(true);
        ShowMainPanel();
    }

    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        menuRoot.SetActive(false);
    }

    private void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        optionPanel.SetActive(false);
        confirmPopup.SetActive(false);
    }

    public void OnClickOption()
    {
        mainPanel.SetActive(false);
        optionPanel.SetActive(true);
    }

    public void OnClickGoToMenu()
    {
        confirmPopup.SetActive(true);
    }

    public void ConfirmGoToMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.Instance.LoadScene("MainMenu");
    }

    public void CloseSubPanel()
    {
        optionPanel.SetActive(false);
        confirmPopup.SetActive(false);
        mainPanel.SetActive(true);
    }
}
