using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState CurrentState { get; private set; }

    void Start()
    {
        CurrentState = GetInitialState();
        CurrentState?.Enter();
    }

    void Update()
    {
        CurrentState?.Update();
    }

    public void ChangeState(BaseState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }
}