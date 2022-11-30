using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    [SerializeField] static Projectile mProjectilePrefab;

    public static Projectile CreateNewProjectile(CombatEntity originEntity, Vector3 direction)
    {
        //instantiate projectile
        Projectile newProjectile = Instantiate(mProjectilePrefab, originEntity.transform.position, Quaternion.identity);
        newProjectile.InitiateProjectile(originEntity, direction, originEntity.mDamage);
        return newProjectile;
    }

    public static Projectile CreateNewProjectile(CombatEntity originEntity, CombatEntity targetEntity)
    {
        //delta
        Vector3 deltaVector = (targetEntity.transform.position - originEntity.transform.position).normalized;
        //instantiate projectile
        Projectile newProjectile = Instantiate(mProjectilePrefab, originEntity.transform.position, Quaternion.identity);
        newProjectile.InitiateProjectile(originEntity, deltaVector, originEntity.mDamage);
        return newProjectile;
    }
}
