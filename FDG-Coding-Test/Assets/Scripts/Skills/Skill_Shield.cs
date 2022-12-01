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
        yield return new WaitForSeconds(mMaxShieldDuration);
        mSkillOwner.BreakShields();
        mSkillCoolDownRemaining = mSkillCoolDown;
    }
}
