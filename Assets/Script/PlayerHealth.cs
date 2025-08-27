using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : HealthManager
{
    public UnityEvent OnDead;
    protected override void Die()
    {
        base.Die(); 

        OnDead?.Invoke();
        Time.timeScale = 0;

    }
}