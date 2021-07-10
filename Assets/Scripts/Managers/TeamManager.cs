using Mirror;
using System;
using UnityEngine;

public class TeamManager : NetworkBehaviour
{
    private static int STARTING_VALUE = 5000;

    [SyncVar(hook = nameof(OnMoneyChanged))] 
    private int money = STARTING_VALUE;
    static TeamManager tm = null;
    public event EventHandler OnMoneyChange;


    public static TeamManager GetTeamManager()
    {
        return tm;
    }

    private void Awake()
    {
        if (tm != null)
        {
            Debug.LogWarning("Multiple TeamManagers detected in the scene. Only one TeamManager can exist at a time. The duplicate TeamManager will be destroyed.");
            Destroy(gameObject);
        }
        tm = this;
    }

    public int GetMoney()
    {
        return money;
    }

    public void SetMoney(int amount)
    {
        money += amount;
    }

    void OnMoneyChanged(int oldMoney, int newMoney)
    {
        OnMoneyChange?.Invoke(this, EventArgs.Empty);
    }

    public void Restart()
    {
        money = STARTING_VALUE;
    }    
}
