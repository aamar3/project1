using UnityEngine;

public class FrameRateCap : MonoBehaviour
{
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 240;
    }
}