using UnityEngine;

public class StandState : IState<Character>
{
    private static StandState m_Instance;
    private StandState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static StandState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new StandState();
            }

            return m_Instance;
        }
    }

    public void Enter(Character _charState)
    {
        _charState.OnStandEnter();
    }

    public void Execute(Character _charState)
    {
        _charState.OnStandExecute();
    }

    public void Exit(Character _charState)
    {
        _charState.OnStandExit();
    }
}