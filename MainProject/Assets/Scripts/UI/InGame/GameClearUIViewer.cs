using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameClearUIViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void SetGameOverUI(int defeatEnemyCount, int clearTime)
    {
        text.text = $"Defeat Enemy Count:{defeatEnemyCount}\nClear Time{clearTime}";
    }
}
