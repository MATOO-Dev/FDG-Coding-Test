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
    [SerializeField] protected float mSkillCoolDown;                    //total cooldownof this skill
    [SerializeField]  protected float mSkillCoolDownRemaining;          //remaining cooldown of this skill
    protected CombatEntity mSkillOwner;                                 //owner of this skill
    public Coroutine mSkillRoutine { get; set; }                        //coroutine reference for skill execution
    [SerializeField] bool mAffectsUIElement;                            //whether the cooldown of this ability will be reflected on the hud or healthbar UIs
    [SerializeField] protected float mDefaultAttackCoolDownAfterUse;    //how long the default attack is disabled after using this ability

    void FixedUpdate()
    {
        //count down the cooldown
        if (mSkillCoolDownRemaining > 0)
        {
            mSkillCoolDownRemaining -= Time.deltaTime;
            //set cooldown on ui element
            if (mAffectsUIElement)
                mSkillOwner.SetAbilityFill(mSkillCoolDownRemaining, mSkillCoolDown);
        }
    }

    //use this skill if cooldown has run out
    public virtual void UseSkill()
    {
        if (mSkillCoolDownRemaining <= 0)
            mSkillRoutine = StartCoroutine(ActivateSkill());
    }

    //set up owner (and possibly other future variables)
    public virtual void InitializeSkill(CombatEntity newOwner)
    {
        mSkillOwner = newOwner;
        return;
    }

    //skill coroutine
    public virtual IEnumerator ActivateSkill()
    {
        yield return null;
    }

    //get charge progress (from 0-1) for this ability, basically the reverse of the cooldown
    public float GetAbilityCharge()
    {
        return Mathf.Clamp(1 - (mSkillCoolDownRemaining / mSkillCoolDown), 0, 1);
    }

    //set the cooldown manually from somewhere else
    public void SetCoolDownRemaining(float newCoolDown)
    {
        mSkillCoolDownRemaining = newCoolDown;
    }
}
