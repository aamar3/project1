using UnityEngine;
public interface IWeapon
{
    public void Use(Ray ray, Transform shootPoint);

    public bool CanUse();

    public void SyncTimeNextAvailable(float time);
}
