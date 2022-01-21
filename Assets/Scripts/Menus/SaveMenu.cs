using UnityEngine;

public class SaveMenu : MonoBehaviour {

	public static SaveMenu Instance { get; private set; }

	private void Start() {
		Instance = this;
		gameObject.SetActive(false);
	}

	public void DoSave(int saveSlot) {
		SaveManager.SaveData(saveSlot);
		//TODO: Animaciones? "Guardado completado", etc
		gameObject.SetActive(false);
	}

}
