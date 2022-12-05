using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Shield : Skill
{
    [SerializeField] float mMaxShieldDuration;              //maximum duration the shield can be active
    [SerializeField] float mMaxHealthShieldMultiplier;      //percent/multiplier of max health that this shield blocks (from 0-1) 

    public override IEnumerator ActivateSkill()
    {  
        //set combatentitys shield
        mSkillOwner.SetShield((int)(mSkillOwner.GetMaxHealth() * mMaxHealthShieldMultiplier));
        //set cooldown to infinity to prevent ability being used again while shield is still active
        mSkillCoolDownRemaining = Mathf.Infinity;
        //set attack cooldown after use -> prevent entity from attacking for a bit after using this skill
        mSkillOwner.GetSkill(0).SetCoolDownRemaining(mDefaultAttackCoolDownAfterUse);
        //reset enemy ai after cast
        mSkillOwner.RestorePreviousState();
        //wait for maximum shield duration
        yield return new WaitForSeconds(mMaxShieldDuration);
        //reset shield
        mSkillOwner.SetShield(0);
        //set cooldown for this skill
        mSkillCoolDownRemaining = mSkillCoolDown;
    }
}
