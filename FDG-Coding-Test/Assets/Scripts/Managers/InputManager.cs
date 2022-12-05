using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //reference to unity input controller file
    public InputController mControlsAsset { get; private set; }

    //create a new set of controls
    public void CreateControlsAsset()
    {
        mControlsAsset = new InputController();
    }
}
