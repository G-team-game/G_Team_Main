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

    public IReadOnlyReactiveProperty<InGameUiType> inGameUiId => _inGameUiId;
    private readonly ReactiveProperty<InGameUiType> _inGameUiId = new ReactiveProperty<InGameUiType>(InGameUiType.outgame);

    public void ChangeUIIdInt(int id)
    {
        _inGameUiId.Value = (InGameUiType)id;
    }

    public void ChangeUIId(InGameUiType type)
    {
        _inGameUiId.Value = type;
    }
}
