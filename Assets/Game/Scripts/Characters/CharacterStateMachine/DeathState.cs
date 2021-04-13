using UnityEngine;

public class DeathState : IState<Character>
{
    private static DeathState m_Instance;
    private DeathState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static DeathState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new DeathState();
            }

            return m_Instance;
        }
    }

    public void Enter(Character _charState)
    {
        _charState.OnDeathStateEnter();
    }

    public void Execute(Character _charState)
    {
        _charState.OnDeathStateExecute();
    }

    public void Exit(Character _charState)
    {
        _charState.OnDeathStateExit();
    }
}