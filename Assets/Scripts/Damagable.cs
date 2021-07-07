using Mirror;
using UnityEngine;

public class Damagable : NetworkBehaviour
{
    Health health;
    private bool invulnerable = false;
    private void Start()
    {
        health = gameObject.GetComponent<Health>();
    }

    public void DoDamage(float damage)
    {
        if(invulnerable)
        {
            Debug.Log("TRIED TO DAMAGE BUT INVULN");
            return;
        }

        Debug.Log("Did " + damage + " damage");

        if(!health)
        {
            Debug.Log("Damageable could not get reference to health");
            return;
        }

        health.removeHealth(damage);
    }

    public void setInvulnerable(bool val)
    {
        invulnerable = val;
    }
}
