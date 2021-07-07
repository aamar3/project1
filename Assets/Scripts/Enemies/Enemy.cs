using UnityEngine;
using UnityEngine.AI;
using Mirror;
public class Enemy : NetworkBehaviour
{
    [SerializeField] Weapon weapon;
    public Transform target;
    private bool stopMoving = false;
    private NavMeshAgent nma;

    private void Start()
    {
        nma = GetComponent<NavMeshAgent>();
    }
    void Update()
    {

        Player player = FindObjectOfType<Player>();
        if (!player)
        {
            Debug.Log("Enemy cannot get a reference to Player");
            return;
        }

        target = player.transform;

        if (!isServer || !nma)
            return;

        if (stopMoving)
        {
            nma.SetDestination(this.transform.position);
            return;
        }

        Vector3 targetPosition = target.position;
        nma.SetDestination(targetPosition); 
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!isServer)
            return;

        var player = collider.GetComponent<Player>();
        if (player != null)
        {
            Damagable damagable = player.GetComponent(typeof(Damagable)) as Damagable;

            if (!damagable.enabled)
                return;

            if (damagable == null)
            {
                Debug.Log("Enemy could not get Damagable component from player");
                return;
            }
            damagable.DoDamage(weapon.GetDamage());
        }
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void SetStopMoving(bool val)
    {
        stopMoving = val;
    }

}