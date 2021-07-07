using UnityEngine.SceneManagement;
using UnityEngine;
using Mirror;

public class SceneObjectManager : NetworkBehaviour
{
    private const string LEVEL1 = "Level1";
    private const string STORE = "StoreScreen - Copy";

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += SceneChanged;
    }

    // Update is called once per frame
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void SceneChanged(Scene current, Scene Next)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case LEVEL1:
                SpawnLevel1();
                break;
            case STORE:
                SpawnStore();
                break;
            default:
                break;
        }

    }

    private void SpawnLevel1()
    {
        Debug.Log("SpawnLevel1 called");
    }

    private void SpawnStore()
    {
        Debug.Log("SpawnStore called");
    }
}
