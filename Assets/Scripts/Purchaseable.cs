using Mirror;
using UnityEngine;

public class Purchaseable : NetworkBehaviour
{

    [SerializeField] private int basePrice;
    [SerializeField] protected string displayName;
    [SerializeField] private string description;
    [SerializeField] private string type;
    [SerializeField] private Sprite image;
    [SerializeField] public int tab { get; set; }
    [SerializeField] public int position { get; set; }
    public int index { get; set; }

    public int GetBasePrice()
    {
        return basePrice;
    }

    public string GetDispalyName()
    {
        return displayName;
    }

    public string GetDescription()
    {
        return description;
    }

    public string GetPurchaseableType()
    {
        return type;
    }

    public Sprite GetImage()
    {
        return image;
    }

    public virtual void Purchased(int playerKey)
    {
        return;
    }

    [ClientRpc]
    public virtual void RPCPurchased(int player)
    {
        Purchased(player);
        Debug.Log("Purchased on client");
    }

    public Player GetPlayer(int playerKey)
    {
        return PlayerManager.GetPlayerManager().GetPlayer(playerKey);
    }
}
