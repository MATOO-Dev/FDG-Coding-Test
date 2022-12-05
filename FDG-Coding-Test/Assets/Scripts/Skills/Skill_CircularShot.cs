using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_CircularShot : Skill
{
    [SerializeField] int mShotCount;    //how many projectiles are shot radially

    public override IEnumerator ActivateSkill()
    {
        //get angle between shots (e.g. 4 shots -> 90° between shots to form a cross)
        float mShotAngle = 360 / mShotCount;
        //shoot x times
        for (int i = 0; i < mShotCount; i++)
        {
            //get current direction vector
            //start at forwards, then rotate around origin -> this ensures that 1 projectile will always be shot in the direction the entity is currently facing
            Vector3 directionVector = Quaternion.Euler(0, mShotAngle * i, 0) * mSkillOwner.transform.forward;
            //create a new projectile in that direction
            GameManager.GMInstance.mProjectileFactory.CreateNewProjectile(mSkillOwner, directionVector);
        }
        //set attack cooldown after use -> prevent entity from attacking for a bit after using skill
        mSkillOwner.GetSkill(0).SetCoolDownRemaining(mDefaultAttackCoolDownAfterUse);
        //reset enemy ai after cast
        mSkillOwner.RestorePreviousState();
        //set cooldown
        mSkillCoolDownRemaining = mSkillCoolDown;
        yield return null;
    }
}
