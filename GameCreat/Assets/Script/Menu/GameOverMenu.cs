using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverMenuFirst;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(gameOverMenuFirst);
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    public void LoadStart()
    {
        SceneManager.LoadScene(1);
    }
}
