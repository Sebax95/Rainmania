using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Core;

public abstract class Controller : MonoBehaviour
{
    public Vector3 Direction { get; protected set; }
    public BoolByte Buttons { get; protected set; }
    public int ID { get; protected set; }

    protected abstract void SetDirection();
    protected abstract void SetButtons();

    public virtual void DoUpdate() {
        SetDirection();
        SetButtons();
    }

    protected void SendInput() {
        throw new NotImplementedException();
    }
}
