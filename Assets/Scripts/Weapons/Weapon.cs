using Mirror;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected float damage;
    private double timeNextAvailable;
    [SerializeField] protected double SHOOT_COOLDOWN;
    private string weaponName = "";

    void Start()
    {
        timeNextAvailable = NetworkTime.time;
    }

    public virtual float GetDamage()
    {
        return damage;
    }

    public bool CanUse()
    {
        if (NetworkTime.time > timeNextAvailable)
            return true;
        else
            return false;
    }

    public virtual void Use(Ray ray, Transform shootPoint) { Debug.Log("weapon.use was called for some reason"); }

    public void SetWeaponName(string name)
    {
        weaponName = name;
    }

    public string GetWeaponName()
    { 
        return weaponName;
    }

    public virtual void Fired()
    {
        timeNextAvailable = NetworkTime.time + SHOOT_COOLDOWN;
    }
}