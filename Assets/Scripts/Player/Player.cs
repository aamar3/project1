using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{
    [SerializeField] Transform playerTransform;
    private PlayerManager playerManager;
    [SerializeField] private Gun gunPrefab = null;
    private GameObject gun = null;
    private StoreManager storeManager;
    private int key;
    private string playerName = "";

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        playerManager = PlayerManager.GetPlayerManager();
        
        playerTransform.position = new Vector3(25, 1, 25);
        if (!playerManager)
        {
            Debug.Log("Player does not have refrecnce to PlayerManager");
            return;
        }
        playerManager.Add(this);

        //if (isServer)
            InstantiateGun();
    }

    private void InstantiateGun()
    {
        // should be able to set position via local position some how
        Gun g = Instantiate(gunPrefab, new Vector3(25, 1.103f, 25.503f), gameObject.transform.rotation);

        gun = g.gameObject;
        gun.transform.parent = gameObject.transform;


        PlayerCombat pc = gameObject.GetComponent<PlayerCombat>();
        pc.SetWeaponPrefab(gun);
        pc.SetShootPoint(gun.GetComponent<Gun>().GetShootPoint());

        NetworkServer.Spawn(gun.gameObject);
    }


    public void Restart()
    {
        playerTransform.position = new Vector3(25, 1, 25);
        InstantiateGun();
    }

    public void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Keyboard.current.rKey.wasReleasedThisFrame)
            RequestRestartGame();

    }

    public Transform GetTransform()
    {
        return playerTransform;
    }

    public void SetPrimary(Weapon wpn)
    {
        if (playerTransform.GetComponent<Weapon>() == null)
        {
            GameObject gameObject = playerTransform.gameObject;
        }
        else
        {
            // destroy component

            // add new component
        }
    }

    public bool GetIsLocalPlayer()
    {
        return isLocalPlayer;
    }

    public GameObject GetGun()
    {
        return gun;
    }

    [Command]
    public void StoreManagerPurchase(int tab, int pos)
    {
        if (storeManager == null)
        {
            storeManager = StoreManager.GetStoreManager();
            if (!storeManager)
            {
                Debug.Log("Player does not have reference to storeManager");
                return;
            }
        }

        storeManager.PurchaseItem(tab, pos, key);
    }

    public void SetKey(int val)
    {
        key = val;
    }

    public int GetKey()
    {
        return key;
    }

    [Command]
    public void PlayButtonHit()
    {
        GameManager gm = GameManager.GetGameManager();
        gm.Play();
    }

    [Command]
    private void RequestRestartGame()
    {
        GameManager.GetGameManager().RestartRequested();
    }

    public void SetName(string name)
    {
        playerName = name;
    }
}
