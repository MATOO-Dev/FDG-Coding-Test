using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public CombatEntity[] mActiveEntitys { get; private set; }

    public void RefreshEntityList()
    {
        //rework this so it grabs the player and all enemies
        mActiveEntitys = new CombatEntity[3];
    }

}
