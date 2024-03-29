﻿using System;
using UnityEngine;
using UnityEngine.Serialization;

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
	[FormerlySerializedAs("whipHitbox")]
	public BoxCastParams[] grabHitbox;
	public BoxCastParams[] damageHitbox;
	public Transform[] arrowSpawns;
}