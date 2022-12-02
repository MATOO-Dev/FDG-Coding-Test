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
    [SerializeField] public int mMaxHealth;
    [SerializeField] protected int mCurrentShield;
    //movement
    [SerializeField] protected float mMaxMoveSpeed;
    //combat
    [SerializeField] public int mDamage;
    [SerializeField] protected ESkillType mDefaultSkillStartType;
    [SerializeField] protected ESkillType mSpecialSkillStartType;
    [SerializeField] protected Skill[] mSkills;
    public HealthBarController mHealthBar { get; protected set; }
    protected Transform mSkillContainer;
    //smooth rotation
    [SerializeField] protected float mTurnSpeed;
    public Vector3 mTurnTarget;


    protected virtual void Awake()
    {
        mRenderRef = transform.GetChild(0).GetComponent<SpriteRenderer>();
        mRigidRef = GetComponent<Rigidbody>();
        mCollider = GetComponent<Collider>();
        mHealthBar = transform.GetChild(1).GetComponent<HealthBarController>();
        mSkillContainer = transform.GetChild(2);
        mSkills = new Skill[2];
    }

    protected virtual void Start()
    {
        mCurrentHealth = mMaxHealth;
        FillSkillContainer(0, mDefaultSkillStartType);
        FillSkillContainer(1, mSpecialSkillStartType);
    }

    protected virtual void Update()
    {
        SmoothLook();
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
        {
            SetTurnTarget(transform.position + mRigidRef.velocity);
        }
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
        mSkills[0].UseSkill();
    }

    public virtual void TakeDamage(int amount)
    {
        //first, reduce shield
        mCurrentShield -= amount;
        //reset amount to prevent health being damaged aswell if there is a shield
        amount = 0;
        //if shield amount is negative, apply the remaining damage normally
        if (mCurrentShield < 0)
        {
            //remaining amount is negative value of shield (-amount if no shield)
            amount = Mathf.Abs(mCurrentShield);
            //reset shield value to prevent damage stacking infinitely
            mCurrentShield = 0;
        }
        mCurrentHealth -= amount;
        mHealthBar.SetHealthFill((float)mCurrentHealth / (float)mMaxHealth);
        mHealthBar.SetShieldFill((float)mCurrentShield / ((float)mMaxHealth * 0.25f));
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
        mHealthBar.SetShieldFill((float)mCurrentShield / ((float)mMaxHealth * 0.25f));
    }

    public void SetShield(int amount)
    {
        mCurrentShield = amount;
        mHealthBar.SetShieldFill((float)mCurrentShield / ((float)mMaxHealth * 0.25f));
    }

    public int GetMaxHealth()
    {
        return mMaxHealth;
    }

    public int GetDamage()
    {
        return mDamage;
    }

    public Skill GetSkill(int index)
    {
        return mSkills[index];
    }

    public void UseSkill(int index)
    {
        mSkills[index]?.UseSkill();
    }

    void FillSkillContainer(int skillIndex, ESkillType newSkillType)
    {
        if (mSkills[skillIndex] != null)
            Destroy(mSkills[skillIndex].gameObject);
        mSkills[skillIndex] = GameManager.GMInstance.mCombatManager.CreateNewSkillFromType(newSkillType);
        mSkills[skillIndex].InitializeSkill(this);
        mSkills[skillIndex].transform.parent = this.transform.GetChild(2);
    }

    protected void SmoothLook()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(mTurnTarget - transform.position), Time.deltaTime * mTurnSpeed);
    }

    public void SetTurnTarget(Vector3 targetPos)
    {
        mTurnTarget = targetPos;
    }
}
