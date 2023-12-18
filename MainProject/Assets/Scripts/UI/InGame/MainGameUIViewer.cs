using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MainGameUIViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private TextMeshProUGUI clearTimeText;
    [SerializeField] private Image reticle, reticle2;
    [SerializeField] private EnemyIconView enemyIconView;
    [SerializeField] private Transform content;
    private List<EnemyIconView> enemyIconViews = new List<EnemyIconView>();

    public void ChnageReticleColor(Color color)
    {
        reticle.color = color;
        reticle2.color = color;
    }

    public void InitializeMainGameUI(int enemyCount)
    {
        enemyCountText.text = $"ENEMIES 0/{enemyCount}";

        for (int i = 0; i < enemyCount; i++)
        {
            var icon = Instantiate(enemyIconView);
            icon.transform.parent = content;
            enemyIconViews.Add(enemyIconView);
        }
    }

    public void UpdateEnemyCount(int count, int maxCount)
    {
        enemyIconViews[maxCount - count].defeatEnemy();
        enemyCountText.text = $"ENEMIES {count}/{maxCount}";
    }

    public void UpdateTimeText(float time)
    {
        timeText.text = "Time:" + time.ToString("F2");
    }
}
