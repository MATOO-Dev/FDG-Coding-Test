using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEntity : MonoBehaviour
{
    //component references
    SpriteRenderer mRenderRef;
    Rigidbody2D mRigidRef;
    [SerializeField] Projectile mProjectilePrefab;
    //health
    [SerializeField] int mCurrentHealth;
    [SerializeField] int mMaxHealth;
    [SerializeField] int mCurrentShield;
    //movement
    [SerializeField] float mMaxMoveSpeed;
    //combat
    [SerializeField] int mDamage;

    protected virtual void Awake()
    {
        mRenderRef = transform.GetChild(0).GetComponent<SpriteRenderer>();
        mRigidRef = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        mCurrentHealth = mMaxHealth;
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {
        //get and cache movement vector
        Vector2 movementDirection = GetMovementVector();
        //apply movement
        Move(movementDirection);
        //attack if applicable (note: movement is still called, because if the magnitude is 0 this makes the player stop)
        if (movementDirection.magnitude == 0)
            AttackClosestCombatEntity();
    }

    protected virtual Vector2 GetMovementVector()
    {
        return Vector2.zero;
    }

    protected virtual void Move(Vector2 direction)
    {
        //normalize direction just in case
        direction.Normalize();
        //multiply normalized vector by movesped and set as velocity
        mRigidRef.velocity = direction * mMaxMoveSpeed;
        //rotate entity to look in movement direction
        if (direction.magnitude != 0)
            transform.up = direction;
        //idea: set move speed to zero if stunned
    }

    protected CombatEntity ReturnClosestCombatEntity()
    {
        return null;
    }

    protected virtual void AttackClosestCombatEntity()
    {

    }

    protected virtual void TakeDamage(int amount)
    {
        //first, reduce shield
        mCurrentShield -= amount;
        //if shield amount is negative, apply the remaining damage normally
        if (mCurrentShield < 0)
        {
            //remaining amount is negative value of shield (-amount if no shield)
            amount = Mathf.Abs(mCurrentShield);
            //reset shield value to prevent damage stacking infinitely
            mCurrentShield = 0;
        }
        mCurrentHealth -= amount;
        if (mCurrentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Debug.Log("Io sono morto");
    }
}
