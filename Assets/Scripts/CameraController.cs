using Cinemachine;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class CameraController : NetworkBehaviour
{
    private Transform player = null;
    [SerializeField] private CinemachineVirtualCamera cam1;
    [SerializeField]  private CinemachineVirtualCamera cam2;

    private void Start()
    {

    }

 

    private void OnPlayerAdded(object sender, System.EventArgs e)
    {
        PlayerManager playerManager = PlayerManager.GetPlayerManager();
        cam1.Follow = playerManager.GetLocalPlayer().gameObject.transform;
    }


}
