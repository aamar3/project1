using UnityEngine;

public class HealthBarUIManager : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        health.OnHealthChange += HealthChange;
        healthBar.SetMaxHealth(health.GetMaxHealth());
    }

    private void HealthChange(object sender, System.EventArgs e)
    {
        if (healthBar)
            healthBar.SetHealth(health.GetHealth());
    }
}
