using UnityEngine;

public class EmptyState : IState<Character>
{
    private static EmptyState m_Instance;
    private EmptyState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static EmptyState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new EmptyState();
            }

            return m_Instance;
        }
    }

    public void Enter(Character _charState)
    {
        _charState.OnEmptyStateEnter();
    }

    public void Execute(Character _charState)
    {
        _charState.OnEmptyStateExecute();
    }

    public void Exit(Character _charState)
    {
        _charState.OnEmptyStateExit();
    }
}