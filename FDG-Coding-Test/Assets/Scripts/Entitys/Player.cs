using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CombatEntity
{

    protected override void Awake()
    {
        base.Awake();
        //enable player controls
        GameManager.GMInstance.mInputManager.mControlsAsset.PlayerMovement.Enable();
        GameManager.GMInstance.mInputManager.mControlsAsset.SpecialAbility.Enable();
        GameManager.GMInstance.mInputManager.mControlsAsset.SpecialAbility.UseSpecialAbility.performed += inputData => UseSkill(1);
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
        //get and cache desired movement vector
        Vector2 movementDirection = GetMovementVector();
        //apply movement
        Move(movementDirection);
        //attack if applicable (note: movement is still called, because if the magnitude is 0 this makes the player stop)
        if (movementDirection.magnitude == 0 && GetMovementVector().magnitude < 0.1f)
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

    protected override void SetHealthFill()
    {
        //update healthbar ui in hud
        GameManager.GMInstance.mHUD.SetHealthSliderFill((float)mCurrentHealth / (float)mMaxHealth);
    }
    protected override void SetShieldFill()
    {
        //update shield bar ui in hud
        if (mLastShieldApplied != 0)
            GameManager.GMInstance.mHUD.SetShieldSliderFill((float)mCurrentShield / (float)mLastShieldApplied);
        else
            GameManager.GMInstance.mHUD.ResetShieldSliderFill();
    }
    public override void SetAbilityFill(float remainingCooldown, float totalCooldown)
    {
        //update ability charge bar in hud
        GameManager.GMInstance.mHUD.SetAbilitySliderFill(1 - (remainingCooldown / totalCooldown));
    }
}
