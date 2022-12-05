using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    public List<CombatEntity> mActiveEntitys { get; private set; }
    [SerializeField] Skill_Projectile mProjectileSkillPrefab;
    [SerializeField] Skill_MultiShot mMultiShotSkillPrefab;
    [SerializeField] Skill_CircularShot mCircularShotSkillPrefab;
    [SerializeField] Skill_Shield mShieldSkillPrefab;

    public void PopulateEntityList()
    {
        //create new enemy list
        mActiveEntitys = new List<CombatEntity>();
        //get the player first, this ensures element 0 is always the player
        mActiveEntitys.Add(GameObject.FindGameObjectWithTag("Player").GetComponent<CombatEntity>());
        //get all enemies
        foreach (GameObject current in GameObject.FindGameObjectsWithTag("Enemy"))
            mActiveEntitys.Add(current.GetComponent<CombatEntity>());
    }

    public void RemoveCombatEntity(CombatEntity target)
    {
        mActiveEntitys.Remove(target);
        if (mActiveEntitys.Count() == 1)
            ActivateVictoryCondition();
    }

    public CombatEntity[] GetAllEnemies()
    {
        return mActiveEntitys.Skip(1).ToArray();
    }

    public CombatEntity GetCombatEntityByIndex(int index)
    {
        return mActiveEntitys[Mathf.Clamp(index, 0, mActiveEntitys.Count - 1)];
    }

    public CombatEntity[] GetAllCombatEntitiesExcept(CombatEntity[] exceptions)
    {
        return mActiveEntitys.Except(exceptions).ToArray();
    }

    public Skill CreateNewSkillFromType(ESkillType type)
    {
        //instantiate a new skill object and return that reference
        //this is done because just using the prefab itself makes all combatentities use the same reference, so only 1 can use it
        switch (type)
        {
            case (ESkillType.projectile):
                return Instantiate(mProjectileSkillPrefab);
            case (ESkillType.multishot):
                return Instantiate(mMultiShotSkillPrefab);
            case (ESkillType.circularshot):
                return Instantiate(mCircularShotSkillPrefab);
            case (ESkillType.shield):
                return Instantiate(mShieldSkillPrefab);
            default:
                return null;
        }
    }

    public void ActivateVictoryCondition()
    {
        GameManager.GMInstance.mInputManager.mControlsAsset.PlayerMovement.Disable();
        GameManager.GMInstance.mInputManager.mControlsAsset.SpecialAbility.Disable();
        GameManager.GMInstance.mHUD.ActivateVictoryHud(mActiveEntitys[0]);
    }
}
