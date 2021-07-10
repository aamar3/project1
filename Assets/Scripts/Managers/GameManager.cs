using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
    static GameManager gm = null;
    private PlayerManager playerManager = null;
    private UIManager uiManager = null;
    private NetworkManager nm;
    private bool playFlag = false;
    private int playFlagCount = 0;
    private bool isRestartable = true;
    [SerializeField] private Text nameInputText = null;
    [SerializeField] private TeamUI teamUI = null;
    private TeamManager teamManager = null;
    private float timer1 = 0;
    private float timer2 = 0;
    private float timer3 = 0;
    private bool isRestarting = false;

    private void Awake()
    {
        gm = this;
    }

    void Start()
    {
        nm = NetworkManager.singleton;
        if (!nm)
            Debug.Log("Missing reference to NetworkManager");

        uiManager = UIManager.GetUIManager();
        if (!uiManager)
        {
            Debug.Log("GameManager does not have reference to UIManager");
            return;
        }

        playerManager = PlayerManager.GetPlayerManager();
        teamManager = TeamManager.GetTeamManager();
    }

    public static GameManager GetGameManager()
    {
        return gm;
    }

    public void PlayButtonHit()
    {
        Player localPlayer = playerManager.localPlayer;
        localPlayer.PlayButtonHit();
    }

    public void Play()
    {
        playFlag = true;
        //    DisableUI();
        //  RPCDisableUI();
        Debug.Log("Play called on server");
        //   playerManager.SpawnPlayers();
    }

    private void LoadLevel1()
    {
        nm.ServerChangeScene("Level1");
    }

    private void Update()
    {

        // not sure why this is here. Did it not work without ?? need to look at eventually
        if (playFlag)
        {
            playFlagCount++;
            if (playFlagCount > 1)
            {
                playFlag = false;
                playFlagCount = 0;
                LoadLevel1();
            }
        }

        if (isServer)
        {
            if (isRestarting)
            {
                if (timer1 < Time.time && timer2 == 0 && timer3 == 0)
                {
                    if(isServerOnly)
                        uiManager.Restart();

                    ResetClientUIManager();
                    timer2 = Time.time + .25f;
                    timer1 = 0;
                }

                if (timer2 < Time.time && timer1 == 0 && timer3 == 0)
                {
                    teamManager.Restart();
                    timer3 = Time.time + .25f;
                    timer2 = 0;
                }

                if(timer3 < Time.time && timer1 == 0 && timer2 == 0)
                {
                    if(isServerOnly)
                        StripItems();
                    
                    ClientStripItems();
                    timer3 = 0;
                    isRestarting = false;
                }
            }
        }

        if (Keyboard.current.uKey.wasReleasedThisFrame)
        {
            uiManager.Restart();
        }

        if (isServer)
        {
            if (Keyboard.current.iKey.wasReleasedThisFrame)
            {
                teamManager.Restart();
            }
        }
    }


    public void StripItems()
    {
        foreach(KeyValuePair<int,Player> player in PlayerManager.GetPlayerManager().GetPlayers())
        {
            GameObject gun = player.Value.GetGun();

            if (!gun)
            {
                Debug.Log("Can not add weapon script because gun is null");
                return;
            }
            Destroy(gun.GetComponent<Weapon>());
            Destroy(gun);

            player.Value.GetComponent<PlayerCombat>().SetWeapon(null);
            player.Value.Restart();
        }

        uiManager.UpdateUI();
    }

    [ClientRpc]
    public void ClientStripItems()
    {
        foreach (KeyValuePair<int, Player> player in PlayerManager.GetPlayerManager().GetPlayers())
        {
            GameObject gun = player.Value.GetGun();

            if (!gun)
            {
                Debug.Log("Can not add weapon script because gun is null");
                return;
            }
           // Destroy(gun.GetComponent<Weapon>());
            Destroy(gun);
            player.Value.GetComponent<PlayerCombat>().SetWeapon(null);
            player.Value.Restart();
            player.Value.GetComponent<PlayerCombat>().Start();
        }        
        uiManager.UpdateUI();
    }


    [ClientRpc]
    private void ResetClientUIManager()
    {
        uiManager.Restart();
    }

    public void RestartRequested()
    {
        if (isRestartable)
        {
            isRestarting = true;
            timer1 = Time.time + .25f;
            nm.ServerChangeScene("StoreScreen");
        }
    }

    /*
    public void JoinGame()
    {
        if (playerManager != null)
        {
            if (nameInputText.text != "")
                playerManager.GetLocalPlayer().SetName(nameInputText.text);
            else
                playerManager.GetLocalPlayer().SetName("Player");
        }

        nm.ServerChangeScene("StoreScreen");

        //SceneManager.LoadScene("StoreScreen");
    }
    */
}
