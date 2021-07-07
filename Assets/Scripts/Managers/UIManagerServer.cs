using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerServer : Mirror.NetworkBehaviour
{

    private PlayerManager playerManager;
    [SerializeField] private List<Text> playerName;
    [SerializeField] private List<Text> playerPrimary;
    [SerializeField] private List<Text> playerSecondary;
    [SerializeField] private List<Text> playerUtility;
    private List<Player> players;

    // Start is called before the first frame update
    void Start()
    {
        if (!isServerOnly)
            this.gameObject.SetActive(false);

        playerManager = PlayerManager.GetPlayerManager();
        if (playerManager != null)
        {
            players = playerManager.GetPlayers();
            playerManager.OnPlayerAdded += OnPlayerAdded;
        }
        UpdateUI();
    }

    private void OnPlayerAdded(object sender, System.EventArgs e)
    {
        players = playerManager.GetPlayers();
        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateNames();
        UpdatePrimary();
    }

    private void UpdateNames()
    {
        return;
        for(int i = 0; i < players.Count; i++)
        {
            // get name from player NOT IMPLEMENTED
            playerName[i].text = "Player" + i;
        }
    }

    private void UpdatePrimary()
    {

        if (players == null)
            return;

        for (int i = 0; i < playerPrimary.Count; i++)
        {
            if (i < players.Count)
            {
                // get primary from player
                PlayerCombat pc = players[i].GetComponent<PlayerCombat>();

                if (!pc)
                {
                    playerPrimary[i].text = "";
                    continue;
                }

                Weapon wpn = pc.GetWeapon();

                if (!wpn)
                {
                    playerPrimary[i].text = "";
                    continue;
                }

                string weaponName = wpn.GetWeaponName();
                playerPrimary[i].text = weaponName;
                continue;
            }
            else
            {
                playerPrimary[i].text = "";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
}
