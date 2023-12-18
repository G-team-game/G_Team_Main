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
        SceneManager.LoadScene("StartMenu");
    }
    public void LoadStart()
    {
        SceneManager.LoadScene(1);
    }
}