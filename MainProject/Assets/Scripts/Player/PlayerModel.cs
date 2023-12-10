using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class PlayerModel : MonoBehaviour
{
    public IReadOnlyReactiveProperty<int> PlayerHp => _playerHp;
    private readonly IntReactiveProperty _playerHp = new IntReactiveProperty(1);

    public IReadOnlyReactiveProperty<bool> isSucsess => _isSucsess;
    private readonly BoolReactiveProperty _isSucsess = new BoolReactiveProperty(false);

    public void Success()
    {
        _isSucsess.Value = true;
    }

    public void RemoveHp(int value)
    {
        int hp = _playerHp.Value - value;
        //0以下にしない
        _playerHp.Value = hp < 0 ? 0 : hp;
    }
}
