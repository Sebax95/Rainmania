using System;
using UnityEngine;

public class CrouchStateRouter : MonoBehaviour {

	public bool IsCrouched { get; set; }

	[SerializeField]
	public PlayerStateParameters standing;
	[SerializeField]
	public PlayerStateParameters crouched;

	public PlayerStateParameters Current {
		get {
			if(!IsCrouched)
				return standing;
			else
				return crouched;
		}
	}
}

[Serializable]
public struct PlayerStateParameters {
	public BoxCastParams[] whipHitbox;
	public Transform[] arrowSpawns;
}