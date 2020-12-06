using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    public IState<T> curState;
    public IState<T> prevState;
    private T m_Owner;

    public StateMachine(T owner)
    {
        m_Owner = owner;
    }

    public void Init(IState<T> state)
    {
        this.curState = state;
        this.curState.Enter(m_Owner);
    }


    public void ChangeState(IState<T> newsStage)
    {
        if (curState != newsStage)
        {
            if (curState != null)
            {
                this.curState.Exit(m_Owner);
            }
            this.prevState = this.curState;
            this.curState = newsStage;
            this.curState.Enter(m_Owner);
        }
    }

    public void ExecuteStateUpdate()
    {
        IState<T> runningState = this.curState;

        if (runningState != null)
        {
            runningState.Execute(m_Owner);
        }
    }

    public void SwitchToPrevState()
    {
        this.curState.Exit(m_Owner);
        this.curState = this.prevState;
        this.curState.Enter(m_Owner);
    }
}