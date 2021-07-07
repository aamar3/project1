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
