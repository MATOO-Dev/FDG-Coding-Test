using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESkillType
{
    projectile,
    multishot,
    circularshot,
    shield
}

public class Skill : MonoBehaviour
{
    [SerializeField] protected float mSkillCoolDown;
    protected float mSkillCoolDownRemaining;
    protected CombatEntity mSkillOwner;
    public Coroutine mSkillRoutine { get; set; }

    void FixedUpdate()
    {
        if (mSkillCoolDownRemaining > 0)
            mSkillCoolDownRemaining -= Time.deltaTime;
    }

    public virtual void UseSkill()
    {
        if (mSkillCoolDownRemaining <= 0)
            mSkillRoutine = StartCoroutine(ActivateSkill());
    }

    public virtual void InitializeSkill(CombatEntity newOwner)
    {
        mSkillOwner = newOwner;
        return;
    }

    public virtual IEnumerator ActivateSkill()
    {
        yield return null;
    }
}
