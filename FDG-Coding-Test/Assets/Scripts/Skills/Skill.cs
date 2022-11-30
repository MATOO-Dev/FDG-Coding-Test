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
    protected CombatEntity mSkillOwner;
    public Coroutine mSkillRoutine { get; private set; }

    public void Use()
    {
        mSkillRoutine = StartCoroutine(ActivateSkill());
    }

    public virtual void InitializeSkill(CombatEntity newOwner)
    {
        mSkillOwner = newOwner;
        return;
    }

    protected virtual IEnumerator ActivateSkill()
    {
        yield return null;
    }

    public virtual void StopSkillExecution()
    {
        StopCoroutine(mSkillRoutine);
    }

    //maybe a get coroutine progress function here??

    public static Skill ReturnNewSkillFromType(ESkillType type)
    {
        Debug.Log("attemping to create new skill from type " + type);
        switch (type)
        {
            case (ESkillType.projectile):
                Debug.Log("creating new skill of type projectile");
                return new Skill_Projectile();
            case (ESkillType.multishot):
                Debug.Log("creating new skill of type multishot");
                return new Skill_MultiShot();
            case (ESkillType.circularshot):
                Debug.Log("creating new skill of type circularshot");
                return new Skill_CircularShot();
            case (ESkillType.shield):
                Debug.Log("creating new skill of type shield");
                return new Skill_Shield();
            default:
                return null;
        }
    }
}
