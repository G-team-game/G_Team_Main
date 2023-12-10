using UnityEngine;
using UniRx;
using UnityEngine.InputSystem;
using System;
public class InputModel : MonoBehaviour, IDisposable
{
    [SerializeField]
    private InputActionMap inputAction;

    private ReadOnlyReactiveProperty<bool> fire = default;
    public IReadOnlyReactiveProperty<bool> Fire => fire;

    private ReadOnlyReactiveProperty<Vector2> move = default;
    public IReadOnlyReactiveProperty<Vector2> Move => move;

    private ReadOnlyReactiveProperty<Vector2> look = default;
    public IReadOnlyReactiveProperty<Vector2> Look => look;

    private ReadOnlyReactiveProperty<bool> pause = default;
    public IReadOnlyReactiveProperty<bool> Pause => pause;

    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

    public void Initialize()
    {
        fire = inputAction.FindAction("Fire").GetButtonProperty();
        move = inputAction.FindAction("Move").GetDeltaAxisProperty();
        look = inputAction.FindAction("Look").GetDeltaAxisProperty();
        pause = inputAction.FindAction("Pause").GetButtonProperty();
    }

    private void OnDestroy()
    {
        Dispose();
    }

    public void Dispose()
    {
        inputAction?.Dispose();
        fire?.Dispose();
        move?.Dispose();
    }
}
