using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingAnchor : MonoBehaviour {

	public bool transformDependant;

	public void NotifyGrapple() => SendMessage("OnGrapple");
}
