using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Projectile : Skill
{
    [SerializeField] int mShotCount;        //how many shots should be fired in sequence
    [SerializeField] float mShotSpacing;    //how long to wait between shots

    public override IEnumerator ActivateSkill()
    {
        //first, get closest enemy
        CombatEntity target = mSkillOwner.ReturnClosestCombatEntity();
        //cancel early if no targets are available
        if (target != null)
        {
            //set cooldown to infinity to prevent ability being used again while this skill is still active
            mSkillCoolDownRemaining = Mathf.Infinity;
            //look at target
            mSkillOwner.SetTurnTarget(target.transform.position);
            //repeat if applicable
            for (int i = 0; i < mShotCount; i++)
            {
                //request new projectile
                GameManager.GMInstance.mProjectileFactory.CreateNewProjectile(mSkillOwner, target);
                //wait for spacing
                yield return new WaitForSeconds(mShotSpacing);
            }
            //set cooldown for this skill
            mSkillCoolDownRemaining = mSkillCoolDown;
        }
        yield return null;
    }

    //sets how many shots are fired, used for multishot secondary skill or maybe future upgrades
    public void SetMultiShot(int shotCount, float shotSpacing)
    {
        mShotCount = shotCount;
        mShotSpacing = shotSpacing;
    }

    //get how many shots are fired per ability cast
    public Vector2 GetMultiShot()
    {
        return new Vector2(mShotCount, mShotSpacing);
    }
}
