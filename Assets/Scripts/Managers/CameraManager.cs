using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

// having this on the local player will make flipping hackable but were doing it this way
// for testing

enum CameraState {READY, TRANSITIONING };

public class CameraManager : MonoBehaviour
{


    private const string LEVEL1 = "Level1";
    private static CameraManager cm;
    [SerializeField] private CinemachineBrain cmb;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineVirtualCamera cam1;
    [SerializeField] private CinemachineVirtualCamera cam2;
    CameraState camState = CameraState.READY;

    private float flipTime = .5f;
    private float nextFlipTime = 0;

    public event EventHandler OnCameraFlip;

    // Start is called before the first frame update

    void Awake()
    {
        cm = this;
    }

    public static CameraManager GetCameraManager()
    {
        return cm;
    }

    void Start()
    {
        SceneManager.activeSceneChanged += SceneChanged;
        mainCamera.gameObject.SetActive(true);
    }

    void Update()
    {
        switch(camState)
        {
            case CameraState.READY:
                if (Keyboard.current.fKey.isPressed && nextFlipTime <= Time.time)
                {
                    FlipTheScreen();
                    nextFlipTime = Time.time + flipTime;
                    camState = CameraState.TRANSITIONING;
                }
                break;
            case CameraState.TRANSITIONING:
                if (nextFlipTime < Time.time)
                {
                    camState = CameraState.READY;
                    OnCameraFlip?.Invoke(this, EventArgs.Empty);
                }
                break;
        }
    }

    private void FlipTheScreen()
    {
        if (cam1.isActiveAndEnabled)
        {
            cam1.gameObject.SetActive(false);
            cam2.gameObject.SetActive(true);
        }
        else
        {
            cam1.gameObject.SetActive(true);
            cam2.gameObject.SetActive(false);
        }
    }

    private void SceneChanged(Scene current, Scene Next)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case LEVEL1:
                AssignCamera();
                break;
            default:
                break;
        }
    }

    private void AssignCamera()
    {
        PlayerManager pm = PlayerManager.GetPlayerManager();
        Player player = pm.GetLocalPlayer();

        cam1.Follow = player.gameObject.transform;
        cam2.Follow = player.gameObject.transform;
        cam1.gameObject.SetActive(false);
    }
}
