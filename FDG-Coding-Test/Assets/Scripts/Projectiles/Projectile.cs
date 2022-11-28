using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float mMoveSpeed;
    [SerializeField] float mMaxLifeTime;
    CombatEntity mOwner;
    int mDamage;
    Rigidbody2D mRigidRef;


    public void InitiateProjectile(CombatEntity owner, Vector2 direction, int damage)
    {
        mOwner = owner;
        mDamage = damage;
        mRigidRef = GetComponent<Rigidbody2D>();
        mRigidRef.velocity = direction.normalized * mMoveSpeed;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), mOwner.mCollider, true);
        Destroy(gameObject, mMaxLifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
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