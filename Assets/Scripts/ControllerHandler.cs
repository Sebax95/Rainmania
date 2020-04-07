using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Core;

public class ControllerHandler : MonoBehaviour
{
    private Dictionary<Controller, Character> controllerToCharacter;
    public static ControllerHandler Instance { get; private set; }

    private void Awake() {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        } else
            Destroy(gameObject);

        controllerToCharacter = new Dictionary<Controller, Character>();
    }



    public void RequestAssignation(Controller control, Character user) {
        if(controllerToCharacter.ContainsValue(user))
            return;
        controllerToCharacter.Add(control, user);
    }

}
