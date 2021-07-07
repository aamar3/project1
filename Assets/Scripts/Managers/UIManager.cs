using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : NetworkBehaviour
{
    static UIManager ui = null;
    private PlayerUI playerUI = null;
    private TeamUI teamUI = null;

    private void Awake()
    {
        ui = this;
    }

    public static UIManager GetUIManager()
    {
        return ui;
    }

    private void Start()
    {
        playerUI = PlayerUI.GetPlayerUI();
        teamUI = TeamUI.GetTeamUI();
    }

    public void UpdateUI()
    {
        playerUI.UpdateUI();
        teamUI.UpdateUI();
    }

    [ClientRpc]
    public void RPCUpdateUI()
    {
        UpdateUI();
        Debug.Log("UIManager.RPCUpdateUI Called");
    }

    private void OnEnable()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Level1")
        {
            this.gameObject.SetActive(false);
        }
    }

    public void Restart()
    {
        teamUI.Restart();
        playerUI.Restart();
    }
}
