using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_MultiShot : Skill
{
    [SerializeField] int mShotCount;
    [SerializeField] float mTimeBetweenShots;
    [SerializeField] float mSkillActiveTime;

    public override IEnumerator ActivateSkill()
    {
        //this affects the default attack to fire thrice instead
        //2 options: 
        //1) get reference to default attack -> edit shot counter variable in default skill -> let default skill handle the rest
        //might be messy code, but scales better
        //2) replace default skill, and handle multi attack in this class
        //cleaner code, but changes have to be made twice
        //going with option 1 for now:

        //get primary skill from owner entity
        Skill_Projectile defaultSkillRef = (Skill_Projectile)mSkillOwner.GetSkill(0);
        //save current variables
        Vector2 multiShotBackup = defaultSkillRef.GetMultiShot();
        //set variables
        defaultSkillRef.SetMultiShot((int)multiShotBackup.x * mShotCount, mTimeBetweenShots);
        //set attack cooldown after use -> if the // are removed, this can be used to e.g. make entity attack instantly after using multishot to get more value out of it
        //mSkillOwner.GetSkill(0).SetCoolDownRemaining(mAttackCoolDownAfterUse);
        //reset enemy ai after cast -> this would probably be better as a switch enemy state to attacking instead of previous
        mSkillOwner.RestorePreviousState();
        //wait until skill is inactive
        yield return new WaitForSeconds(mSkillActiveTime);
        //restore variables
        defaultSkillRef.SetMultiShot((int)multiShotBackup.x, multiShotBackup.y);
        mSkillCoolDownRemaining = mSkillCoolDown;
    }
}
