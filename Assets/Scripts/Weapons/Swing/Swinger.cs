using System.Runtime.CompilerServices;
using UnityEngine;
using CustomMSLibrary.Unity;

public class Swinger : Controllable, IMoveOverride {
	public float gravityMult;

	private float anchorTime;
	public float WhipDistance { private get; set; }
	[Range(0f, 90f)] public float maxSwingAngle;
	public float swingAddingStrength;

	private bool dependOnTransform;
	private Transform anchorTransform;
	private Vector3 anchorPos;

	private float initialState; //Initial angle in rads * Sqrt(gravity / distance from anchor)
	private float initialAngle;

	private sbyte initialAngleDirection;
	private bool movingRight;

	private bool overriding;
	private IMoveOverrideable swingOverrider;


	private Rigidbody thisRB;
	private SwingAnim anim;
	private MomentumKeeper momentum;
	private bool prevGravitystate;

	private float GetTime => Time.time - anchorTime;
	private float MaxSwingAngleRad => maxSwingAngle * Mathf.Deg2Rad;
	public const float HALF_PI = 1.57079637F;

	[HideInInspector]
	public bool firstFrame = false;


	private void Awake() {
		thisRB = GetComponent<Rigidbody>();
		momentum = GetComponent<MomentumKeeper>();
		swingOverrider = GetComponent<IMoveOverrideable>();
		anim = GetComponent<SwingAnim>();

		ControllerHandler.Instance.RequestAssignation(Controller.Create<PlayerSwingerController>(), this);
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	public void SetupSwing(Transform anchor, Transform _trans) {
		anchorTransform = anchor;
		dependOnTransform = true;
		Vector3 relativePos = transform.RelativePosTo(anchor);
		Internal_SetupSwing(relativePos, _trans);
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	public void SetupSwing(Vector3 anchor, Transform _trans) {
		anchorPos = anchor;
		anchorTransform = null;
		dependOnTransform = false;
		Vector3 relativePos = anchor - transform.position;
		Internal_SetupSwing(relativePos, _trans);
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	private void Internal_SetupSwing(Vector3 relativePos,Transform _trans) {
		initialAngle = Vector3.SignedAngle(Vector3.down, -relativePos.ZeroZ(), Vector3.forward) * Mathf.Deg2Rad;
		float max = MaxSwingAngleRad;
		initialAngle = Mathf.Clamp(initialAngle, -max, max);
		initialAngleDirection = (sbyte)(initialAngle > 0 ? 1 : -1);
		//distanceFromAnchor = relativePos.magnitude;
		anim.BeginSwing(_trans);
		anchorTime = Time.time;
		WhipDistance = relativePos.magnitude;
		initialState = Mathf.Sqrt(gravityMult / WhipDistance);
		firstFrame = true;
	}
	protected override void OnFixedUpdate() {
		if(!overriding)
			return;
		thisRB.position = UpdateSwing();
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	public Vector3 UpdateSwing() {
		firstFrame = false;
		float angleCos = Mathf.Cos(initialState * GetTime);
		float angleSin = Mathf.Sin(initialState * GetTime);
		float newAngle = initialAngle * angleCos;
		var anchorPos = dependOnTransform ? anchorTransform.position : this.anchorPos;
		anim.UpdateStatus(newAngle);
		movingRight = initialAngleDirection * angleSin < 0; //If the sine is negative, it's moving right

		return new Vector3(
			anchorPos.x + (Mathf.Sin(newAngle) * WhipDistance),
			anchorPos.y - (Mathf.Cos(newAngle) * WhipDistance),
			transform.position.z);
	}

	public Vector3 GetVelocity() {
		float velocity = initialAngle * Mathf.Sqrt(WhipDistance * gravityMult) * Mathf.Sin(initialState * GetTime);
		float newAngle = initialAngle * Mathf.Cos(initialState * GetTime);

		Vector3 addedVelocity = Vector3.zero;
		if(dependOnTransform)
		{
			var rb = anchorTransform?.GetComponent<Rigidbody>();
			if(rb != null)
				addedVelocity = rb.velocity.ZeroZ();
		}
		return new Vector3(
			velocity * -Mathf.Cos(newAngle),
			velocity * -Mathf.Sin(newAngle),
			0f) + addedVelocity;
	}

	public void BreakSwing() {
		Release(swingOverrider);
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	public void Attach(IMoveOverrideable user) {
		user.Attach(this);

		var player = user as Controllable;
		if(player != null)
			ControllerHandler.Instance.OverrideWithUser(this, player);

		overriding = true;
		prevGravitystate = thisRB.useGravity;
		thisRB.useGravity = false;
		thisRB.isKinematic = true;
	}

	public void Release(IMoveOverrideable user) {
		var velocity = GetVelocity();
		user.Release(this);
		anim.EndSwing();

		var player = user as Controllable;
		if(player != null)
			ControllerHandler.Instance.UndoOverrideWithUser(this, player, true);

		overriding = false;
		thisRB.useGravity = prevGravitystate;
		thisRB.velocity = velocity;
		momentum.velocity = velocity;
		thisRB.isKinematic = false;
	}

	public void StartSwing() {
		Attach(swingOverrider);
	}

	private void ChangeSpeed(float direction) {
		if(direction == 0)
			return;

		bool accelerating = direction > 0 ^ !movingRight; //XOR. Acelerar si se mueve a derecha y balancea a derecha, o el mismo caso en otra direccion.
		#region Comment
		/*
		 * initialAngle: Determina el angulo maximo del balanceo, por lo que es lo que se va a modificar
		 * Multiplicacion:
		 *		initialAngleDirection: para mantener los siguientes multiplicadores relativos a direccion de balanceo
		 *		(accelerating ? 1 : -1): para sumar si acelera, o restar si desacelera
		 *		Mathf.Abs(direction): escala absoluta la accion segun la fuerza del input (anticipa soporte de joystick)
		 *		swingAddingStrength: multiplicador de fuerza de aceleracion. variable publica asignable
		 *		Time.deltaTime: consistencia de framerate
		*/
		#endregion
		float newMax = initialAngle + (initialAngleDirection * (accelerating ? 1 : -1) * Mathf.Abs(direction) * swingAddingStrength * Time.deltaTime);
		initialAngle = initialAngleDirection * Mathf.Clamp(Mathf.Abs(newMax), 0, MaxSwingAngleRad);
	}

	public override void Move(Vector2 direction) {
		if(direction.x == 0)
			return;
		if(direction.x > 0)
			transform.localRotation = Quaternion.Euler(0, 90, 0);
		else
			transform.localRotation = Quaternion.Euler(0, -90, 0);
		anim.UpdateWithLast();
		ChangeSpeed(direction.x);
	}

	private void ForceRelease() {
		if(overriding)
			BreakSwing();
	}

	private void ForceRelease(IDamager dmg) => ForceRelease();

	private void OnCollisionEnter() {
		ForceRelease();
	}

	private void OnEnable() {
		var chara = GetComponent<IDamageable>();
		if(chara != null)
			chara.OnDeath += ForceRelease;
	}

	private void OnDisable() {
		var chara = GetComponent<IDamageable>();
		if(chara != null)
			chara.OnDeath -= ForceRelease;
	}
}