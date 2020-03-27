using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Core;

public class PlayerController : Controller
{
    private const KeyCode ATTACK_KEY = KeyCode.J;
    private const KeyCode SWITCH_KEY = KeyCode.K;

    protected override void SetDirection() {
        Direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    protected override void SetButtons() {
        Buttons = new BoolByte(Input.GetButton("Jump"), Input.GetKey(ATTACK_KEY), Input.GetKey(SWITCH_KEY));
    }

}
