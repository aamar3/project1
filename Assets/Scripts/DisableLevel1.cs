using Mirror;
using UnityEngine;

public class DisableLevel1 : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update method of DisableLevel1 called before isServerOnly");

        if (isServerOnly)
        {
            Debug.Log("Update method of DisableLevel1 called after isServerOnly");
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "StoreScreen")
                if(this.gameObject.activeInHierarchy)
                    this.gameObject.SetActive(false);
        }
    }
}
