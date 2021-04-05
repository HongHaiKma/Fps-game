using UnityEngine;

public class ShootFlagState : IState<Character>
{
    private static ShootFlagState m_Instance;
    private ShootFlagState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static ShootFlagState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new ShootFlagState();
            }

            return m_Instance;
        }
    }

    public void Enter(Character _charState)
    {
        _charState.OnShootFlagEnter();
    }

    public void Execute(Character _charState)
    {
        _charState.OnShootFlagExecute();
    }

    public void Exit(Character _charState)
    {
        _charState.OnShootFlagExit();
    }
}