using UnityEngine;

public class IdleState : IState<Character>
{
    private static IdleState m_Instance;
    private IdleState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static IdleState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new IdleState();
            }

            return m_Instance;
        }
    }

    public void Enter(Character _charState)
    {
        _charState.OnIdleEnter();
    }

    public void Execute(Character _charState)
    {
        _charState.OnIdleExecute();
    }

    public void Exit(Character _charState)
    {
        _charState.OnIdleExit();
    }
}