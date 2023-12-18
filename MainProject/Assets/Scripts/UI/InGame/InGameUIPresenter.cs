using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using static InGameUIModel;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class InGameUIPresenter : MonoBehaviour
{
    [SerializeField] private List<FadeViewer> uiViewers = new List<FadeViewer>();
    [SerializeField] private List<Button> firstSelectButton = new List<Button>();
    [SerializeField] private InGameUIModel inGameUIModel;

    private void Start()
    {
        inGameUIModel.inGameUiId
        .Subscribe(type =>
        {
            Cursor.visible = type != InGameUiType.main;
            Cursor.lockState = type != InGameUiType.main ? CursorLockMode.None : CursorLockMode.Locked;

            Time.timeScale = type == InGameUiType.pause ? 0 : 1;

            for (int i = 0; i < uiViewers.Count; i++)
            {
                if (i == (int)type)
                {
                    uiViewers[i].FadeIn(null);
                }
                else
                {
                    uiViewers[i].FadeOut();
                }

            }

            if (Gamepad.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                firstSelectButton[(int)type].Select();
            }

        }).AddTo(this);
    }
}