using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Shield : Skill
{
    [SerializeField] float mMaxShieldDuration;
    [SerializeField] float mMaxHealthShieldMultiplier;

    public override IEnumerator ActivateSkill()
    {
        mSkillOwner.SetShield((int)(mSkillOwner.GetMaxHealth() * mMaxHealthShieldMultiplier));
        mSkillCoolDownRemaining = Mathf.Infinity;
        //set attack cooldown after use -> prevent entity from attacking for a bit after using this skill
        mSkillOwner.GetSkill(0).SetCoolDownRemaining(mDefaultAttackCoolDownAfterUse);
        //reset enemy ai after cast
        mSkillOwner.RestorePreviousState();
        yield return new WaitForSeconds(mMaxShieldDuration);
        mSkillOwner.SetShield(0);
        mSkillCoolDownRemaining = mSkillCoolDown;
    }
}
