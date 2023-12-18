using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class WaveUIViewer : MonoBehaviour
{
    [SerializeField] private CanvasGroup waveCanvasGroup;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Transform waveUIParent;
    [SerializeField] private EnemyIconView waveEnemyIcon;

    private List<EnemyIconView> waveEnemyIcons = new List<EnemyIconView>();

    public void InitWaveUI(int enemycount)
    {
        waveEnemyIcons.Clear();
        if (waveUIParent.transform.childCount > 0)
        {
            foreach (Transform w in waveUIParent.transform)
            {
                Destroy(w);
            }
        }

        text.text = "Enemies 0/" + enemycount.ToString();
        DOTween.To(() => waveCanvasGroup.alpha, (v) => waveCanvasGroup.alpha = v, 1, 0.3f);

        for (int i = 0; i < enemycount; i++)
        {
            var w = Instantiate(waveEnemyIcon);
            w.transform.parent = waveUIParent;
            w.transform.localScale = Vector3.one;
            waveEnemyIcons.Add(w);
        }
    }

    public void UpdateWaveUI(int enemycount, int maxEnemyCount)
    {
        text.text = "ENEMIES " + enemycount.ToString() + "/" + maxEnemyCount.ToString();
        waveEnemyIcons[maxEnemyCount - enemycount].defeatEnemy();
    }
}
