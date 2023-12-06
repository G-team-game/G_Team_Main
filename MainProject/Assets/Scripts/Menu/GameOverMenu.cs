using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    // Update is called once per frame
    public void QuitGame()
    {
        Time.timeScale = 1;

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void LoadStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}