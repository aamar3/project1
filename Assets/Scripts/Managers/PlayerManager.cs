using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    private static PlayerManager playerManager;
    private Dictionary<int,Player> players = new Dictionary<int,Player>();
    public Player localPlayer { get; private set; }
    private Store store = null;
    public event EventHandler OnPlayerAdded;
    public event EventHandler AllPlayersDead;

    public void Start()
    {
        store = Store.GetStore();
    }

    public void Awake()
    {
        if (playerManager != null)
        {
            Debug.LogWarning("Multiple PlayerManager detected in the scene. Only one PlayerManager can exist at a time. The duplicate PlayerManager will be destroyed.");
            Destroy(gameObject);
        }

        playerManager = this;
    }

    public static PlayerManager GetPlayerManager()
    {
        return playerManager;
    }

    // I think this will eventually break because there's no gaurantee players will be added in the same
    // order on clients and server in all cases. probably should move to do on server and then dat synced
    // to clients
    public void Add(Player player)
    {
        Debug.Log("Player added");
        player.SetKey(players.Count);
        players.Add(players.Count,player);

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
        foreach(KeyValuePair<int,Player> player in players)
            player.Value.transform.position = new Vector3(25, 1, 25); 
        // foreach(Player player in players)
        //   player.transform.position = new Vector3(25, 1, 25);
    }

    public Dictionary<int,Player> GetPlayers()
    {
        return players;
    }

    public Player GetPlayer(int key)
    {
        return players[key];
    }

    public void PlayerPurchasedPrimary(Purchaseable purchaseable, int playerIndex)
    {
        purchaseable.Purchased(playerIndex);
        RPCPurchased(purchaseable.index, playerIndex);
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
