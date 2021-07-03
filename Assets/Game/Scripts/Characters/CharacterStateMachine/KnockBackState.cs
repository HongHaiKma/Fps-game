using UnityEngine;

public class KnockBackState : IState<Character>
{
    private static KnockBackState m_Instance;
    private KnockBackState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static KnockBackState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new KnockBackState();
            }

            return m_Instance;
        }
    }

    public void Enter(Character _charState)
    {
        _charState.OnKnockBackStateEnter();
    }

    public void Execute(Character _charState)
    {
        _charState.OnKnockBackStateExecute();
    }

    public void Exit(Character _charState)
    {
        _charState.OnKnockBackStateExit();
    }
}