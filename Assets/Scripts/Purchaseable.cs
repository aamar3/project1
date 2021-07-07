using Mirror;
using UnityEngine;

public class Purchaseable : NetworkBehaviour
{

    [SerializeField] private int basePrice;
    [SerializeField] protected string displayName;
    [SerializeField] private string description;
    [SerializeField] private string type;
    [SerializeField] private Sprite image;
    [SerializeField] private int tab;
    [SerializeField] private int position;
    private int purchaseableIndex;

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

    public virtual void Purchased(int player)
    {
        return;
    }

    [ClientRpc]
    public virtual void RPCPurchased(int player)
    {
        Purchased(player);
        Debug.Log("Purchased on client");
    }

    public void SetPosition(int pos)
    {
        position = pos;
    }

    public int GetPosition()
    {
        return position;
    }

    public void SetTab(int val)
    {
        tab = val;
    }

    public int GetTab()
    { 
        return tab;
    }

    public void SetPurchaseableIndex(int val)
    {
        purchaseableIndex = val;
    }

    public int GetPurchaseableIndex()
    {
        return purchaseableIndex;
    }

}
