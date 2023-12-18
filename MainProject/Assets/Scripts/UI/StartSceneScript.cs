using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using KanKikuchi.AudioManager;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class StartSceneScript : MonoBehaviour
{
    [SerializeField] Button firstControls;
    [SerializeField] Animator cont, how;
    [SerializeField] private CanvasGroup canvasGroup, control, howto;
    PlayerInput playerInput;
    InputAction cancel;
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        cancel = playerInput.actions["Cancel"];

        if (Gamepad.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            firstControls.Select();
        }

        canvasGroup.alpha = 1.0f;
        DOTween.To(() => canvasGroup.alpha, (a) => canvasGroup.alpha = a, 0.0f, 0.3f);
    }

    private void Update()
    {
        if (cancel.WasPressedThisFrame())
        {
            Debug.Log("Cancel");
            OnHowTo(false);
            OnControl(false);
            firstControls.Select();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("selected!!");
        SEManager.Instance.Play(SEPath.HIGHLIGHT);
    }

    public void OnStart()
    {
        SEManager.Instance.Play(SEPath.BUTTON);
        DOTween.To(() => canvasGroup.alpha, (a) => canvasGroup.alpha = a, 0.0f, 0.3f).SetDelay(0.3f).OnComplete(() => SceneManager.LoadScene("Main"));
    }

    public void OnHowTo(bool isOpen)
    {
        SEManager.Instance.Play(SEPath.BUTTON);
        DOTween.To(() => howto.alpha, (a) => howto.alpha = a, isOpen ? 1.0f : 0.0f, 0.2f).SetDelay(0.2f).OnComplete(() =>
        {
            howto.blocksRaycasts = isOpen;
            howto.interactable = isOpen;
        });
    }

    public void OnControl(bool isOpen)
    {
        SEManager.Instance.Play(SEPath.BUTTON);
        control.blocksRaycasts = isOpen;
        control.interactable = isOpen;
        DOTween.To(() => control.alpha, (a) => control.alpha = a, isOpen ? 1.0f : 0.0f, 0.2f).SetDelay(0.2f).OnComplete(() =>
        {
            control.blocksRaycasts = isOpen;
            control.interactable = isOpen;
        });
    }

    public void OnPointEnter()
    {
        SEManager.Instance.Play(SEPath.HIGHLIGHT);
    }

    public void OnExit()
    {
        SEManager.Instance.Play(SEPath.BUTTON);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}