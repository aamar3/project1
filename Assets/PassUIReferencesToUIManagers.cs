using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassUIReferencesToUIManagers : MonoBehaviour
{
    [SerializeField] private Text primary;
    [SerializeField] private Text secondary;
    [SerializeField] private Text utility;
    [SerializeField] private List<Text> primaries;
    [SerializeField] private List<Text> secondaries;
    [SerializeField] private List<Text> utilities;
    [SerializeField] private Text money;
    private static PassUIReferencesToUIManagers references = null;
    // Start is called before the first frame update

    private void Awake()
    {
        references = this;
    }

    public static PassUIReferencesToUIManagers GetPassUIReferencesToUIManagers()
    {
        return references;
    }

    public List<Text> GetTeamUIPrimaries()
    {
        return primaries;
    }

    public List<Text> GetTeamUISecondaries()
    {
        return secondaries;
    }

    public List<Text> GetTeamUIUtilities()
    {
        return utilities;
    }

    public Text GetTeamUIMoney()
    {
        return money;
    }

    public Text GetPlayerUIPrimary()
    {
        return primary;
    }
    public Text GetSecondaryUIPrimary()
    {
        return secondary;
    }
    public Text GetPlayerUIUtilities()
    {
        return utility;
    }

    void Start()
    {

    }
}
