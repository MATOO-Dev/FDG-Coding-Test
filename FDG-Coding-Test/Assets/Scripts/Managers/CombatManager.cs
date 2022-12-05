using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    public List<CombatEntity> mActiveEntitys { get; private set; }      //list of "active" combatentities. active entitites are those that are alive
    [SerializeField] Skill_Projectile mProjectileSkillPrefab;           //prefab for the default projectile attack skill
    [SerializeField] Skill_MultiShot mMultiShotSkillPrefab;             //prefab for multishot projectile attack skill
    [SerializeField] Skill_CircularShot mCircularShotSkillPrefab;       //prefab for circular shot skill
    [SerializeField] Skill_Shield mShieldSkillPrefab;                   //prefab for defensive shield skill

    //fill the entity list with all active entities
    public void PopulateEntityList()
    {
        //create new entity list
        mActiveEntitys = new List<CombatEntity>();
        //get the player first, this ensures element 0 is always the player in the beginning
        mActiveEntitys.Add(GameObject.FindGameObjectWithTag("Player").GetComponent<CombatEntity>());
        //then add all enemies
        foreach (GameObject current in GameObject.FindGameObjectsWithTag("Enemy"))
            mActiveEntitys.Add(current.GetComponent<CombatEntity>());
    }

    //remove an entity from the active list, and activate the victory condition if only one is left
    public void RemoveCombatEntity(CombatEntity target)
    {
        mActiveEntitys.Remove(target);
        if (mActiveEntitys.Count() == 1)
            ActivateVictoryCondition();
    }

    //get a combat entity by index
    public CombatEntity GetCombatEntityByIndex(int index)
    {
        return mActiveEntitys[Mathf.Clamp(index, 0, mActiveEntitys.Count - 1)];
    }

    //get all combat entities except a list of exceptions. can be used to for example get all entities except self
    public CombatEntity[] GetAllCombatEntitiesExcept(CombatEntity[] exceptions)
    {
        return mActiveEntitys.Except(exceptions).ToArray();
    }

    //based on parameter, create a new skill from linked prefab and return the attached skill script
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

    //disable player movement and activate victory hud
    public void ActivateVictoryCondition()
    {
        GameManager.GMInstance.mInputManager.mControlsAsset.PlayerMovement.Disable();
        GameManager.GMInstance.mInputManager.mControlsAsset.SpecialAbility.Disable();
        GameManager.GMInstance.mHUD.ActivateVictoryHud(mActiveEntitys[0]);
    }
}
