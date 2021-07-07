using UnityEngine.AI;
using UnityEngine;

public class ThrowRock : MonoBehaviour
{
    protected float timeNextAvailable;
    private Transform target;
    [SerializeField] private float range;
    [SerializeField] private float THROW_COOLDOWN;
    private Enemy enemy;
    [SerializeField] private float throwDelay;
    private float throwTime;
    private bool throwStarted;
    private GameObject rockGO;
    private ThrowableRock rock;

    private void Start()
    {
        ThrowableRock[] rocks = this.GetComponentsInChildren<ThrowableRock>();
        if (rocks.Length > 0)
            rock = rocks[0];
        else
        {
            Debug.Log("ThrowRock could not find ThrowableRocks");
            return;
        }

        rockGO = rock.gameObject;

        enemy = this.GetComponent<Enemy>();
        if(!enemy)
            Debug.Log("ThrowRock could not get reference to enemy");

        rockGO.SetActive(false);
    }

    private bool CanThrow()
    {
        if (timeNextAvailable < Time.time)
            return true;
        else
            return false;
    }

    public void AquireTarget()
    {
        if(!enemy)
        {
            target = null;
            return;
        }

        target = enemy.GetTarget();
    }

    void Update()
    {
        AquireTarget();

        if (!target)
        {
            Debug.Log("ThrowRock could not aquire target");
            return;
        }

        Vector3 heading = target.position - this.gameObject.transform.position;
        float distance = heading.magnitude;

        if (target && CanThrow() && !throwStarted && distance <= range)
            EnableRock();

        if (throwTime < Time.time && throwStarted )
            Throw();
    }

    private void EnableRock()
    {
        throwTime = Time.time + throwDelay;
        enemy.SetStopMoving(true);
        rockGO.SetActive(true);
        throwStarted = true;
    }

    private void Throw()
    {
        if(!rock)
        {
            Debug.Log("ThrowableRock component not found");
            return;
        }

        rock.Throw(target.position);
        throwStarted = false;
        enemy.SetStopMoving(false);
        timeNextAvailable = THROW_COOLDOWN + Time.time;
    }
}
