using UnityEngine;

public class PlayButton : MonoBehaviour
{
    private GameManager gm;
    public UnityEngine.UI.Button playButton;
    // Start is called before the first frame update

    void Start()
    {
        CreateManagers cm = CreateManagers.GetCreateManagers();
        if (cm == null)
            return;

        cm.OnManagersCreated += OnManagersCreated;

        if (cm.GetManagersCreated())
            gm = GameManager.GetGameManager();
    }

    private void OnManagersCreated(object sender, System.EventArgs e)
    {
        gm = GameManager.GetGameManager();
    }

    public void Clicked()
    {
        gm.PlayButtonHit();
    }
}
