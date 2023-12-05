using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class WaveUIView : MonoBehaviour
{
    [SerializeField] private CanvasGroup waveCanvasGroup;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Transform waveUIParent;
    [SerializeField] private WaveEnemyIconView waveEnemyIcon;
    [SerializeField] private StageDatabase stageDatabase;

    private List<WaveEnemyIconView> waveEnemyIcons = new List<WaveEnemyIconView>();
    private int stageCount;

    public void InitWaveUI(int stageCount)
    {
        waveEnemyIcons.Clear();
        if (waveUIParent.transform.childCount > 0)
        {
            foreach (Transform w in waveUIParent.transform)
            {
                Destroy(w);
            }
        }

        this.stageCount = stageCount;
        var waveData = stageDatabase.stageDatas[stageCount].waveData;
        text.text = "WAVE 1/" + waveData.wave.Count.ToString();
        DOTween.To(() => waveCanvasGroup.alpha, (v) => waveCanvasGroup.alpha = v, 1, 0.3f);

        for (int i = 0; i < waveData.wave[0].enemylist.Count; i++)
        {
            var w = Instantiate(waveEnemyIcon);
            w.transform.parent = waveUIParent;
            w.transform.localScale = Vector3.one;
            waveEnemyIcons.Add(w);
        }
    }

    public void UpdateWaveUI(int waveIndex)
    {
        var waveCount = waveIndex + 1;
        var waveData = stageDatabase.stageDatas[stageCount].waveData;
        text.text = "WAVE " + waveCount.ToString() + "/" + waveData.wave.Count.ToString();
        waveEnemyIcons[waveIndex].defeatEnemy();
    }
}
