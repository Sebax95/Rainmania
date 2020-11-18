using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgraderEntity : TimedBehaviour {

	[Tooltip("MAKE SURE IT'S UNIQUE. Unique upgrade identifier. Makes sure it doesn't respawn.")]
	public string idName;

	public enum UpgradeType { IntegerAdder, FloatDecreaser, FloatMultiplier, BoolEnabler}
	public UpgradeType upgradeType;
	public string propertyName;

	public float value;

	public float destroyDelay = 0;

	public void Start() {
		if(UpgradesManager.Instance.HasUpgradeBeenConsumed(idName))
			Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.layer != Player.PLAYER_LAYER)
			return;
		ExecuteUpgrade();
	}

	private void ExecuteUpgrade() {
		var manager = UpgradesManager.Instance;
		switch(upgradeType)
		{
			case UpgradeType.IntegerAdder:
				manager.IncrementIntProperty(propertyName,(int)value);
				break;
			case UpgradeType.FloatDecreaser:
				manager.DecrementFloatProperty(propertyName,value);
				break;
			case UpgradeType.FloatMultiplier:
				manager.MultiplyFloatProperty(propertyName,value);
				break;
			case UpgradeType.BoolEnabler:
				manager.EnableBoolProperty(propertyName);
				break;
		}
		manager.ConsumeUpgrade(idName);
		GetComponent<Collider>().enabled = false;
		PlayVisual();
		Destroy(gameObject,destroyDelay);
	}

	private void PlayVisual() {
		//TODO: Agregar efecto visual al agarrar, quizas?
	}

	private void OnValidate() {
		if(upgradeType == UpgradeType.IntegerAdder)
			value = (int)value;
	}
}
