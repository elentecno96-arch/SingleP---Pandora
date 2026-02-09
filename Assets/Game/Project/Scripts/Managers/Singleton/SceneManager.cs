using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Project.Utility.Generic;

public class SceneManager : Singleton<SceneManager>
{
    private bool _isInitialized = false;

    public void Init()
    {
        if (_isInitialized) return;
        

        _isInitialized = true;
        Debug.Log("SceneManager: 초기화 완료");
    }
    public void LoadScene(string sceneName)
    {
        Debug.Log($"SceneManager: {sceneName} 로딩 시도");
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
