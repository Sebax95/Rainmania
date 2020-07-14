using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class GameObjectSelector : EditorWindow {

	private int selectedLayer;
	private int selectedTag;

	[MenuItem("Custom Tools/RainMania Tools/Game Object Selector")]
	public static void Open() {
		var wind = GetWindow<GameObjectSelector>();
		wind.Show();
	}

	private void OnEnable() {
		var root = rootVisualElement;
		{
			root.Add(UI_LayerTagContainer());

		}
	}



	#region UI structure

	private VisualElement UI_LayerTagContainer() {
		var horizontalContainer = new VisualElement();
		horizontalContainer.style.alignItems = Align.Stretch;
		horizontalContainer.style.flexDirection = FlexDirection.Row;
		{
			horizontalContainer.Add(UI_LayerSelector());
		}

		return horizontalContainer;
	}

	private LayerField UI_LayerSelector() {
		var layers = new LayerField();
		layers.RegisterValueChangedCallback(x => selectedLayer = x.newValue);
		return layers;
	}

	#endregion
}
