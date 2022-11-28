using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    public List<CombatEntity> mActiveEntitys { get; private set; }

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
}
