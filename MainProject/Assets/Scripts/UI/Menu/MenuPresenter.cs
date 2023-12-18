using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Events;
using Stage;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace UI.Menu
{
    public class MenuPresenter : MonoBehaviour
    {
        [SerializeField] private MenuModel menuModel;
        [SerializeField] private Button firstControls;
        [SerializeField] private List<FadeViewer> fadeScripts = new List<FadeViewer>();
        // Start is called before the first frame update
        void Start()
        {
            StageLoader.InitSettings();

            if (Gamepad.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                firstControls.Select();
            }

            List<UnityAction> fadeActions = new List<UnityAction>();
            for (int i = 0; i < fadeScripts.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        fadeActions.Add(null);
                        break;
                    case 1:
                        fadeActions.Add(null);
                        break;
                    case 2:
                        fadeActions.Add(StartGame);
                        break;
                    case 3:
                        fadeActions.Add(null);
                        break;
                    case 4:
                        fadeActions.Add(QuitGame);
                        break;
                }
            }

            menuModel.MenuWindowId
            .SkipLatestValueOnSubscribe()
            .Subscribe(id =>
            {
                for (int i = 0; i < fadeScripts.Count; i++)
                {
                    if (id == i)
                    {
                        fadeScripts[i].FadeIn(fadeActions[i]);
                    }
                    else
                    {
                        fadeScripts[i].FadeOut();
                    }
                }

            }).AddTo(this);

            fadeScripts[4].FadeOut();
        }

        void StartGame()
        {
            StageLoader.Load(0);
        }

        void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}