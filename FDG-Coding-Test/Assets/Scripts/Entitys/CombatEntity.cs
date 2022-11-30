using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatEntity : MonoBehaviour
{
    //component references
    protected SpriteRenderer mRenderRef;
    protected Rigidbody mRigidRef;
    public Collider mCollider { get; private set; }
    [SerializeField] protected Projectile mProjectilePrefab;
    //health
    [SerializeField] protected int mCurrentHealth;
    [SerializeField] public int mMaxHealth { get; protected set; }
    [SerializeField] protected int mCurrentShield;
    //movement
    [SerializeField] protected float mMaxMoveSpeed;
    //combat
    [SerializeField] public int mDamage { get; protected set; }
    [SerializeField] protected float mFiringCooldown;
    [SerializeField] protected float mFiringCooldownRemaining;
    [SerializeField] protected ESkillType mDefaultSkillType;
    [SerializeField] protected ESkillType mSpecialSkillType;
    [SerializeField] protected Skill mDefaultSkill;
    [SerializeField] protected Skill mSpecialSkill;

    protected virtual void Awake()
    {
        mRenderRef = transform.GetChild(0).GetComponent<SpriteRenderer>();
        mRigidRef = GetComponent<Rigidbody>();
        mCollider = GetComponent<Collider>();
    }

    protected virtual void Start()
    {
        mCurrentHealth = mMaxHealth;
        mDefaultSkill = Skill.ReturnNewSkillFromType(mDefaultSkillType);
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

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
        mRigidRef.velocity = new Vector3(direction.x, 0, direction.y) * mMaxMoveSpeed;
        //rotate entity to look in movement direction
        if (direction.magnitude != 0)
            //transform.LookAt(transform.position + (Vector3)direction);
            Quaternion.LookRotation(direction);
        //idea: set move speed to zero if stunned
    }

    public CombatEntity ReturnClosestCombatEntity()
    {
        //get all other combat entities
        CombatEntity[] otherEntities = GameManager.GMInstance.mCombatManager.GetAllCombatEntitiesExcept(new CombatEntity[] { this });
        //using system.linq to order by distance, then return first element
        //note: Vector2.sqrMagnitude is much faster than Vector2.magnitude, but can still be used to accurately compare distances
        return otherEntities.OrderBy(entity => (entity.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
    }

    protected virtual void AttackClosestCombatEntity()
    {
        //mDefaultSkill.Use();
    }

    public virtual void TakeDamage(int amount)
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
        //if ui is added, consider doing mathf.clamp here to prevent values below 0 breaking health bars
        if (mCurrentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Debug.Log("Io sono morto");
        //remove own reference in combatManager (this might break if the player is deleted, todo: check this one implemented)
        GameManager.GMInstance.mCombatManager.RemoveCombatEntity(this);
        Destroy(gameObject);
    }

    public void AddShield(int amount)
    {
        mCurrentShield += amount;
    }

    public void SetShield(int amount)
    {
        mCurrentShield = amount;
    }

    public void BreakShields()
    {
        mCurrentShield = 0;
    }
}
