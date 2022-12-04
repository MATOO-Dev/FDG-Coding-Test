using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESkillType
{
    projectile,
    multishot,
    circularshot,
    shield
}

public class Skill : MonoBehaviour
{
    [SerializeField] protected float mSkillCoolDown;
    protected float mSkillCoolDownRemaining;
    protected CombatEntity mSkillOwner;
    public Coroutine mSkillRoutine { get; set; }
    [SerializeField] bool mAffectsUIElement;
    [SerializeField] protected float mDefaultAttackCoolDownAfterUse;

    protected void Start()
    {
        //set ability cooldown right at the start, to prevent instant ability use upon starting the game
        mSkillCoolDownRemaining = mSkillCoolDown;
    }

    void FixedUpdate()
    {
        if (mSkillCoolDownRemaining > 0)
        {
            mSkillCoolDownRemaining -= Time.deltaTime;
            if (mAffectsUIElement)
                //mSkillOwner.mHealthBar.SetAbilityFill(1 - (mSkillCoolDownRemaining / mSkillCoolDown));
                mSkillOwner.SetAbilityFill(mSkillCoolDownRemaining, mSkillCoolDown);
        }
    }

    public virtual void UseSkill()
    {
        if (mSkillCoolDownRemaining <= 0)
            mSkillRoutine = StartCoroutine(ActivateSkill());
    }

    public virtual void InitializeSkill(CombatEntity newOwner)
    {
        mSkillOwner = newOwner;
        return;
    }

    public virtual IEnumerator ActivateSkill()
    {
        yield return null;
    }

    public float GetAbilityCharge()
    {
        return Mathf.Clamp(1 - (mSkillCoolDownRemaining / mSkillCoolDown), 0, 1);
    }

    public void SetCoolDownRemaining(float newCoolDown)
    {
        mSkillCoolDownRemaining = newCoolDown;
    }
}
