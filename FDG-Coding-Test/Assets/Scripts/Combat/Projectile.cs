using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float mMoveSpeed;              //projectile move speed
    [SerializeField] float mMaxLifeTime;            //maximum life time before self destruct
    [SerializeField] float mTerrainLingerTime;      //how long to stick in terrain after hit
    CombatEntity mOwner;                            //skill owner
    int mDamage;                                    //damage on hit
    Rigidbody mRigidRef;                            //reference to rigidbody
    bool mIgnoreCollisions;                         //whether to ignore collisions

    //set up projectile
    public virtual void InitiateProjectile(CombatEntity owner, Vector3 direction, int damage)
    {
        //set owner and damage
        mOwner = owner;
        mDamage = damage;
        //set movement
        mRigidRef = GetComponent<Rigidbody>();
        mRigidRef.velocity = direction.normalized * mMoveSpeed;
        //prevent projectile from hitting the owner
        Physics.IgnoreCollision(GetComponent<Collider>(), mOwner.mCollider, true);
        //rotate in movement direction
        transform.LookAt(transform.position + mRigidRef.velocity);
        //self destruct after max life time is reached
        Destroy(gameObject, mMaxLifeTime);
    }

    //when something is hit
    public virtual void OnTriggerEnter(Collider other)
    {
        //cancel if collisions are ignored (used to have arrows stick in wall for a short moment)
        if (mIgnoreCollisions)
            return;
        //first, resolve for wall or combatentity
        switch (other.gameObject.layer)
        {
            //if blockprojectile, self destruct after short amount of time
            case (9):
                mRigidRef.velocity = Vector2.zero;
                mIgnoreCollisions = true;
                Destroy(gameObject, mTerrainLingerTime);
                break;
            //if block projectile and entity, self destruct after short amount of time
            case (10):
                mRigidRef.velocity = Vector2.zero;
                mIgnoreCollisions = true;
                Destroy(gameObject, mTerrainLingerTime);
                break;
            //if combatentity, damage entity, then self destruct
            case (6):
                mRigidRef.velocity = Vector2.zero;
                other.gameObject.GetComponent<CombatEntity>().TakeDamage(mDamage);
                Destroy(gameObject);
                break;
        }
    }
}