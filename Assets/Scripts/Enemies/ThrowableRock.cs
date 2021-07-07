using Mirror;
using UnityEngine;

public class ThrowableRock : MonoBehaviour
{

    private bool thrown = false;
    private Vector3 target;
    private readonly float speed = 25;
    private float timeToTravel = 0.0f; // in seconds
    private float timeThrown = 0.0f;
    private Vector3 localPosition;

    public void Start()
    {
        localPosition = transform.localPosition;
    }

    public void Throw(Vector3 targetPosition)
    {
        thrown = true;
        target = targetPosition;
        Vector3 heading = target - this.gameObject.transform.position;
        float distance = heading.magnitude;
        timeToTravel = distance / speed;
        timeThrown = Time.time;
    }

    private void Update()
    {
        if (thrown)
        {
            float t = (Time.time - timeThrown) / timeToTravel;
            
            if (t > 1)
                t = 1;

            TravelToTarget(t);
        }

        if(this.transform.position == target)
        {
            thrown = false;
            ResetPosition();
            this.gameObject.SetActive(false);
        }
    }

    private void TravelToTarget(float t)
    {
        this.transform.position = Vector3.Lerp(this.transform.position, target, t);
    }

    public void ResetPosition()
    {
        transform.localPosition = localPosition;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!this.GetComponentInParent<NetworkIdentity>().isServer)
            return;

        var player = collider.GetComponent<Player>();
        if (player != null)
        {
            Damagable damagable = player.GetComponent(typeof(Damagable)) as Damagable;

            if (damagable == null)
            {
                Debug.Log("Throwable rock does not have reference to Damagable from player");
                return;
            }

            if (!damagable.enabled)
                return;

            damagable.DoDamage(10);
        }
    }
}
