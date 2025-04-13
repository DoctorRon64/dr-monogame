using Microsoft.Xna.Framework;

namespace MonoEngine.Framework;

public interface IState<T> {
    public void OnInitialize(StateMachine<T> owner);
    public void OnEnter();
    public void OnExit();
    public void OnUpdate();
}