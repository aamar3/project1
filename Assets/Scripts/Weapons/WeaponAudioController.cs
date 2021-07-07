using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudioController : MonoBehaviour
{

    [SerializeField] private PoolableAudio sound;
    [SerializeField] private Transform parent;
    private Queue<PoolableAudio> soundQueue;

    void Start()
    {
        soundQueue = new Queue<PoolableAudio>();
    }

    public void PlayFire()
    {
        var poolableAudio = GetPoolableAudio();
        poolableAudio.SetController(this);
        poolableAudio.Play();
    }

    private PoolableAudio GetPoolableAudio()
    {
        if (soundQueue.Count > 0)
        {
            var poolableAudio = soundQueue.Dequeue();
            poolableAudio.gameObject.SetActive(true);
            return poolableAudio;
        }
        else
        {
            var audioSource = Instantiate(sound, parent.position,parent.rotation);
            return audioSource;
        }
    }

    public void AddToQueue(PoolableAudio source)
    {
        soundQueue.Enqueue(source);
    }
}
