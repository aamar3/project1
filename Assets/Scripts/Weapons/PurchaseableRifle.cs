using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PurchaseableRifle : Purchaseable
{
    public override void Purchased(int playerKey)
    {
        PlayerManager pm = PlayerManager.GetPlayerManager();
        
        if(!pm)
        {
            Debug.Log("PurchaseableRifle could not get reference to PlayerManager");
            return;
        }

        Player player = pm.GetPlayer(playerKey);

        if (!player)
        {
            Debug.Log("Could not get reference to player");
            return;
        }

        // get gun component
        GameObject gun = player.GetGun();

        if (!gun)
        {
            Debug.Log("Can not add weapon script because gun is null");
            return;
        }

        // remove old weapon
        Destroy(gun.GetComponent<Weapon>());

        // add new weapon
        Rifle rifle = gun.AddComponent<Rifle>() as Rifle;

        rifle.SetAutomaticRifleBaseStats();

        PlayerCombat pc = player.GetComponent<PlayerCombat>();
        
        if (!pc)
        {
            Debug.Log("Player does not have a player combat script");
            return;
        }

        rifle.SetWeaponName(displayName);
        pc.SetWeapon(rifle);
    }

    [ClientRpc]
    public override void RPCPurchased(int player)
    {
        Purchased(player);
        Debug.Log("Purchased on client");
    }

}
