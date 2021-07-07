using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableUI : MonoBehaviour
{
    GameObject ui;
    // Start is called before the first frame update

    public void Start()
    {
        UIManager uiM = UIManager.GetUIManager();
        ui = uiM.gameObject;
    }

    public void Update()
    {
        Disable();
    }

    public void Disable()
    {
        ui.SetActive(false);
    }
}
