using UnityEngine;
using UnityEngine.Events;

public class WorldSaveActuator : MonoBehaviour {

	public string dataId;
	public bool state = false;

	public UnityEvent OnTrueExecute;

	// Start is called before the first frame update
	void Start() {
		WorldDataManager.Instance.TryLoadElseWriteDefault(dataId, ref state);

		if(state)
			OnTrueExecute?.Invoke();
	}

}
