using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float mMoveSpeed;
    [SerializeField] float mMaxLifeTime;
    CombatEntity mOwner;
    int mDamage;
    Rigidbody mRigidRef;


    public virtual void InitiateProjectile(CombatEntity owner, Vector3 direction, int damage)
    {
        mOwner = owner;
        mDamage = damage;
        mRigidRef = GetComponent<Rigidbody>();
        mRigidRef.velocity = direction.normalized * mMoveSpeed;
        Physics.IgnoreCollision(GetComponent<Collider>(), mOwner.mCollider, true);
        transform.LookAt(transform.position + mRigidRef.velocity);
        Destroy(gameObject, mMaxLifeTime);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        //first, resolve for wall or combatentity
        switch (other.gameObject.layer)
        {
            //if wall, self destruct
            case (8):
                mRigidRef.velocity = Vector2.zero;
                Destroy(gameObject);
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