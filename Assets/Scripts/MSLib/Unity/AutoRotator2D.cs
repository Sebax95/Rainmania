using UnityEngine;

// compile with: -doc:DocFileName.xml 
namespace CustomMSLibrary.Unity {
	/// <summary cref="C{T}">
	/// </summary>
	public class AutoRotator2D {
		private float turningSpeed;
		private Transform transform;
		private Vector2 v1;
		private Vector2 v2;

		private float rotationPercent;
		private bool hasFinished;

		#region Properties
		public float RotationPercent { get { return rotationPercent; } }
		public bool HasFinished { get { return hasFinished; } }
		#endregion Properties

		///<summary>Initializing constructor</summary>
		public AutoRotator2D(Transform transform, float turningSpeed) {
			this.transform = transform;
			this.turningSpeed = turningSpeed;
		}

		///<summary>Use Builder methods if using empty constructor</summary>
		public AutoRotator2D() { }

		public void Start(Vector2 a, Vector2 b) {
			v1 = a;
			v2 = b;
			rotationPercent = 0;
			float deltaAngle = Vector2.SignedAngle(Vector2.right, v1) - Vector2.SignedAngle(Vector2.right, v2);
			if (deltaAngle == 0) rotationPercent = 0;
			hasFinished = false;
		}

		public void Update(float deltaTime) {
			if (hasFinished)
			{
				Debug.LogWarning("Autorotator on " + transform.name + " has already finished rotating." +
					" Add a condition to check on property AutoRotator2D.HasFinished.");
				return;
			}
			rotationPercent += ((turningSpeed != 0f) ? (deltaTime / turningSpeed) : 1);
			transform.rotation = MiscUnityUtilities.Slerp2D(v1, v2, rotationPercent, true);
			if (rotationPercent >= 1)
				hasFinished = true;
		}

		public void Stop() {
			hasFinished = true;
		}

		//Decorator
		public AutoRotator2D SetTransform(Transform transform) {
			this.transform = transform;
			return this;
		}

		public AutoRotator2D SetTurningSpeed(float speed) {
			turningSpeed = speed;
			return this;
		}

	}
}
