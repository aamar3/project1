using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using UnityEngine.SceneManagement;
public class PlayerCombat : NetworkBehaviour
{
    private Weapon weapon;
    private LineRenderer laserPointer;
    private GameObject weaponPrefab;
    private Transform shootPoint;
    private WeaponAudioController audioController;

    public void SetWeaponPrefab(GameObject wpn)
    {
        weaponPrefab = wpn;
    }
    public void SetShootPoint(Transform transform)
    {
        shootPoint = transform;
    }

   public void Start()
    {
        if (isServerOnly)
            return;

        if (!isLocalPlayer)
            return;

        if(weaponPrefab)
        {
            laserPointer = weaponPrefab.GetComponent<LineRenderer>();
            laserPointer.SetPosition(0, shootPoint.position);
            laserPointer.SetPosition(1, new Vector3(shootPoint.position.x, shootPoint.position.y, 600));

            audioController = weaponPrefab.GetComponent<WeaponAudioController>();
            
            weapon = weaponPrefab.GetComponent<Weapon>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Level1")
            return;

        if (!isLocalPlayer)
            return;

        if (weapon != null)
            CheckForFire();
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        if(laserPointer)
            DoLaserPointer();
    }

    private void CheckForFire()
    {
        // if the right mouse button is down and (locally corrected can use is true || SyncedCanUseIsTrue)
        // request to fire


        if (Mouse.current.rightButton.isPressed && weapon.CanUse())
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            ClientAuthFire(ray);

           // RequestFire(ray); Will be used when server auth implemented
           ClientFire();
        }
    }

    [Command]
    private void ClientAuthFire(Ray ray)
    {
        ServerFire(ray);
        CRPCFire();
    }

    // request from server to fire
    [Command]
    private void RequestFire(Ray ray)
    {
        if (weapon.CanUse())
        {
            ServerFire(ray);
            CRPCFire();
        }
        else
            Debug.Log("RequestFire called but CanUse = false");
    }

    [ClientRpc]
    void CRPCFire()
    {
        if (isLocalPlayer)
        {
            Debug.Log("CRPCFire Called on LocalPlayer");
            return;
        }
        else
            Debug.Log("Bang Not Self");

        if(audioController == null)
        {
            Debug.Log("Attempting to get audio controller");

            audioController = weaponPrefab.GetComponent<WeaponAudioController>();

            if (audioController == null)
            {
                Debug.Log("Could not get audioController");
                return;
            }
            else
            {
                Debug.Log("retrieved audioController");
                audioController.PlayFire();
            }
        }
        audioController.PlayFire();
    }


    private void ServerFire(Ray ray)
    {
        if (!isServer) // probably not needed since called from command?
            return;

        Debug.Log("Command PlayerCombat.ServerFire called");
        weapon.Use(ray, shootPoint);
    }

    private void ClientFire()
    {
        if (isLocalPlayer)
            Debug.Log("BANG Self");
        else
        {
            Debug.Log("Bang Not Self");
            return;
        }

        weapon.Fired();
        audioController.PlayFire();
    }

    private void DoLaserPointer()
    {
        int layerMask = 1 << 6;
        layerMask = ~layerMask;
        Vector3 screenPoint = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 1000f, layerMask))
        {
            var target = hitInfo.point;
            laserPointer.SetPosition(0, shootPoint.position);
            laserPointer.SetPosition(1, target);
        }
    }

    public void SetWeapon(Weapon wpn)
    {
        weapon = wpn;
    }

    public Weapon GetWeapon()
    {
        return weapon;
    }    
}
