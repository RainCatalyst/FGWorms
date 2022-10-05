using System;

[Serializable]
public class BaseState
{
    public string Name { get; private set; }
    protected StateMachine StateMachine;

    public virtual void Setup(string name, StateMachine stateMachine)
    {
        Name = name;
        StateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}