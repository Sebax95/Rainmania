using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Upgrade Data Preset", menuName = "Upgrade Data Preset", order = 4)]
public class UpgradesData : ScriptableObject {
	[Header("General")]
	public int playerMaxLife;

	[Header("Bow")]
	public bool bowAcquired = false;
	public bool arrowCanPlatform = false;
	public bool arrowCanAnchor;
	public int arrowMaxAmount;
	public int arrowDamage;
	public float arrowShootSpeed;

	[Header("Whip")]
	public bool whipAcquired = false;
	public bool whipCanGrapple = false;
	public int whipDamage;
	public float whipAttackSpeed;

	public void IncrementIntProperty(ref int property, int amount) => property += amount;
	public void DecrementFloatProperty(ref float property, float amount) => property -= amount;
	public void MultiplyFloatProperty(ref float property, float mult) => property *= mult;
	public void EnableBoolProperty(ref bool property) => property = true;
}
