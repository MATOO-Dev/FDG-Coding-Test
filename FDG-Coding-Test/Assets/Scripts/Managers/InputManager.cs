using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputController mControlsAsset { get; private set; }

    void Awake()
    {
        mControlsAsset = new InputController();
    }
}
