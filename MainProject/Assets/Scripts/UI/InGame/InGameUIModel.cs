using UnityEngine;
using UniRx;
public class InGameUIModel : MonoBehaviour
{
    public enum InGameUiType
    {
        outgame,
        main,
        pause,
        dead,
        clear,
    }

    //ゲームの状態
    public IReadOnlyReactiveProperty<InGameUiType> inGameUiId => _inGameUiId;
    private readonly ReactiveProperty<InGameUiType> _inGameUiId = new ReactiveProperty<InGameUiType>(InGameUiType.outgame);

    //経過時間
    public IReadOnlyReactiveProperty<float> passedTime => _passedTime;
    private readonly FloatReactiveProperty _passedTime = new FloatReactiveProperty(0f);

    //倒した敵の数
    public IReadOnlyReactiveProperty<int> enemyCount => _enemyCount;
    private readonly IntReactiveProperty _enemyCount = new IntReactiveProperty(0);
    public int stageEnemyCount { get; private set; }

    public void ChangeTime(float time)
    {
        _passedTime.Value = time;
    }

    public void ChangeUIIdInt(int id)
    {
        _inGameUiId.Value = (InGameUiType)id;
    }

    public void ChangeUIId(InGameUiType type)
    {
        _inGameUiId.Value = type;
    }

    public void SetStageEnemyCount(int value)
    {
        stageEnemyCount = value;
    }

    public void ChangeEnemyCount(int value)
    {
        _enemyCount.Value = value;
    }
}
