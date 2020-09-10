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
		if(Input.GetKeyDown(baseController.JumpKey))
		{
			User?.BreakSwing();
			playerPawn.ForceJump();
		}
		User?.Move(baseController.GetAxises());
	}

	protected override void DoActions() {
		if(!ControllerHandler.Instance.IsOverriding(this))
			return;

		if(Input.GetKeyDown(baseController.attackWhipKey) && User && !User.firstFrame)
		{
			User.BreakSwing();
		}
	}



	//public override void AssignUser(Controllable user) {
	//	base.AssignUser(user);
	//	swingerPawn = basePawn as Swinger;
	//}
}