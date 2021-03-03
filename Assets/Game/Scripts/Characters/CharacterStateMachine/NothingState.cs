using UnityEngine;

public class NothingState : IState<Character>
{
    private static NothingState m_Instance;
    private NothingState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static NothingState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new NothingState();
            }

            return m_Instance;
        }
    }

    public void Enter(Character _charState)
    {
        _charState.OnNothingEnter();
    }

    public void Execute(Character _charState)
    {
        _charState.OnNothingExecute();
    }

    public void Exit(Character _charState)
    {
        _charState.OnNothingExit();
    }
}