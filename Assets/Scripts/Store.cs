using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : NetworkBehaviour
{
    [SerializeField] private List<Purchaseable> primaryItems = null;
    [SerializeField] private List<Purchaseable> purchaseables = null;
    [SerializeField] private List<Purchaseable> secondaryItems = null;
    [SerializeField] private List<Purchaseable> utilityItems = null;
    [SerializeField] private List<GameObject> storeItemBox = null;
    [SerializeField] private Text descriptionBox = null;
    [SerializeField] private Text displayPrice = null;
    /* [SerializeField]*/ private Text primary = null;
    /* [SerializeField]*/ private Text secondary = null;
    /* [SerializeField]*/ private Text utility = null;
    private TeamManager teamManager = null;
    private int selectedTab;
    private Purchaseable selectedItem;
    private PlayerManager playerManager;
    private static Store store;
    private Player localPlayer;

    private void Awake()
    {
        store = this;
    }
    private void Start()
    {
        if(isServer)
        {
            teamManager = TeamManager.GetTeamManager();
        }
        SetTabsAndPositions(primaryItems);
      //  SetTabsAndPositions(secondaryItems);
      //  SetTabsAndPositions(utilityItems);
        playerManager = PlayerManager.GetPlayerManager();
        if(playerManager != null)
            localPlayer = playerManager.GetLocalPlayer();

        GameManager gm = GameManager.GetGameManager();
        SetPrimaryUI();
        SetSecondary();
        SetUtility();
        LoadPrimaryPage();

        int count = 0;
        foreach(Purchaseable purchaseable in purchaseables)
        {
            purchaseable.SetPurchaseableIndex(count);
            count++;
        }
    }

    private void LoadPrimaryPage()
    {
        int itemsAdded = 0;

        if (primaryItems == null)
            return;

        if (storeItemBox == null)
            return;

        if (primaryItems.Count > 0)
        {
            foreach (Purchaseable item in primaryItems)
            {
                if (itemsAdded >= storeItemBox.Count)
                    break;

                if (item.GetPurchaseableType() == "Primary")
                {

                    Image image = storeItemBox[itemsAdded].GetComponent<Image>(); // add it into store box

                    if (image != null)
                    {
                        image.sprite = item.GetImage();
                        itemsAdded++;
                    }
                }
            }
        }
        else
        {
            foreach (GameObject itemBox in storeItemBox)
            {
                Image image = itemBox.GetComponent<Image>();

                if (!image)
                    return;

                image.sprite = null;
            }
        }

        selectedTab = 0;
        if (primaryItems.Count > 0)
            selectedItem = primaryItems[0];
        else
            selectedItem = null;

        ItemSelected();
    }

    private void LoadSecondaryPage()
    {
        int itemsAdded = 0;

        if (secondaryItems.Count > 0)
        {
            foreach (Purchaseable item in secondaryItems)
            {
                if (itemsAdded >= storeItemBox.Count)
                    break;

                if (item.GetPurchaseableType() == "Secondary")
                {
                    Image image = storeItemBox[itemsAdded].GetComponent<Image>(); // add it into store box

                    if (image != null)
                    {
                        image.sprite = item.GetImage();
                        itemsAdded++;
                    }
                }
            }
        }
        else
        {
            foreach( GameObject itemBox in storeItemBox)
            {
                Image image = itemBox.GetComponent<Image>();
                image.sprite = null;
            }
        }

        selectedTab = 1;
        if (secondaryItems.Count > 0)
            selectedItem = secondaryItems[0];
        else
            selectedItem = null;

        ItemSelected();
    }

    private void LoadUtilityPage()
    {
        int itemsAdded = 0;

        if (utilityItems.Count > 0)
        {
            foreach (Purchaseable item in utilityItems)
            {
                if (itemsAdded >= storeItemBox.Count)
                    break;

                if (item.GetPurchaseableType() == "Utility")
                {
                    Image image = storeItemBox[itemsAdded].GetComponent<Image>(); // add it into store box

                    if (image != null)
                    {
                        image.sprite = item.GetImage();
                        itemsAdded++;
                    }
                }
            }
        }
        else
        {
            foreach (GameObject itemBox in storeItemBox)
            {
                Image image = itemBox.GetComponent<Image>();
                image.sprite = null;
            }
        }

        selectedTab = 1;
        if (utilityItems.Count > 0)
            selectedItem = utilityItems[0];
        else
            selectedItem = null;

        ItemSelected();
    }

    public void TabSelected(int index)
    {
        if (index == 0)
            LoadPrimaryPage();
        else if (index == 1)
            LoadSecondaryPage();
        else
            LoadUtilityPage();

       // ItemSelected(0);
    }

    private void PrintDescription()
    {
        if(descriptionBox == null)
        {
            Debug.Log("Store does not have reference to descriptionBox");
            return;
        }

        // here is where we display the description
        if(selectedItem != null)
            descriptionBox.text = selectedItem.GetDescription();
        else
            descriptionBox.text = "";

    }

    private void SetDisplayPrice()
    {
        if(displayPrice == null)
        {
            Debug.Log("Store does not have reference to displayPrice. (Store L211)");
            return;
        }

        if (selectedItem != null)
            displayPrice.text = selectedItem.GetBasePrice().ToString() + " CREDITS";
        else
            displayPrice.text = "";

    }

    public void ItemSelected(int index)
    {
        if (selectedTab == 0)
        {
            if (primaryItems.Count > index)
            {
                selectedItem = primaryItems[index];
                ItemSelected();
                return;
            }
        }
        else if (selectedTab == 1)
        {
            if (secondaryItems.Count > index)
            {
                selectedItem = secondaryItems[index];
                ItemSelected();
                return;
            }
        }
        else
        {
            if (utilityItems.Count > index)
            {
                selectedItem = utilityItems[index];
                ItemSelected();
                return;
            }
        }

        selectedItem = null;
        ItemSelected();
    }

    public void ItemSelected()
    {
        PrintDescription();
        SetDisplayPrice();
    }

    public void PurchaseItem()
    {
        if(!localPlayer)
        {
            if(!playerManager)
            {
                Debug.Log("Store does not have reference to PlayManager");
                return;
            }

            localPlayer = playerManager.GetLocalPlayer();

            if(!localPlayer)
            {
                Debug.Log("Store does not have a reference to the player");
                return;
            }
        }

        if(selectedItem == null)
        {
            Debug.Log("Selected item null");
            return;
        }

        localPlayer.StoreManagerPurchase(selectedItem.GetTab(),selectedItem.GetPosition());
    }

    private void SetPrimaryWeapon()
    {
        if (!selectedItem)
            return;

        selectedItem.Purchased(localPlayer.GetIndex());
    }

    private void SetPrimaryUI()
    {
        if (!primary)
            return;

        if(selectedItem == null)
        {
            primary.text = "";
            return;
        }

        primary.text = selectedItem.GetDispalyName();
        playerManager.SetPrimary(selectedItem.GetComponent<Weapon>());
    }

    private void SetSecondary()
    {
        if (!secondary)
            return;

        if (selectedItem == null)
        {
            secondary.text = "";
            return;
        }

        secondary.text = selectedItem.GetDispalyName();
    }

    private void SetUtility()
    {
        if (!utility)
            return;

        if (selectedItem == null)
        {
            utility.text = "";
            return;
        }

        utility.text = selectedItem.GetDispalyName();
    }

    public static Store GetStore()
    {
        return store;
    }

    public Purchaseable GetPurchaseable(int index)
    {
        return purchaseables[index];
    }

    private void SetTabsAndPositions(List<Purchaseable> itemList)
    {
        int pos = 0;
        foreach (Purchaseable item in itemList)
        {
            item.SetTab(0);
            item.SetPosition(pos);
            pos++;
        }
    }
}
