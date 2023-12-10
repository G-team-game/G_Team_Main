using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
public class WavePresenter : MonoBehaviour
{
    // [SerializeField] private StageDatabase stageDatabase;
    [SerializeField] private WaveModel waveModel;
    [SerializeField] private WaveUIViewer waveUIView;
    [SerializeField] private EnemyManagement enemyManagement;
    // Start is called before the first frame update
    void Start()
    {
        waveModel.StartStage();
        waveModel.StageIndex
            .Where(stage => stage >= 0)
            .Subscribe(stage =>
            {
                // var s = Instantiate(stageDatabase.stageDatas[stage].stageObject);
                // s.transform.position = Vector3.zero;
                // s.transform.rotation = Quaternion.identity;

                waveUIView.InitWaveUI(stage);

                // var item = Instantiate(stageDatabase.stageDatas[stage].waveData.wave[0].spwanItem);
                // var itemPosition = Instantiate(stageDatabase.stageDatas[stage].waveData.wave[0].spawnItemPosition);

                // item.transform.position = itemPosition.position;
                // item.WaveItem(waveModel);

                // var points = Instantiate(stageDatabase.stageDatas[stage].waveData.wave[0].spawnPoints);
                // points.transform.position = Vector3.zero;

            }).AddTo(this);

        waveModel.WaveIndex
        .Where(wave => wave >= 0)
            .Subscribe(wave =>
            {
                waveUIView.UpdateWaveUI(wave, 1);
                enemyManagement.SpawnEney(waveModel.StageIndex.Value, wave);
            }).AddTo(this);
    }
}
