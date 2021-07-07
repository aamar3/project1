using Mirror;
using System;
using UnityEngine;

public class CreateManagers : NetworkBehaviour
{
    [SerializeField] GameManager gameManager = null;
    [SerializeField] PlayerManager playerManager = null;
    [SerializeField] TeamManager teamManager = null;
    [SerializeField] StoreManager storeManager = null;
    [SerializeField] UIManager uiManager = null;
    [SerializeField] Gun gun = null;


    [SyncVar]
    private bool managersCreated = false;
    public event EventHandler OnManagersCreated;
    private static CreateManagers cm = null;

    private void Awake()
    {
        cm = this;

        if (GameManager.GetGameManager() != null)
        {
            managersCreated = true;
            return;
        }
    }

    public static CreateManagers GetCreateManagers()
    {
        return cm;
    }

    void Start()
    {
        // if server, check if managers exist
        // if not create them

        if (!isServer || managersCreated)
            return;

        GameManager gm = Instantiate(gameManager);
        NetworkServer.Spawn(gm.gameObject);

        PlayerManager pm = Instantiate(playerManager);
        NetworkServer.Spawn(pm.gameObject);
        

        TeamManager tm = Instantiate(teamManager);
        NetworkServer.Spawn(tm.gameObject);
        
        StoreManager sm = Instantiate(storeManager);
        //NetworkServer.Spawn(sm.gameObject);

        UIManager ui = Instantiate(uiManager);
        NetworkServer.Spawn(ui.gameObject);

        managersCreated = true;

        NotifyClients();
    }

    [ClientRpc]
    private void NotifyClients()
    {
        OnManagersCreated?.Invoke(this, EventArgs.Empty);
    }

    public bool GetManagersCreated()
    {
        return managersCreated;
    }
}
