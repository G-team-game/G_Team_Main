using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputSystemExtension
{
    public static ReadOnlyReactiveProperty<bool> GetButtonProperty(this InputAction inputAction)
    {
        return Observable.EveryUpdate().Select(_ => inputAction.IsPressed()).ToReadOnlyReactiveProperty(false);
    }

    //Delta入力はUpdate基準なのでUpdate基準に変換(主にマウスで使用)
    public static ReadOnlyReactiveProperty<Vector2> GetDeltaAxisProperty(this InputAction inputAction)
    {
        return Observable.EveryUpdate().Select(_ => inputAction.ReadValue<Vector2>()).ToReadOnlyReactiveProperty(Vector2.zero);
    }

}