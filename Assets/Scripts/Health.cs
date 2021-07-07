using Mirror;
using System;
using UnityEngine;
public class Health : NetworkBehaviour
{
    [SyncVar(hook = nameof(HealthChanged))] 
    [SerializeField] private float health;
    private float maxHealth;

    public event EventHandler OnHealthChange;

    // Start is called before the first frame update

    public void Awake()
    {
        maxHealth = health;
    }

    public void removeHealth(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
            RpcDie();
        }

        OnHealthChange?.Invoke(this, EventArgs.Empty);
    }

    private void Die()
    {
        if (!isServer)
            return;
        
        gameObject.SetActive(false);
    }

    [ClientRpc]
    private void RpcDie()
    {
        gameObject.SetActive(false);
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    void HealthChanged(float oldHealth, float newHealth)
    {
        OnHealthChange?.Invoke(this, EventArgs.Empty);
    }
}
