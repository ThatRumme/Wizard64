using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{

    public PlayerInput inputs;

    // Start is called before the first frame update
    private void Awake()
    {
        inputs = new PlayerInput();
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }
}
