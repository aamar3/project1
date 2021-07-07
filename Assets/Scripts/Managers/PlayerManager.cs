using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    private static PlayerManager playerManager;
    private List<Player> players = new List<Player>();
    private Player localPlayer;
    private Store store = null;
    public event EventHandler OnPlayerAdded;
    public event EventHandler AllPlayersDead;

    public void Start()
    {
        store = Store.GetStore();
    }

    public void Awake()
    {
        playerManager = this;
    }

    public static PlayerManager GetPlayerManager()
    {
        return playerManager;
    }

    public void Add(Player player)
    {
        Debug.Log("Player added");
        player.SetIndex(players.Count);
        players.Add(player);

        if (player.isLocalPlayer)
        {
            localPlayer = player;
            Debug.Log("Local player set");
        }

        OnPlayerAdded?.Invoke(this, EventArgs.Empty);

        if(isServer)
        {
            InstantiateBasePlayerAddons();
        }
    }

    [ClientRpc]
    private void InstantiateBasePlayerAddons()
    {

    }

    // pretty sure old test code delete soon 7/3/2021 12:31pm
    public void SetPrimary(Weapon wpn)
    {
        players[0].SetPrimary(wpn);
        // add weapon specific
    }

    public void SpawnPlayers()
    {
        foreach(Player player in players)
            player.transform.position = new Vector3(25, 1, 25);
    }

    public List<Player> GetPlayers()
    {
        return players;
    }

    public Player GetLocalPlayer()
    {
        return localPlayer;
    }


    public void PlayerPurchasedPrimary(Purchaseable purchaseable, int playerIndex)
    {
        purchaseable.Purchased(playerIndex);
        RPCPurchased(purchaseable.GetPurchaseableIndex(), playerIndex);
    }

    [ClientRpc]
    public void RPCPurchased(int itemIndex, int playerIndex)
    {
        // get item from list of all purchaseables
        if(!store)
        {
            store = Store.GetStore();
            if (store == null)
            {
                Debug.Log("Missing reference to store");
                return;
            }
        }

        Purchaseable purchaseable = store.GetPurchaseable(itemIndex);
        purchaseable.Purchased(playerIndex);
        UIManager uIManager = UIManager.GetUIManager();
        uIManager.UpdateUI();
    }
}
