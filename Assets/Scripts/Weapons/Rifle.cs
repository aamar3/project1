using UnityEngine;

public class Rifle : Weapon
{
    public override void Use(Ray ray, Transform shootPoint)
    {
        Fired();

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 1000f))
        {

            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue, 3);
            var target = hitInfo.point;
            target.y = shootPoint.position.y;
            Vector3 rayOrigin = shootPoint.position;

            Vector3 line = (target - rayOrigin);
            Vector3 direction = line.normalized;

            Ray shootRay = new Ray(rayOrigin, direction);
            Debug.DrawRay(rayOrigin, direction * 1000, Color.yellow, 3);
            if (Physics.Raycast(shootRay, out RaycastHit shootRayHitInfo, maxDistance: 1000f))
            {
                if (shootRayHitInfo.collider)
                {
                    Debug.Log(shootRayHitInfo.collider.gameObject.name);
                    Damagable damagable = shootRayHitInfo.collider.gameObject.GetComponent(typeof(Damagable)) as Damagable;
                    if (damagable)
                        damagable.DoDamage(GetDamage());
                }
            }
        }
    }

    public void SetSniperBaseStats()
    {
        SHOOT_COOLDOWN = 2f;
        damage = 50f;
}

    public void SetAutomaticRifleBaseStats()
    {
        SHOOT_COOLDOWN = .1f;
        damage = 3f;
    }
}