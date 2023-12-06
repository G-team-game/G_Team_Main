using UnityEngine;
using UniRx;
using System.Collections.Generic;
public sealed class WaveModel : MonoBehaviour
{
    [SerializeField] private StageDatabase stageDatabase;
    public IReadOnlyReactiveProperty<int> WaveIndex => _waveIndex;
    private readonly IntReactiveProperty _waveIndex = new IntReactiveProperty(-1);

    public IReadOnlyReactiveProperty<int> StageIndex => _stageIndex;
    private readonly IntReactiveProperty _stageIndex = new IntReactiveProperty(-1);

    public void StartStage()
    {
        _stageIndex.Value = 0;
    }

    public void StartWave()
    {
        _waveIndex.Value = 0;
    }

    private void OnDestroy()
    {
        _waveIndex.Dispose();
        _stageIndex.Dispose();
    }
}