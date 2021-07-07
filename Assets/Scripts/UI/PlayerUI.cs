using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    private Text primary;
    private Text utility;
    private Text secondary;
    private HealthBar healthBar = null;
    private string primaryText = null;
    private string secondaryText = null;
    private string utilityText = null;
    private Health health = null;
    static PlayerUI playerUI = null;


    private void Awake()
    {
        playerUI = this;
        primaryText = "";
    }

    public static PlayerUI GetPlayerUI()
    {
        return playerUI;
    }

    private void Start()
    {
        PassUIReferencesToUIManagers references = PassUIReferencesToUIManagers.GetPassUIReferencesToUIManagers();

        primary = references.GetPlayerUIPrimary();
        secondary = references.GetSecondaryUIPrimary();
        utility = references.GetPlayerUIUtilities();

        SceneManager.sceneLoaded += OnSceneLoaded;

        CreateManagers cm = CreateManagers.GetCreateManagers();

        if (cm != null)
        {
            if (cm.GetManagersCreated())
            {
                PlayerManager playerManager = PlayerManager.GetPlayerManager();

                if (!playerManager)
                {
                    Debug.Log("PlayerManager could not be retrieved by PlayerUI");
                    return;
                }

                playerManager.OnPlayerAdded += OnPlayerAdded;
                PlayerAdded();
            }
            else
                cm.OnManagersCreated += OnManagersCreated;
        }

        UpdateUI();
    }

    private void OnManagersCreated(object sender, System.EventArgs e)
    {
        PlayerAdded();
    }

    private void PlayerAdded()
    {
        if (health)
            return;

        PlayerManager playerManager = PlayerManager.GetPlayerManager();

        if (!playerManager)
        {
            Debug.Log("PlayerManager could not be retrieved by PlayerUI");
            return;
        }

        Player player = playerManager.GetLocalPlayer();

        if (!player)
        {
            Debug.Log("player could not be retrieved by PlayerUI");
            return;
        }

        health = player.GetComponent<Health>();
        health.OnHealthChange += HealthChange;

        if (!health)
        {
            Debug.Log("health could not be retrieved by PlayerUI");
            return;
        }
    }

    private void OnPlayerAdded(object sender, System.EventArgs e)
    {
        PlayerAdded();
        UpdateUI();
    }

    private void HealthChange(object sender, System.EventArgs e)
    {
        if (healthBar)
            healthBar.SetHealth(health.GetHealth());
    }

    public void UpdateUI()
    {


        // get primary from player
        PlayerManager pm = PlayerManager.GetPlayerManager();
        if (pm != null)
        {
            Player player = pm.GetLocalPlayer();
            if (player != null)
            {
                PlayerCombat pc = player.GetComponent<PlayerCombat>();

                if (!pc)
                    primary.text = "";
                else
                {
                    Weapon wpn = pc.GetWeapon();

                    if (!wpn)
                        primary.text = "";
                    else
                    {
                        string weaponName = wpn.GetWeaponName();
                        primary.text = weaponName;
                    }
                }
            }
        }
        else
        {
            primary.text = "";
        }



        secondary.text = secondaryText;
        utility.text = utilityText;

        if(healthBar)
            healthBar.SetHealth(health.GetHealth());
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1")
        {
            GameObject phb = GameObject.Find("PlayerHealthBar");

            if (!phb)
            {
                Debug.Log("PlayerHealthBar could not be found by PlayerUI");
                return;
            }

            GameObject playerGameObject = phb.gameObject;
            healthBar = playerGameObject.GetComponent<HealthBar>();
            healthBar.SetMaxHealth(health.GetMaxHealth());
        }
    }

    public void Restart()
    {
        Start();
    }
}