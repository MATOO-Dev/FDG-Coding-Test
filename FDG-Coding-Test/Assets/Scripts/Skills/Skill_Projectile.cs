using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Projectile : Skill
{
    protected override IEnumerator ActivateSkill()
    {
        //first, get closest enemy
        CombatEntity target = mSkillOwner.ReturnClosestCombatEntity();
        //cancel early if no targets are available
        if (target == null)
            StopSkillExecution();
        //look at enemy
        Quaternion.LookRotation(target.transform.position);
        //request new projectile
        ProjectileFactory.CreateNewProjectile(mSkillOwner, target);
        //set cooldown
        yield return new WaitForSeconds(mSkillCoolDown);
    }
}
