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
        //wait until skill is inactive
        yield return new WaitForSeconds(mSkillActiveTime);
        //restore variables
        defaultSkillRef.SetMultiShot((int)multiShotBackup.x, multiShotBackup.y);
        mSkillCoolDownRemaining = mSkillCoolDown;


        ////first, get closest enemy
        //CombatEntity target = mSkillOwner.ReturnClosestCombatEntity();
        ////cancel early if no targets are available
        //if (target != null)
        //{
        //
        //    //StopSkillExecution();
        //    //look at enemy
        //    Quaternion.LookRotation(target.transform.position);
        //    //shoot x times
        //    for (int i = 0; i < mShotCount; i++)
        //    {
        //        GameManager.GMInstance.mProjectileFactory.CreateNewProjectile(mSkillOwner, target);
        //        yield return new WaitForSeconds(mTimeBetweenShots);
        //    }
        //    //set cooldown
        //    mSkillCoolDownRemaining = mSkillCoolDown;
        //}
    }
}
