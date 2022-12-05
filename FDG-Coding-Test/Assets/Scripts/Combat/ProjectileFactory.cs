using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    [SerializeField] Projectile mProjectilePrefab;

    public Projectile CreateNewProjectile(CombatEntity originEntity, Vector3 direction)
    {
        //instantiate projectile
        Projectile newProjectile = Instantiate(mProjectilePrefab, originEntity.transform.position, Quaternion.identity);
        newProjectile.InitiateProjectile(originEntity, direction, originEntity.GetDamage());
        return newProjectile;
    }

    public Projectile CreateNewProjectile(CombatEntity originEntity, CombatEntity targetEntity)
    {
        //delta
        if (targetEntity == null)
            return null;
        Vector3 deltaVector = (targetEntity.transform.position - originEntity.transform.position).normalized;
        //instantiate projectile
        Projectile newProjectile = Instantiate(mProjectilePrefab, originEntity.transform.position, Quaternion.identity);
        newProjectile.InitiateProjectile(originEntity, deltaVector, originEntity.GetDamage());
        return newProjectile;
    }
}
