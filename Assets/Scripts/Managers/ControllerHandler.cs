using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Core;

public class ControllerHandler : MonoBehaviour {
	private Dictionary<Controller, Controllable> controllerToCharacter = new Dictionary<Controller, Controllable>();
	private Dictionary<Controllable, Controller> characterToController = new Dictionary<Controllable, Controller>();

	private Dictionary<Controllable, Controller> userOverriddenBy = new Dictionary<Controllable, Controller>();
	private Dictionary<Controller, Controllable> controllerOverriding = new Dictionary<Controller, Controllable>();
	
	public static ControllerHandler Instance { get; private set; }

	private void Awake() {
		if(Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(this);
		} else
			Destroy(gameObject);

	}

	public void RequestAssignation(Controller control, Controllable user) {
		if(controllerToCharacter.ContainsValue(user))
			return;
		controllerToCharacter.Add(control, user);
		characterToController.Add(user, control);
		//control.AssignUser(user);
	}

	public Controller FindController(Controllable source) {
		if(characterToController.TryGetValue(source, out var value))
			return value;

		return null;
	}

	public Controllable GetUser(Controller controller) {
		if(controllerToCharacter.TryGetValue(controller, out var value))
			if(!userOverriddenBy.ContainsKey(value))
				return value;
		return null;
	}

	public void OverrideWithUser(Controllable overrider, Controllable target) {
		OverrideController(FindController(overrider), target);
	}

	public void UndoOverrideWithUser(Controllable overrider, Controllable target, bool delayed = false) {
		UndoOverride(FindController(overrider), target, delayed);
	}

	public void OverrideController(Controller overrider, Controllable target) {
		userOverriddenBy[target] = overrider;
		controllerOverriding[overrider] = target;
	}

	public void UndoOverride(Controller overrider, Controllable target, bool delayed = false) {
		if(delayed)
		{
			StartCoroutine(Coroutine_DelayedUndoOverride(overrider, target));
			return;
		}

		if(userOverriddenBy.ContainsKey(target) && userOverriddenBy[target] == overrider)
		{
			userOverriddenBy.Remove(target);
			controllerOverriding.Remove(overrider);
		}
	}

	public bool IsOverriding(Controller controller) => controllerOverriding.ContainsKey(controller);

	private IEnumerator Coroutine_DelayedUndoOverride(Controller overrider, Controllable target) {
		yield return null;

		if(userOverriddenBy.ContainsKey(target) && userOverriddenBy[target] == overrider)
		{
			userOverriddenBy.Remove(target);
			controllerOverriding.Remove(overrider);
		}
	}

	public void DestroyController(Controllable item) {
		if(!characterToController.ContainsKey(item))
			return;
		var control = characterToController[item];
		if(control)
			control.DestroyController();
		RemoveAssociation(item);
	}

	public void RemoveAssociation(Controllable item) {
		if(!characterToController.ContainsKey(item))
			return;
		var control = characterToController[item];
		characterToController.Remove(item);
		controllerToCharacter.Remove(control);
	}
}
