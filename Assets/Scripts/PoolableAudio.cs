using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableAudio : MonoBehaviour
{
    private WeaponAudioController controller;
    [SerializeField] private AudioSource source;

    // Update is called once per frame
    void Update()
    {
        if (source.gameObject.activeSelf)
        {
            if (!source.isPlaying)
            {
                source.gameObject.SetActive(false);
                ReturnToQueue();
            }
        }
    }

    private void ReturnToQueue()
    {
        controller?.AddToQueue(this);
    }

    public void Play()
    {
        source.Play();
    }

    public void SetController(WeaponAudioController ctrl)
    {
        controller = ctrl;
    }
}
