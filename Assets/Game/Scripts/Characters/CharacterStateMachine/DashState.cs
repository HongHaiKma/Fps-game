using UnityEngine;

public class DashState : IState<Character>
{
    private static DashState m_Instance;
    private DashState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static DashState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new DashState();
            }

            return m_Instance;
        }
    }

    public void Enter(Character _charState)
    {
        _charState.OnDashStateEnter();
    }

    public void Execute(Character _charState)
    {
        _charState.OnDashStateExecute();
    }

    public void Exit(Character _charState)
    {
        _charState.OnDashStateExit();
    }
}