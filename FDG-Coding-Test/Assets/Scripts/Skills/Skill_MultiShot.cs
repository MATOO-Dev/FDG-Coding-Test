using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_MultiShot : Skill
{
    [SerializeField] int mShotCount;
    [SerializeField] float mTimeBetweenShots;

    protected override IEnumerator ActivateSkill()
    {
        //first, get closest enemy
        CombatEntity target = mSkillOwner.ReturnClosestCombatEntity();
        //cancel early if no targets are available
        if (target == null)
            StopSkillExecution();
        //look at enemy
        Quaternion.LookRotation(target.transform.position);
        //shoot x times
        for (int i = 0; i < mShotCount; i++)
        {
            ProjectileFactory.CreateNewProjectile(mSkillOwner, target);
            yield return new WaitForSeconds(mTimeBetweenShots);
        }
        //set cooldown
        yield return new WaitForSeconds(mSkillCoolDown);
    }
}
