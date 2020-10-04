using UnityEngine;

public class PlayerSwingerController : Controller {

	private PlayerController baseController;
	private Player playerPawn;

	private Swinger User => User<Swinger>();

	private void Start() {
		playerPawn = User.GetComponent<Player>();
		baseController = ControllerHandler.Instance.FindController(playerPawn) as PlayerController;
	}

	protected override void DoMovement() {
		if(!ControllerHandler.Instance.IsOverriding(this))
			return;
		if(Input.GetKeyDown(baseController.JumpKey) || Input.GetKeyDown(KeyCode.Joystick1Button0)) //JOYSTICK TEMPORAL!!!!
		{
			User?.BreakSwing();
			playerPawn.ForceJump();
		}
		User?.Move(baseController.GetAxises());
	}

	protected override void DoActions() {
		if(!ControllerHandler.Instance.IsOverriding(this))
			return;

		if ((Input.GetKeyDown(baseController.attackWhipKey) || Input.GetKeyDown(KeyCode.JoystickButton7)) && User &&
		    !User.firstFrame) //JOYSTICK TEMPORAL!!!!
		{
			User.BreakSwing();
		}
	}



	//public override void AssignUser(Controllable user) {
	//	base.AssignUser(user);
	//	swingerPawn = basePawn as Swinger;
	//}
}