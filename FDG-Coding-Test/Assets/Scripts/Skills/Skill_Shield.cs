using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Shield : Skill
{
    [SerializeField] float mMaxShieldDuration;

    protected override IEnumerator ActivateSkill()
    {
        mSkillOwner.SetShield((int)(mSkillOwner.mMaxHealth * 0.25f));
        yield return new WaitForSeconds(mMaxShieldDuration);
        mSkillOwner.BreakShields();
    }
}
