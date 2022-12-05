using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatEntity : MonoBehaviour
{
    //component references
    protected Rigidbody mRigidRef;                                      //reference to rigidbody component
    public Collider mCollider { get; private set; }                     //reference to collider component
    //health
    [SerializeField] protected int mCurrentHealth;                      //remaining health
    [SerializeField] protected int mMaxHealth;                          //maximum health
    [SerializeField] protected int mCurrentShield;                      //amount of shield remaining
    [SerializeField] protected int mLastShieldApplied;                  //amount of last shield granted
    //movement
    [SerializeField] protected float mMoveSpeed;                        //move speed of entity
    //combat
    [SerializeField] protected int mDamage;                             //damage this entity deals with projectiles
    [SerializeField] protected ESkillType mDefaultSkillStartType;       //what type of default ability this entity starts with
    [SerializeField] protected ESkillType mSpecialSkillStartType;       //what type of special ability this entity starts with
    [SerializeField] protected Skill[] mSkills;                         //array of available skills
    protected Transform mSkillContainer;                                //container gameobject that skill objects will be attached to
    //smooth rotation
    [SerializeField] protected float mTurnSpeed;                        //how fast the character mesh turns (movement turns instantly)
    [HideInInspector] public Vector3 mTurnTarget;                       //what direction the character mesh should face


    protected virtual void Awake()
    {
        //get component references
        mRigidRef = GetComponent<Rigidbody>();
        mCollider = GetComponent<Collider>();
        mSkillContainer = transform.GetChild(1);
        mSkills = new Skill[2];
    }

    protected virtual void Start()
    {
        //set current health to max health
        mCurrentHealth = mMaxHealth;
        //generate starting skills
        FillSkillContainer(0, mDefaultSkillStartType);
        FillSkillContainer(1, mSpecialSkillStartType);
        //update ui (healthbar/hud)
        SetHealthFill();
        SetShieldFill();
        SetAbilityFill(0, 1);
    }

    protected virtual void Update()
    {
        //rotate mesh in look direction smoothly
        SmoothLook();
    }

    protected virtual void FixedUpdate()
    {
        //used so fixedupdate can be overridden by children
    }

    protected virtual Vector2 GetMovementVector()
    {
        //get movement direction. overriden by children
        return Vector2.zero;
    }

    protected virtual void Move(Vector2 direction)
    {
        //normalize direction just in case
        direction.Normalize();
        //multiply normalized vector by movesped and set as velocity
        mRigidRef.velocity = new Vector3(direction.x, 0, direction.y) * mMoveSpeed;
        //rotate entity to look in movement direction -> sets a rotation target, actual rotation is handled by smoothlook();
        if (direction.magnitude != 0)
        {
            SetTurnTarget(transform.position + mRigidRef.velocity);
        }
    }

    //returns closest other combat entity
    public CombatEntity ReturnClosestCombatEntity()
    {
        //get all other combat entities
        CombatEntity[] otherEntities = GameManager.GMInstance.mCombatManager.GetAllCombatEntitiesExcept(new CombatEntity[] { this });
        //using system.linq to order by distance, then return first element
        //note: Vector2.sqrMagnitude is much faster than Vector2.magnitude, but can still be used to accurately compare distances
        return otherEntities.OrderBy(entity => (entity.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
    }

    //use default skill to attack closest combat entity
    protected virtual void AttackClosestCombatEntity()
    {
        mSkills[0].UseSkill();
    }

    public virtual void TakeDamage(int amount)
    {
        //first, reduce shield
        mCurrentShield -= amount;
        //reset amount to prevent health being damaged aswell if there is a shield
        amount = 0;
        //if shield amount is negative, damage was bigger than shield -> apply the remaining damage normally
        if (mCurrentShield < 0)
        {
            //remaining amount is negative value of shield (-amount if no shield)
            amount = Mathf.Abs(mCurrentShield);
            //reset shield value to prevent damage stacking infinitely
            mCurrentShield = 0;
        }
        mCurrentHealth -= amount;
        //update ui
        SetHealthFill();
        SetShieldFill();
        //die if health is zero
        if (mCurrentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        //remove own reference in combatManager, then self destruct
        GameManager.GMInstance.mCombatManager.RemoveCombatEntity(this);
        Destroy(gameObject);
    }

    public void SetShield(int amount)
    {
        //set shield
        mCurrentShield = amount;
        //set last shield applied. this is used to calculate the remaining shield fraction for ui
        mLastShieldApplied = amount;
        //update ui
        SetShieldFill();
    }

    //i tried getting maxHealth and damage via public var {get; private set;} like the other abilities
    //however, for some reson, on these 2 specific variables it breaks for some unknown reason
    //therefore, these are the only 2 variables with getters
    public int GetMaxHealth()
    {
        return mMaxHealth;
    }

    public int GetDamage()
    {
        return mDamage;
    }

    //return skill from skills array by index
    public Skill GetSkill(int index)
    {
        return mSkills[index];
    }

    //use skill from skills array by index if not null
    public void UseSkill(int index)
    {
        mSkills[index]?.UseSkill();
    }

    //fill skills array slot with a new skill
    void FillSkillContainer(int skillIndex, ESkillType newSkillType)
    {
        //if already filled, delete previous skill
        if (mSkills[skillIndex] != null)
            Destroy(mSkills[skillIndex].gameObject);
        //request a new skill from combat manager and initialize it
        mSkills[skillIndex] = GameManager.GMInstance.mCombatManager.CreateNewSkillFromType(newSkillType);
        mSkills[skillIndex].InitializeSkill(this);
        //attach new skill to skill container gameobject
        mSkills[skillIndex].transform.parent = mSkillContainer;
    }

    protected void SmoothLook()
    {
        //set rotation using lerp between current and look target. used to make mesh turn smoothly
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(mTurnTarget - transform.position), Time.deltaTime * mTurnSpeed);
    }

    public void SetTurnTarget(Vector3 targetPos)
    {
        //sets turn target for smooth rotation
        mTurnTarget = targetPos;
    }

    public float GetAbilityCharge(int index)
    {   
        //get ability charge fraction (from 0-1) for ability from skills array by index
        return mSkills[index].GetAbilityCharge();
    }

    //unused in player, but used for enemy ai, will be overriden
    //and just calling this, and letting the player do nothing is probably cleaner than:
    //adding a variable to the ability whether to cast combatentity to enemy and then call it
    public virtual void RestorePreviousState()
    {

    }

    //virtual placeholder functions for ui update, will be overriden
    protected virtual void SetHealthFill()
    {

    }
    protected virtual void SetShieldFill()
    {

    }
    public virtual void SetAbilityFill(float remainingCooldown, float totalCooldown)
    {

    }
}
