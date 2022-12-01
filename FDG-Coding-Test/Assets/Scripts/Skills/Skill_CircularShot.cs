using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_CircularShot : Skill
{
    [SerializeField] int mShotCount;

    public override IEnumerator ActivateSkill()
    {
        //get angle between shots (e.g. 4 shots -> 90° between shots to form a cross)
        float mShotAngle = 360 / mShotCount;
        //shoot x times
        for (int i = 0; i < mShotCount; i++)
        {
            //get current direction vector
            //start at forwards, then rotate around origin
            Vector3 directionVector = Quaternion.Euler(0, mShotAngle * i, 0) * mSkillOwner.transform.forward;
            //create a new projectile in that direction
            GameManager.GMInstance.mProjectileFactory.CreateNewProjectile(mSkillOwner, directionVector);
        }
        //set cooldown
        //yield return new WaitForSeconds(mSkillCoolDown);
        mSkillCoolDownRemaining = mSkillCoolDown;
        yield return null;
    }
}
