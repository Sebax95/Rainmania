using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Core;

public abstract class Controller : MonoBehaviour
{
    public int ID { get; protected set; }
    protected Character basePawn;

    protected abstract void DoMovement();
    protected abstract void DoActions();

    protected virtual void Update() {
        DoMovement();
        DoActions();
    }
}
