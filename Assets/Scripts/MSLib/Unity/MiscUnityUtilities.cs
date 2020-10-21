using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// compile with: -doc:DocFileName.xml 
namespace CustomMSLibrary.Unity {

	public static class MiscUnityUtilities {
		#region Detectors
		#region 2D Detector
		/// <summary>
		/// Compact implementation of a "Line of Sight"/"Detection" algorithm in 2D.
		/// </summary>
		/// <param name="transform">Transform of the detector entity</param>
		/// <param name="detectionLayerMask">Layer mask of detectable targets</param>
		/// <param name="detectionRange">Detection radius from detector's pivot/centre</param>
		/// <param name="detectionFOV">Detection Fielf of View in degrees</param>
		/// <param name="detectionSightlineLayerMask">"Opaque" objects layer mask. Detector will ignore those excluded layers when determining visibility to target.
		/// Target must be included</param>
		/// <param name="target">First acquired target. Null if none found.</param>
		/// <param name="usesRight">When determining field of view, assign to 'true' if 'transform.right' is considered the front. Otherwise it will use 'transform.forward'.</param>
		/// <returns></returns>
		public static bool Detection2D(Transform transform, LayerMask detectionLayerMask, float detectionRange,
			float detectionFOV, LayerMask detectionSightlineLayerMask, out Transform target, bool usesRight = false) {

			Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, detectionRange, detectionLayerMask);
			if (cols != null && cols.Length > 0)
			{
				foreach (var item in cols)
				{
					Vector2 relativePos = item.transform.position - transform.position;
					if (Vector2.Angle((usesRight ? transform.right : transform.forward), relativePos) < detectionFOV * 0.5f)
					{
						RaycastHit2D hit = Physics2D.Raycast(transform.position, relativePos, detectionRange,
							detectionSightlineLayerMask);
						if (detectionLayerMask.ContainsLayer(hit.transform.gameObject.layer))
						{
							target = hit.transform;
							return true;
						}
					}
				}
			}
			target = null;
			return false;
		}
		#endregion
		#region 3D Detector
		/// <summary>
		/// Compact implementation of a "Line of Sight"/"Detection" algorithm in 3D.
		/// </summary>
		/// <param name="transform">Transform of the detector entity</param>
		/// <param name="detectionLayerMask">Layer mask of detectable targets</param>
		/// <param name="detectionRange">Detection radius from detector's pivot/centre</param>
		/// <param name="detectionFOV">Detection Fielf of View in degrees</param>
		/// <param name="detectionSightlineLayerMask">"Opaque" objects layer mask. Detector will ignore those excluded layers when determining visibility to target.
		/// Target must be included</param>
		/// <param name="target">First acquired target. Null if none found.</param>
		/// <param name="usesRight">When determining field of view, assign to 'true' if 'transform.right' is considered the front. Otherwise it will use 'transform.forward'.</param>
		/// <returns></returns>
		public static bool Detection3D(Transform transform, LayerMask detectionLayerMask, float detectionRange,
			float detectionFOV, LayerMask detectionSightlineLayerMask, out Transform target, bool usesRight = false) =>
			Detection3D(transform, detectionLayerMask, detectionRange, detectionFOV,
				detectionSightlineLayerMask, out target, Vector3.zero, usesRight);
		
		/// <summary>
		/// Compact implementation of a "Line of Sight"/"Detection" algorithm in 3D.
		/// </summary>
		/// <param name="transform">Transform of the detector entity</param>
		/// <param name="detectionLayerMask">Layer mask of detectable targets</param>
		/// <param name="detectionRange">Detection radius from detector's pivot/centre</param>
		/// <param name="detectionFOV">Detection Fielf of View in degrees</param>
		/// <param name="detectionSightlineLayerMask">"Opaque" objects layer mask. Detector will ignore those excluded layers when determining visibility to target.
		/// Target must be included</param>
		/// <param name="target">First acquired target. Null if none found.</param>
		/// <param name="deltaPos">View offset for when the "eyes" are not located at the object pivot</param>
		/// <param name="usesRight">When determining field of view, assign to 'true' if 'transform.right' is considered the front. Otherwise it will use 'transform.forward'.</param>
		/// <returns></returns>
		public static bool Detection3D(Transform transform, LayerMask detectionLayerMask, float detectionRange,
			float detectionFOV, LayerMask detectionSightlineLayerMask, out Transform target, Vector3 deltaPos, bool usesRight = false) =>
			 Detection3D(transform, detectionLayerMask, detectionRange, detectionFOV,
				 detectionSightlineLayerMask, out target, deltaPos, usesRight ? Vector3.right : Vector3.forward);
		

		/// <summary>
		/// Compact implementation of a "Line of Sight"/"Detection" algorithm in 3D.
		/// </summary>
		/// <param name="transform">Transform of the detector entity</param>
		/// <param name="detectionLayerMask">Layer mask of detectable targets</param>
		/// <param name="detectionRange">Detection radius from detector's pivot/centre</param>
		/// <param name="detectionFOV">Detection Fielf of View in degrees</param>
		/// <param name="detectionSightlineLayerMask">"Opaque" objects layer mask. Detector will ignore those excluded layers when determining visibility to target.
		/// Target must be included</param>
		/// <param name="target">First acquired target. Null if none found.</param>
		/// <param name="deltaPos">View offset for when the "eyes" are not located at the object pivot</param>
		/// <param name="frontDirection">Determines the direction considered as "front", relative to the transform.
		/// It will be rotated accordingly to the transform's rotation.</param>
		/// <returns></returns>
		public static bool Detection3D(Transform transform, LayerMask detectionLayerMask, float detectionRange,
			float detectionFOV, LayerMask detectionSightlineLayerMask, out Transform target, Vector3 deltaPos, Vector3 frontDirection) {
			Vector3 refPos = transform.position + (transform.rotation * deltaPos);
			Collider[] cols = Physics.OverlapSphere(refPos, detectionRange, detectionLayerMask);
			if(cols != null && cols.Length > 0)
			{
				foreach(var item in cols)
				{
					Vector3 relativePos = item.transform.position - refPos;
					if(Vector3.Angle((transform.rotation * frontDirection), relativePos) < detectionFOV * 0.5f)
					{
						RaycastHit hit;
						bool didHit = Physics.Raycast(refPos, relativePos, out hit, detectionRange,
							detectionSightlineLayerMask);
						if(didHit && detectionLayerMask.ContainsLayer(hit.transform.gameObject.layer))
						{
							target = hit.transform;
							return true;
						}
					}
				}
			}
			target = null;
			return false;
		}
		#endregion
		#region 3D Detector All
		public static bool Detection3DAll(Transform transform, LayerMask detectionLayerMask, float detectionRange,
			float detectionFOV, LayerMask detectionSightlineLayerMask, out List<Transform> targets, Vector3 deltaPos, bool usesRight = false) {

			Vector3 refPos = transform.position + (transform.rotation * deltaPos);
			Collider[] cols = Physics.OverlapSphere(refPos, detectionRange, detectionLayerMask);
			bool found = false;
			targets = new List<Transform>();

			if (cols != null && cols.Length > 0)
			{
				foreach (var item in cols)
				{
					Vector3 relativePos = item.transform.position - transform.position;
					if (Vector3.Angle((usesRight ? transform.right : transform.forward), relativePos) < detectionFOV * 0.5f)
					{
						RaycastHit hit;

						bool didHit = Physics.Raycast(refPos, relativePos, out hit, detectionRange,
							detectionSightlineLayerMask);
						if (didHit && detectionLayerMask.ContainsLayer(hit.transform.gameObject.layer))
						{
							targets.Add(hit.transform);
							found = true;
						}
					}
				}
			}
			return found;
		}

		public static bool Detection3DAll(Transform transform, LayerMask detectionLayerMask, float detectionRange,
			float detectionFOV, LayerMask detectionSightlineLayerMask, out List<Transform> targets, bool usesRight = false) {

			return Detection3DAll(transform, detectionLayerMask, detectionRange, detectionFOV, detectionSightlineLayerMask, out targets, Vector3.zero, usesRight);
		}
		#endregion
		#endregion

		public static Quaternion Slerp2D(Vector2 a, Vector2 b, float t, bool usesRight = false) {

			float fa;
			float fb;

			if (!usesRight)
			{
				fa = Vector2.SignedAngle(Vector2.up, a);
				fb = Vector2.SignedAngle(Vector2.up, b);
			} else
			{
				fa = Vector2.SignedAngle(Vector2.right, a);
				fb = Vector2.SignedAngle(Vector2.right, b);
			}

			Quaternion qa = Quaternion.AngleAxis(fa, Vector3.forward);
			Quaternion qb = Quaternion.AngleAxis(fb, Vector3.forward);

			return Quaternion.Slerp(qa, qb, t);
		}

		public static Quaternion SlerpWithAxises(Vector3 a, Vector3 b, float t, Vector3 reference, Vector3 axis) {
			
			float fa = Vector3.SignedAngle(reference, a, axis);
			float fb = Vector3.SignedAngle(reference, b, axis);

			Quaternion qa = Quaternion.AngleAxis(fa, axis);
			Quaternion qb = Quaternion.AngleAxis(fb, axis);

			return Quaternion.Slerp(qa, qb, t);
		}

		public static Vector3 Vector3XY(Vector2 vec, float z) => new Vector3(vec.x, vec.y, z);

		public static Vector3 Vector3XZ(Vector2 vec, float y) => new Vector3(vec.x, y, vec.y);

		/// <summary>
		/// Returns true if the angle between vec1 and vec2 is smaller than the given angle.
		/// </summary>
		/// <param name="vec1">First vector</param>
		/// <param name="vec2">Second vector</param>
		/// <param name="comparisonAngle">Angle to compare against in degrees.</param>
		/// <returns></returns>
		public static bool AngleBetweenIsSmaller(Vector3 vec1, Vector3 vec2, float comparisonAngle)
			=> Vector3.Angle(vec1, vec2) < comparisonAngle;

		/// <summary>
		/// Returns Cosine of angle. Listen, it may be easy, but sometimes you need a reminder
		/// </summary>
		public static float DotValueForNormalizedAngles(float degrees) => Mathf.Cos(degrees);

		/// <summary>
		/// Given an int value, returns a layer mask for that layer alone.
		/// </summary>
		/// <param name="layer">Layer to create a layer mask from (0-31)</param>
		/// <returns></returns>
		public static LayerMask IntToLayerMask(int layer) {
			return (1 << layer);
		}
	}
}
