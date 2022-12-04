using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : CombatEntity
{
    protected enum EEnemyState
    {
        moving,
        attacking,
        abilityuse
    }

    [SerializeField] EEnemyState mCurrentState;
    EEnemyState mPreviousState;
    [SerializeField] protected float mModeSwitchTimeMin;
    [SerializeField] protected float mModeSwitchTimeMax;
    [SerializeField] protected float mModeSwitchTimeRemaining;
    [SerializeField] protected float mRandomMoveMinDistance;
    [SerializeField] protected float mRandomMoveMaxDistance;
    protected NavMeshAgent mNavAgent;

    protected override void Awake()
    {
        base.Awake();
        mNavAgent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
        SetRandmomMoveTarget();
        SetRandomSwitchTime();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //update timer
        SetCurrentStateAuto();
        BehaveBasedOnState();
        //debug
        if (mNavAgent.hasPath && mNavAgent.path.corners.Length > 1)
            Debug.DrawLine(transform.position, mNavAgent.path.corners[1], Color.magenta, 0.2f);
    }

    protected virtual void SetBehaviourState(EEnemyState newState)
    {
        mPreviousState = mCurrentState;
        mCurrentState = newState;
    }

    protected virtual void FlipMoveAttackStates()
    {
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
        mModeSwitchTimeRemaining = Random.Range(mModeSwitchTimeMin, mModeSwitchTimeMax);
    }

    protected virtual void MoveToAITarget()
    {
        //grab first corner point on navmesh path
        if (mNavAgent.path.corners.Length < 2)
            return;
        //todo: sometimes the mNavAgent.path.corners only has one element, which causes an error/crash, put some kind of check in for this case
        Vector3 cornerPos = mNavAgent.path.corners[1];
        //calculate direction vector
        Vector3 moveDirection = (cornerPos - transform.position).normalized;
        //feed direction vector into entity move function
        Move(new Vector2(moveDirection.x, moveDirection.z));
    }

    protected virtual void SetRandmomMoveTarget()
    {
        //get random point on navmesh
        Vector3 randomTarget = GameManager.GMInstance.mAIManager.GetRandomPointOnNavMesh(transform.position, mRandomMoveMinDistance, mRandomMoveMaxDistance);
        if (randomTarget == Vector3.zero)
        {
            Debug.Log("random target is zero, canceling");
            return;
        }
        //set point as navmesh target
        mNavAgent.destination = randomTarget;
        //make sure navmesh agent is not moving via nav agent component
        mNavAgent.isStopped = true;
    }

    protected virtual void BehaveBasedOnState()
    {
        switch (mCurrentState)
        {
            case (EEnemyState.moving):
                if (GetDistanceToAITarget() > 0.8f)
                    MoveToAITarget();
                else
                {
                    //FlipMoveAttackStates();
                    Move(Vector2.zero);
                    SetBehaviourState(EEnemyState.attacking);
                    SetRandomSwitchTime();
                    Debug.Log("switched states by proximity, new state is " + mCurrentState);
                }
                break;
            case (EEnemyState.attacking):
                UseSkill(0);
                break;
            case (EEnemyState.abilityuse):
                UseSkill(1);
                break;
        }
    }

    protected float GetDistanceToAITarget()
    {
        if (mNavAgent.hasPath && mNavAgent.path.corners.Length > 1)
            return Vector3.Distance(mNavAgent.path.corners[1], transform.position);
        return 0;
    }

    protected void SetCurrentStateAuto()
    {
        if (mCurrentState == EEnemyState.abilityuse)
            return;
        if (GetAbilityCharge(1) == 1)
        {
            SetBehaviourState(EEnemyState.abilityuse);
            Move(Vector2.zero);
            return;
        }
        if (mModeSwitchTimeRemaining > 0)
        {
            mModeSwitchTimeRemaining -= Time.deltaTime;
        }
        else
        {
            FlipMoveAttackStates();
            SetRandomSwitchTime();
        }
    }

    public override void RestorePreviousState()
    {
        SetBehaviourState(mPreviousState);
    }
}
