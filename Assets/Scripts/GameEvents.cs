using System;

public static class GameEvents
{
    public static Action OnPlayerGetsHit;
    public static void PlayerGetsHit()
    {
        OnPlayerGetsHit?.Invoke();
    }
}
