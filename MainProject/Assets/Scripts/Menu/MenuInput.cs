using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuInput : MonoBehaviour
{
    [Header("Menu Object")]
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject settingMenuCanvas;
    [SerializeField] GameObject successMenuCanvas;
    [SerializeField] GameObject gameOverMenuCanvas;

    [Header("Player Script")]
    [SerializeField] GameObject _player;
    [SerializeField] GameObject hpBar;

    [Header("First Selected Options")]
    [SerializeField] GameObject _mainMenuFirst;
    [SerializeField] GameObject _settingMenuFirst;
    [SerializeField] GameObject _successMenuFirst;
    [SerializeField] GameObject _gameOverMenuFirst;

    private PlayerMove _playerMove;
    private Grappling _graplling;
    private PlayerHP _playerHP;

    private bool isPaused;
    private bool isOutGame;
    private PlayerInputSystem playerInput;
    private InputAction menuOpenCloseAction;

    private void Awake()
    {
        playerInput = new PlayerInputSystem();
        menuOpenCloseAction = playerInput.UI.MenuOpenClose;

        // Subscribe to the performed event for the MenuOpenClose action
        menuOpenCloseAction.performed += _ => TogglePause();
    }

    private void OnEnable()
    {
        // Enable the action
        menuOpenCloseAction.Enable();
    }

    private void OnDisable()
    {
        // Disable the action and unsubscribe to avoid memory leaks
        menuOpenCloseAction.Disable();
        menuOpenCloseAction.performed -= _ => TogglePause();
    }

    private void Start()
    {
        mainMenuCanvas.SetActive(false);
        settingMenuCanvas.SetActive(false);
        successMenuCanvas.SetActive(false);
        gameOverMenuCanvas.SetActive(false);

        _playerMove = _player.GetComponent<PlayerMove>();
        _playerHP = _player.GetComponent<PlayerHP>();
        _graplling = _player.GetComponent<Grappling>();
    }

    private void TogglePause()
    {
        if (!isPaused)
        {
            Pause();
        }
        else
        {
            Unpause();
        }
    }

    public void Pause()
    {
        if (isOutGame) return;
        isPaused = true;
        Time.timeScale = 0f;

        _playerMove.enabled = false;
        _graplling.enabled = false;
        _playerHP.enabled = false;
        hpBar.SetActive(false);

        OpenMainMenu();
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        _playerMove.enabled = true;
        _graplling.enabled = true;
        _playerHP.enabled = true;
        hpBar.SetActive(true);

        CloseAllMenu();
    }

    public void GameOver()
    {
        isOutGame = true;
        Time.timeScale = 0f;

        _playerMove.enabled = false;
        _graplling.enabled = false;
        _playerHP.enabled = false;
        hpBar.SetActive(false);

        OpenGameOverMenuHandle();
    }

    public void Success()
    {
        isOutGame = true;
        Time.timeScale = 0f;

        _playerMove.enabled = false;
        _graplling.enabled = false;
        _playerHP.enabled = false;
        hpBar.SetActive(false);

        OpenSuccessMenuHandle();
    }

    private void OpenMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        settingMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }

    private void OpenSettingMenuHandle()
    {
        settingMenuCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_settingMenuFirst);
    }

    private void OpenSuccessMenuHandle()
    {
        successMenuCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_successMenuFirst);
    }

    private void OpenGameOverMenuHandle()
    {
        gameOverMenuCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_gameOverMenuFirst);
    }

    private void CloseAllMenu()
    {
        mainMenuCanvas.SetActive(false);
        settingMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnSettingPress()
    {
        OpenSettingMenuHandle();
    }

    public void OnResumePress()
    {
        Unpause();
    }

    public void OnExitPress()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnSettingBackPress()
    {
        OpenMainMenu();
    }
}

