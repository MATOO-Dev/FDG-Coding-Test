using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton instance
    public static GameManager GMInstance { get; private set; }
    //references other managers
    public CombatManager mCombatManager { get; private set; }
    public InputManager mInputManager { get; private set; }
    //player reference
    public Player mPlayerRef { get; private set; }

    void Awake()
    {
        //establish singleton pattern
        if (GMInstance == null)
            GMInstance = this;
        else if (GMInstance != this)
            Destroy(gameObject);
        //get references and initialize their base values if applicable
        mCombatManager = GetComponentInChildren<CombatManager>();
        mCombatManager.PopulateEntityList();
        mInputManager = GetComponentInChildren<InputManager>();
        mInputManager.CreateControlsAsset();
        mPlayerRef = (Player)mCombatManager.GetCombatEntityByIndex(0);
        //mPlayerRef = GameObject.Find("Player").GetComponent<Player>();
    }
}
