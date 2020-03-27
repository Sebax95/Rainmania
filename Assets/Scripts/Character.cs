using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Core;

public abstract class Character : MonoBehaviour
{
    public abstract void DoUpdate(Vector3 direction, BoolByte buttons);
}
