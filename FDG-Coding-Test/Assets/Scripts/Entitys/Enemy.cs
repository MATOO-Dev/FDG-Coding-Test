using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : CombatEntity
{
    protected enum EEnemyState  //enum for representing enemys current behaviour state
    {
        moving,
        attacking,
        abilityuse
    }

    [SerializeField] EEnemyState mCurrentState;                     //current behaviour state
    EEnemyState mPreviousState;                                     //previous behaviour state, used to reset after casting an ability
    [SerializeField] protected float mModeSwitchTimeMin;            //minimum time before flipping between movement/attack
    [SerializeField] protected float mModeSwitchTimeMax;            //maximum time before flipping between movement/attack
    protected float mModeSwitchTimeRemaining;                       //remaining time before flipping between movement/attack
    [SerializeField] protected float mRandomMoveMinDistance;        //minimum distance when moving to a random location
    [SerializeField] protected float mRandomMoveMaxDistance;        //maximum distance when moving to a random location
    [SerializeField] protected float mFirstAbilityUseWait;          //how long to wait until first ability cast after starting the level (sets ability cooldown to this)
    protected NavMeshAgent mNavAgent;                               //reference to navmesh agent component
    public HealthBarController mHealthBar { get; protected set; }   //reference to health bar

    protected override void Awake()
    {
        base.Awake();
        //get references
        mNavAgent = GetComponent<NavMeshAgent>();
        mHealthBar = transform.GetChild(2).GetComponent<HealthBarController>();
    }

    protected override void Start()
    {
        base.Start();
        //set initial ability cooldown upon starting level
        mSkills[1].SetCoolDownRemaining(mFirstAbilityUseWait);
        //set a random target to move to
        SetRandmomMoveTarget();
        //set a random time to switch to attacking
        SetRandomSwitchTime();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //update timers and change current state accordingly
        SetCurrentStateAuto();
        //execute behaviour based on current state
        BehaveBasedOnState();
    }

    protected virtual void SetBehaviourState(EEnemyState newState)
    {
        //set a new behaviour state and store the previous one
        mPreviousState = mCurrentState;
        mCurrentState = newState;
    }

    protected virtual void FlipMoveAttackStates()
    {
        //flip between movement and attacking states
        switch (mCurrentState)
        {
            case (EEnemyState.moving):
                Move(Vector2.zero);
                SetBehaviourState(EEnemyState.attacking);
                break;
            case (EEnemyState.attacking):
                SetRandmomMoveTarget();
                SetBehaviourState(EEnemyState.moving);
                break;
        }
    }

    protected virtual void SetRandomSwitchTime()
    {
        //set a random time (between minimum and maximum) until next movement/attacking state flip
        mModeSwitchTimeRemaining = Random.Range(mModeSwitchTimeMin, mModeSwitchTimeMax);
    }

    protected virtual void MoveToAITarget()
    {
        //sometimes the mNavAgent.path.corners only has one element, which causes an error/crash, cancel if applicable
        if (mNavAgent.path.corners.Length < 2)
            return;
        //grab first corner point on navmesh path (corner 0 is current position)
        Vector3 cornerPos = mNavAgent.path.corners[1];
        //calculate direction vector to that position
        Vector3 moveDirection = (cornerPos - transform.position).normalized;
        //feed direction vector into entity move function
        Move(new Vector2(moveDirection.x, moveDirection.z));
    }

    protected virtual void SetRandmomMoveTarget()
    {
        //get random point on navmesh from ai manager
        Vector3 randomTarget = GameManager.GMInstance.mAIManager.GetRandomPointOnNavMesh(transform.position, mRandomMoveMinDistance, mRandomMoveMaxDistance);
        //set point as navmesh target
        mNavAgent.destination = randomTarget;
        //make sure navmesh agent is not moving via nav agent component
        mNavAgent.isStopped = true;
    }

    protected virtual void BehaveBasedOnState()
    {
        //resolve for current state
        switch (mCurrentState)
        {
            //if moving
            case (EEnemyState.moving):
                //if still far away from movement target, keep moving to target
                if (GetDistanceToAITarget() > 0.8f)
                    MoveToAITarget();
                //if close to movement target, switch to attacking early
                else
                {
                    Move(Vector2.zero);
                    SetBehaviourState(EEnemyState.attacking);
                    SetRandomSwitchTime();
                }
                break;
            //if attacking, use default attack
            case (EEnemyState.attacking):
                UseSkill(0);
                break;
            //if ability use, use special ability
            case (EEnemyState.abilityuse):
                UseSkill(1);
                break;
        }
    }

    protected float GetDistanceToAITarget()
    {
        //if nav agent has a path, get the distance to the first corner point (corner 0 is current position)
        if (mNavAgent.hasPath && mNavAgent.path.corners.Length > 1)
            return Vector3.Distance(mNavAgent.path.corners[1], transform.position);
        return 0;
    }

    protected void SetCurrentStateAuto()
    {
        //set current state based on timers and ability charge
        //if current state is ability use, cancel. this will changed via restorepreviousstate(); called from the ability
        if (mCurrentState == EEnemyState.abilityuse)
            return;
        //if ability is fully charged, stop moving and set state to ability use
        if (GetAbilityCharge(1) == 1)
        {
            SetBehaviourState(EEnemyState.abilityuse);
            Move(Vector2.zero);
            return;
        }
        //count down timer to next mode switch
        if (mModeSwitchTimeRemaining > 0)
        {
            mModeSwitchTimeRemaining -= Time.deltaTime;
        }
        else
        {
            //if timer finished, flip move/attack states and start a new timer
            FlipMoveAttackStates();
            SetRandomSwitchTime();
        }
    }

    public override void RestorePreviousState()
    {
        //restore behaviour state from previous state
        SetBehaviourState(mPreviousState);
    }

    protected override void SetHealthFill()
    {
        //update healthbar ui
        mHealthBar.SetHealthFill((float)mCurrentHealth / (float)mMaxHealth);
    }
    protected override void SetShieldFill()
    {
        //if last shield applied is 0, the shield is being reset, so set fill value to 0 aswell
        if (mLastShieldApplied == 0)
            mHealthBar.SetShieldFill(0);
        else
            //update shield bar ui
            mHealthBar.SetShieldFill((float)mCurrentShield / (float)mLastShieldApplied);
    }
    public override void SetAbilityFill(float remainingCooldown, float totalCooldown)
    {
        //update update ability charge bar ui
        mHealthBar.SetAbilityFill(1 - (remainingCooldown / totalCooldown));
    }
}
