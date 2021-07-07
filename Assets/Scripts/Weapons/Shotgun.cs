using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    /* shotgun should probably use something like this when not hard coded
    private float coneAngle1 = 30;
    private float coneAngle2 = 15;
    private int pellets = 9;
    private int effectiveRange = 10;
    */

    private float coneRadius = .075f;

    private void Start()
    {
        damage = .72f;
        SHOOT_COOLDOWN = 1;
    }

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

            // center
            ShootRay(rayOrigin, direction);
            float ogX = direction.x;

            // up 1
            direction.y = .075f;
            ShootRay(rayOrigin, direction);

            // up 2
            direction.y = .125f;
            ShootRay(rayOrigin, direction);


            // right 1
            direction.y = 0;
            direction.x = ogX + coneRadius / 2;
            ShootRay(rayOrigin, direction);

            // right 2
            direction.y = 0;
            direction.x = ogX + coneRadius;
            ShootRay(rayOrigin, direction);


            // down 1
            direction.y = -.075f;
            ShootRay(rayOrigin, direction);

            // down 2
            direction.y = -.125f;
            ShootRay(rayOrigin, direction);


            // left 1
            direction.y = 0;
            direction.x = ogX - coneRadius / 2;
            ShootRay(rayOrigin, direction);

            // left 2
            direction.y = 0;
            direction.x = ogX - coneRadius;
            ShootRay(rayOrigin, direction);

        }
    }

    private void ShootRay(Vector3 rayOrigin, Vector3 direction)
    {
        Ray shootRay = new Ray(rayOrigin, direction);
        Debug.DrawRay(rayOrigin, direction * 1000, Color.yellow, 3);
        if (Physics.Raycast(shootRay, out RaycastHit shootRayHitInfo, maxDistance: 1000f))
        {
            if (shootRayHitInfo.collider)
            {
                //Debug.Log(shootRayHitInfo.collider.gameObject.name);
                Damagable damagable = shootRayHitInfo.collider.gameObject.GetComponent(typeof(Damagable)) as Damagable;
                if (damagable)
                    damagable.DoDamage(GetDamage());
            }
        }
    }

}
