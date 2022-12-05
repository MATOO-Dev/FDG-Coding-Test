using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GMInstance { get; private set; }              //singleton instance
    //references other managers
    public CombatManager mCombatManager { get; private set; }               //reference to combat manager
    public InputManager mInputManager { get; private set; }                 //reference to input manager
    public AIManager mAIManager { get; private set; }                       //reference to ai manager
    //factories
    public ProjectileFactory mProjectileFactory { get; private set; }       //reference to projectile factory
    //other important object references
    public Player mPlayerRef { get; private set; }                          //reference to player
    public Camera mMainCamera { get; private set; }                         //reference to main camera
    public HUDController mHUD { get; private set; }                         //reference to hud controller

    void Awake()
    {
        //establish singleton pattern for game manager
        if (GMInstance == null)
            GMInstance = this;
        else if (GMInstance != this)
            Destroy(gameObject);
        //get references and initialize their base values if applicable
        //get component in children is not very efficient, but since this is only called one, it should not have a major performance impact
        mMainCamera = GetComponentInChildren<Camera>();
        mCombatManager = GetComponentInChildren<CombatManager>();
        mCombatManager.PopulateEntityList();
        mInputManager = GetComponentInChildren<InputManager>();
        mInputManager.CreateControlsAsset();
        mAIManager = GetComponentInChildren<AIManager>();
        mProjectileFactory = GetComponentInChildren<ProjectileFactory>();
        mPlayerRef = (Player)mCombatManager.GetCombatEntityByIndex(0);
        mHUD = GetComponentInChildren<HUDController>();
        //generate a "random" seed from the current time
        Random.InitState(System.DateTime.Now.Millisecond);
    }
}
