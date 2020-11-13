using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRepulsion : TimedBehaviour
{
    private Collider col;
    private PhysicMaterial standard;
    private PhysicMaterial frictionless;
    private bool usingFrictionless;

    private void Start() {
        col = GetComponent<Collider>();
        standard = col.material;
        frictionless = new PhysicMaterial();
        frictionless.frictionCombine = PhysicMaterialCombine.Minimum;
        frictionless.dynamicFriction = 0;
        frictionless.staticFriction = 0;
    }

    private void OnCollisionStay(Collision collision) {
        Vector3 normal = collision.GetContact(0).normal;
        bool horizontalIsGreater = Mathf.Abs(normal.x) > Mathf.Abs(normal.y);


        //XOR. If using frictionless and against wall, change nothing.
        //If using standard and on ground, change nothing.
        //If using standard and against wall, or using frictionless and on ground, change.
        if(usingFrictionless ^ horizontalIsGreater)
        {
            col.material = horizontalIsGreater ? frictionless : standard;
            usingFrictionless = horizontalIsGreater;
        }
    }
}
