using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class LocalUIManager : NetworkBehaviour
{
    private TeamManager tm;
    [SerializeField] private Text moneyText;

    // [SyncVar]
    private float money;

    private void Awake()
    {
        //DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        if(isServer)
        {
            tm = TeamManager.GetTeamManager();

            if (!tm)
                return;

            money = tm.GetMoney();
            UpdateUI();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateUI()
    {
        UpdateMoney();
    }

    public void UpdateMoney()
    {
        moneyText.text = "CREDITS AVAILABLE\n " + money;
    }
}
