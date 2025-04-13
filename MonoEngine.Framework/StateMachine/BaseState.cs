namespace MonoEngine.Framework;

public abstract class BaseState<T> : IState<T> {
    protected StateMachine<T> StateMachine { get; private set; } = null!;
    protected T Blackboard => StateMachine.Blackboard;

    public virtual void OnInitialize(StateMachine<T> owner) {
        this.StateMachine = owner;
    }
    public abstract void OnEnter();
    public abstract void OnExit();
    public virtual void OnUpdate() { }
}