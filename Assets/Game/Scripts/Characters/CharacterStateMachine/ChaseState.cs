using UnityEngine;

public class ChaseState : IState<Character>
{
    private static ChaseState m_Instance;
    private ChaseState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static ChaseState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new ChaseState();
            }

            return m_Instance;
        }
    }

    public void Enter(Character _charState)
    {
        _charState.OnChaseEnter();
    }

    public void Execute(Character _charState)
    {
        _charState.OnChaseExecute();
    }

    public void Exit(Character _charState)
    {
        _charState.OnChaseExit();
    }
}