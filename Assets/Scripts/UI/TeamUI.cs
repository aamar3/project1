using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamUI : MonoBehaviour
{
    private Text moneyText;
    private float money;
    private List<Text> primaries;
    private List<Text> secondaries;
    private List<Text> utilities;
    List<Player> players;
    private PlayerManager playerManager;
    private TeamManager teamManager;
    static TeamUI teamUI = null;

    private void Awake()
    {
        teamUI = this;
    }

    public static TeamUI GetTeamUI()
    {
        return teamUI;
    }

    /*
    public void SetPrimaries(List<Text> primaries)
    {
        this.primaries = primaries;
    }

    public void SetSecondaries(List<Text> secondaries)
    {
        this.secondaries = primaries;
    }
    public void SetUtilities(List<Text> utilities)
    {
        this.utilities = utilities;
    }
    public void SetMoneyText(Text moneyText)
    {
        this.moneyText = moneyText;
    }
    */

    void Start()
    {
        PassUIReferencesToUIManagers references = PassUIReferencesToUIManagers.GetPassUIReferencesToUIManagers();

        primaries = references.GetTeamUIPrimaries();
        secondaries = references.GetTeamUISecondaries();
        utilities = references.GetTeamUIUtilities();
        moneyText = references.GetTeamUIMoney();

        // subcribe to event to notify when managers have been created
        CreateManagers cm = CreateManagers.GetCreateManagers();
        if (cm == null)
            return;

        cm.OnManagersCreated += OnManagersCreated;
       
        // if theyre already created
        if(cm.GetManagersCreated())
        {
            GetReferenceToPlayerManager();
            GetReferenceToTeamManager();
            SetPlayers();
            UpdateUI();
        }
    }

    private void GetReferenceToPlayerManager()
    {
        playerManager = PlayerManager.GetPlayerManager();

        if (!playerManager)
        {
            Debug.Log("PlayerManager could not be retrieved by PlayerUI");
            return;
        }

        playerManager.OnPlayerAdded += OnPlayerAdded;
    }

    private void GetReferenceToTeamManager()
    {
        teamManager = TeamManager.GetTeamManager();

        if (!teamManager)
        {
            Debug.Log("teamManager could not be retrieved by PlayerUI");
            return;
        }

        teamManager.OnMoneyChange += OnMoneyChanged;

        money = teamManager.GetMoney();
        UpdateMoney();
    }

    private void OnManagersCreated(object sender, System.EventArgs e)
    {
        GetReferenceToPlayerManager();
        GetReferenceToTeamManager();

        SetPlayers();
        UpdateUI();
    }

    public void SetPlayers()
    {
        if(!playerManager)
        {
            Debug.Log("TeamUI does not have reference to player manager");
            return;
        }

        players = playerManager.GetPlayers();
    }

    private void OnPlayerAdded(object sender, System.EventArgs e)
    {
        SetPlayers();
        UpdateUI();
    }

    public void UpdateUI()
    {
        UpdateMoney();
        UpdateItems();
    }

   private void UpdateItems()
   { //probably need to check for localplayer
        UpdatePrimaries();
        UpdateSeconadries();
        UpdateUtilities();

    }

    private void UpdateUtilities()
    {
        for (int i = 0; i < utilities.Count; i++)
            utilities[i].text = "";
    }

    private void UpdateSeconadries()
    {
        for (int i = 0; i < secondaries.Count; i++)
            secondaries[i].text = "";
    }

    private void UpdatePrimaries()
    {
        // get player
        // check if local player
        // if LP move to next player
        // if not - sync with primaries

        int textSet = 0;
        foreach(Player player in players)
        {
            if (player.isLocalPlayer)
                continue;

            // get primary from player
            PlayerCombat pc = player.GetComponent<PlayerCombat>();

            if (!pc)
            {
                primaries[textSet].text = "";
                textSet++;
                continue;
            }

            Weapon wpn = pc.GetWeapon();

            if (!wpn)
            {
                primaries[textSet].text = "";
                textSet++;
                continue;
            }

            string weaponName = wpn.GetWeaponName();
            primaries[textSet].text = weaponName;
            textSet++;
            continue;
        }

        // if we have set 1 that means there are 3 left and we start at 2(1 if 0 indexed)
        if(primaries.Count > textSet)
        {
            for (int i = textSet; i < primaries.Count; i++)
                primaries[i].text = "";
        }

        /*
        for (int i = 0; i < primaries.Count; i++)
        {
            if (i < players.Count)
            {
                // get primary from player
                PlayerCombat pc = players[i].GetComponent<PlayerCombat>();

                if (!pc)
                {
                    primaries[i].text = "";
                    continue;
                }

                Weapon wpn = pc.GetWeapon();

                if (!wpn)
                {
                    primaries[i].text = "";
                    continue;
                }

                string weaponName = wpn.GetWeaponName();
                primaries[i].text = weaponName;
                continue;
            }
            else
            {
                primaries[i].text = "";
            }
            position++;
        }
        */
    }

    private void OnMoneyChanged(object sender, System.EventArgs e)
    {
        money = teamManager.GetMoney();
        UpdateMoney();
    }

    // gets called way to many times need to look at why
    public void UpdateMoney()
    {
        moneyText.text = "CREDITS AVAILABLE\n " + money;
    }

    public void Restart()
    {
        Start();
    }
}
