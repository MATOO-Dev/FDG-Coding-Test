using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CombatEntity
{

    protected override void Awake()
    {
        base.Awake();
        GameManager.GMInstance.mInputManager.mControlsAsset.PlayerMovement.Enable();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //get and cache movement vector
        Vector2 movementDirection = GetMovementVector();
        //apply movement
        Move(movementDirection);
        //attack if applicable (note: movement is still called, because if the magnitude is 0 this makes the player stop)
        mFiringCooldownRemaining -= Time.deltaTime;
        if (movementDirection.magnitude == 0 && mFiringCooldownRemaining <= 0)
            AttackClosestCombatEntity();
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
    }

    protected override void Die()
    {
        base.Die();
    }

    protected override Vector2 GetMovementVector()
    {
        //get desired movement direction based on input
        return GameManager.GMInstance.mInputManager.mControlsAsset.PlayerMovement.MoveDirection.ReadValue<Vector2>().normalized;
    }
}
