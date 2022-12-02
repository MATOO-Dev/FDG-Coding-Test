using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Projectile : Skill
{
    [SerializeField] int mShotCount;
    [SerializeField] float mShotSpacing;

    public override IEnumerator ActivateSkill()
    {
        //first, get closest enemy
        CombatEntity target = mSkillOwner.ReturnClosestCombatEntity();
        //cancel early if no targets are available
        if (target != null)
        {
            mSkillCoolDownRemaining = Mathf.Infinity;
            //StopSkillExecution();
            //look at enemy
            //mSkillOwner.transform.LookAt(target.transform.position);
            mSkillOwner.SetTurnTarget(target.transform.position);
            //repeat if applicable
            for (int i = 0; i < mShotCount; i++)
            {
                //request new projectile
                GameManager.GMInstance.mProjectileFactory.CreateNewProjectile(mSkillOwner, target);
                //wait for spacing
                yield return new WaitForSeconds(mShotSpacing);
            }
            //set cooldown
            mSkillCoolDownRemaining = mSkillCoolDown;
        }
        yield return null;
        //StopSkillExecution();
    }

    public void SetMultiShot(int shotCount, float shotSpacing)
    {
        mShotCount = shotCount;
        mShotSpacing = shotSpacing;
    }

    public Vector2 GetMultiShot()
    {
        return new Vector2(mShotCount, mShotSpacing);
    }
}
