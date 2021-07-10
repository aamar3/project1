using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    private TeamManager teamManager;
    [SerializeField] private List<Purchaseable> primaryItems = null;
    [SerializeField] private List<Purchaseable> secondaryItems = null;
    [SerializeField] private List<Purchaseable> utilityItems = null;
    private PlayerManager playerManager;
    static StoreManager storeManager;

    private void Start()
    {
        CreateManagers cm = CreateManagers.GetCreateManagers();
        if (cm != null)
        {
            cm.OnManagersCreated += OnManagersCreated;

            if (cm.GetManagersCreated())
            {
                playerManager = PlayerManager.GetPlayerManager();
                teamManager = TeamManager.GetTeamManager();
            }
        }

        SetTabsAndPositions(primaryItems);
        SetTabsAndPositions(secondaryItems);
        SetTabsAndPositions(utilityItems);
    }

    private void OnManagersCreated(object sender, System.EventArgs e)
    {
        playerManager = PlayerManager.GetPlayerManager();
        teamManager = TeamManager.GetTeamManager();
    }

    public static StoreManager GetStoreManager()
    {
        return storeManager;
    }

    private void Awake()
    {
        storeManager = this;
    }

    private Purchaseable GetItem(List<Purchaseable> itemList, int pos)
    {
        Purchaseable item = null;

        foreach (Purchaseable itm in itemList)
            if (itm.position == pos)
                item = itm;

        return item;
    }

    public void PurchaseItem(int tab, int position, int playerIndex)
    {
        // Get item
        Purchaseable item = GetItem(GetItemList(tab), position);
        if(item == null)
        {
            Debug.Log("Could not get reference to item");
            return;
        }

        // check item was actually available

        // subtract money
        if (SpendMoney(item))
        {
            Purchase(item, playerIndex);
        }

        // update ui
        UIManager.GetUIManager().UpdateUI();
    }

    private List<Purchaseable> GetItemList(int tab)
    {
        List<Purchaseable> itemList = null;

        if (tab == 0)
            itemList = primaryItems;
        else if (tab == 1)
            itemList = secondaryItems;
        else if (tab == 2)
            itemList = utilityItems;

        return itemList;
    }

    public void UpdateUI()
    {
        // tell each client to update UI
    }

    private bool SpendMoney(Purchaseable item)
    {
        if (teamManager.GetMoney() >= item.GetBasePrice())
        {
            teamManager.SetMoney(-item.GetBasePrice());
            return true;
        }

        Debug.Log("Tried to purchase item but could not afford");
        return false;
    }

    private void Purchase(Purchaseable item, int player)
    {
        if(!playerManager)
        {
            Debug.Log("StoreManager does not have a reference to PlayerManager");
            return;
        }

        playerManager.PlayerPurchasedPrimary(item, player);
    }

    private void SetTabsAndPositions(List<Purchaseable> itemList)
    {
        int pos = 0;
        foreach (Purchaseable item in itemList)
        {
            item.tab = 0;
            item.position = pos;
            pos++;
        }
    }
}
