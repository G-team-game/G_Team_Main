using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartMenu : MonoBehaviour
{
    public string gameScene; 
    public GameObject startMenuFirst;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(startMenuFirst);
    }

    public void StartButtonPress()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void ExitButtonPress()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    
}
