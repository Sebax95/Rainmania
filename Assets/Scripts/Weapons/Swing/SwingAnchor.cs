using UnityEngine;
using UnityEngine.Events;

public class SwingAnchor : MonoBehaviour {

	public bool transformDependant;
	public UnityEvent onGrapple;
	public UnityEvent onRelease;

	private void DoOnGrapple() {
		if(onGrapple == null)
			return;
		onGrapple.Invoke();
	}

	public void ReleaseGrapple() {
		if(onRelease == null)
			return;
		onRelease.Invoke();
	}

	public void AttachSwinger(Swinger swinger) {
		swinger.SetupSwing(this);
		DoOnGrapple();
	}

}
