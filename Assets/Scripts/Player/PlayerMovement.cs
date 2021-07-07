using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]

    [Header("Settings")]
    [SerializeField] private float MOVEMENT_SPEED = 5f;
    [SerializeField] private float DODGE_FRAMES = 75f;
    [SerializeField] private float DODGE_COOLDOWN = .75f;
    [SerializeField] private float DODGE_SPEED = 10;
    [SerializeField] private Damagable damagable;
    private CameraManager cameraManager = null;
    Vector3 forward, right;
    private bool isDodging = false;
    private float dodgeFrameCount;
    private float timeDodgeAvailable;
    private bool lookAtMouse = false;

    /*
    private class Tappable
    {
        private float tappableWindow = -1;
        private const float tapRange = 1f;
        private bool tapped = false;
        private bool pressed = false;
        private bool pressedPrev = false;

        public void Pressed(bool val)
        {
            if (tapped)
                tapped = false;

            pressed = val;

            if (pressed)
                Debug.Log("PRESSED");

            if(!pressed && pressedPrev)
            {
                tappableWindow = Time.time + tapRange;
                Debug.Log("RELEASED");
            }
            else if (pressed && !pressedPrev && tappableWindow < Time.time)
            {
                Debug.Log("TAPPED TO SLOW");
            }
            else if (pressed && !pressedPrev && tappableWindow > Time.time)
            {
                tapped = true;
                tappableWindow = -1;
            }

            pressedPrev = pressed;
        }

        public bool getTapped()
        {
            return tapped;
        }
    }
    */

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1")
            lookAtMouse = true;
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (!isLocalPlayer)
            return;

        SetControlDirection();

        timeDodgeAvailable = Time.time;
        dodgeFrameCount = 0;

        cameraManager = CameraManager.GetCameraManager();
        cameraManager.OnCameraFlip += CamFlipped;

    }

    private void CamFlipped(object sender, System.EventArgs e)
    {
        SetControlDirection();
    }

    [ClientCallback]
    private void Update()
    {
        if (!isLocalPlayer) { return; }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) { return; }

        if(lookAtMouse)
            LookAtMouse();

        DoMovementStuff();

    }

    private void LookAtMouse()
    {
        int layerMask = 1 << 6;
        layerMask = ~layerMask;
        Vector3 screenPoint = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(screenPoint); 
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 1000f,layerMask))
        {
            var target = hitInfo.point;
            if (target.y < 1)
                target.y = 1;

            transform.LookAt(target);
        }
    }

    private void DoMovementStuff()
    {
        int upVal = 0;
        int rightVal = 0;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.sKey.isPressed)
            upVal = System.Convert.ToInt16(Keyboard.current.wKey.isPressed) - System.Convert.ToInt16(Keyboard.current.sKey.isPressed);

        if (Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed)
            rightVal = System.Convert.ToInt16(Keyboard.current.dKey.isPressed) - System.Convert.ToInt16(Keyboard.current.aKey.isPressed);

        bool roll = Keyboard.current.spaceKey.isPressed && !isDodging && timeDodgeAvailable < Time.time;

        if(roll)
            Dodge();

        Vector3 rightMovement = right;
        Vector3 upMovement = forward;

        //    if (cameraManager.IsFlipped())
        //      {
        //         rightMovement = -right;
        //         upMovement = -forward;
        //       }

        if (isDodging)
        {
            damagable.setInvulnerable(true);
            if (dodgeFrameCount > DODGE_FRAMES)
            {
                isDodging = false;
                dodgeFrameCount = 0;
                damagable.setInvulnerable(false);
            }
            else
                dodgeFrameCount++;
        }

        if (isDodging)
            {
                rightMovement = right * MOVEMENT_SPEED * Time.deltaTime * rightVal * MOVEMENT_SPEED;
                upMovement = forward * MOVEMENT_SPEED * Time.deltaTime * upVal * MOVEMENT_SPEED;
                Debug.Log("Dodging");
            }
            else
            {
                rightMovement = right * MOVEMENT_SPEED * Time.deltaTime * rightVal;
                upMovement = forward * MOVEMENT_SPEED * Time.deltaTime * upVal;
            }

        transform.position += rightMovement;
        transform.position += upMovement; 
    }

    private void Dodge()
    {
        if (timeDodgeAvailable < Time.time)
        {
            timeDodgeAvailable = Time.time + DODGE_COOLDOWN;
            isDodging = true;
            Debug.Log("Dodge called");
        }
    }

    public bool GetIsLocalPlayer()
    {
        return isLocalPlayer;
    }

    private void SetControlDirection()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }
}
