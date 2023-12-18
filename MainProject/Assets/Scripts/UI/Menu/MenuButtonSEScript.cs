using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;
namespace UI.Menu
{
    public class MenuButtonSEScript : MonoBehaviour
    {
        public void PlayPutSE()
        {
            SEManager.Instance.Play(SEPath.BUTTON);
        }

        public void PlayHighlightSE()
        {
            SEManager.Instance.Play(SEPath.HIGHLIGHT);
        }
    }
}