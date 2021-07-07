using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PurchaseableSniper : Purchaseable
{
    public override void Purchased(int playerIndex)
    {
        PlayerManager pm = PlayerManager.GetPlayerManager();

        if (!pm)
        {
            Debug.Log("PurchaseableRifle could not get reference to PlayerManager");
            return;
        }

        List<Player> players = pm.GetPlayers();
        Player player = players[playerIndex];

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
       // DestroyImmediate(gun.GetComponent<Weapon>(), true);
        Destroy(gun.GetComponent<Weapon>());

        // add new weapon
        Rifle sniper = gun.AddComponent<Rifle>() as Rifle;

        sniper.SetSniperBaseStats();

        PlayerCombat pc = player.GetComponent<PlayerCombat>();

        if (!pc)
        {
            Debug.Log("Player does not have a player combat script");
            return;
        }


        sniper.SetWeaponName(displayName);
        pc.SetWeapon(sniper);
    }

    [ClientRpc]
    public override void RPCPurchased(int player)
    {
        Purchased(player);
        Debug.Log("Purchased Sniper on client");
    }

}
