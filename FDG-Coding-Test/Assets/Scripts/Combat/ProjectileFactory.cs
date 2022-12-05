using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    [SerializeField] Projectile mProjectilePrefab;      //prefab for default projectile

    //create projectile targeted at a certain direction
    public Projectile CreateNewProjectile(CombatEntity originEntity, Vector3 direction)
    {
        //instantiate projectile
        Projectile newProjectile = Instantiate(mProjectilePrefab, originEntity.transform.position, Quaternion.identity);
        //initiate projectile
        newProjectile.InitiateProjectile(originEntity, direction, originEntity.GetDamage());
        return newProjectile;
    }

    //create projectile targetet at a certain entity
    public Projectile CreateNewProjectile(CombatEntity originEntity, CombatEntity targetEntity)
    {
        //if target is null, cancel (this can happen if an ability coroutine is trying to request a projectile while the enemy is dying)
        if (targetEntity == null)
            return null;
        //difference vector between target entity and origin entity
        Vector3 deltaVector = (targetEntity.transform.position - originEntity.transform.position).normalized;
        //instantiate projectile
        Projectile newProjectile = Instantiate(mProjectilePrefab, originEntity.transform.position, Quaternion.identity);
        //initiate projectile
        newProjectile.InitiateProjectile(originEntity, deltaVector, originEntity.GetDamage());
        return newProjectile;
    }
}
