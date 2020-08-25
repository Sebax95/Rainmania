using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using CustomMSLibrary.Unity;

public class GameObjectSelector : EditorWindow {

	private (bool enabled, LayerMask mask) selectedLayers;
	private (bool enabled, string tag) selectedTag;

	[MenuItem("Custom Tools/RainMania Tools/Game Object Selector")]
	public static void Open() {
		var wind = GetWindow<GameObjectSelector>();
		wind.Show();
	}

	private void OnEnable() {
		var root = rootVisualElement;
		{
			root.Add(UiUtils.QuickLabel("Filters", 20));
			root.Add(UI_LayerTagContainer());

			root.Add(new Button(CommitSelection));
		}
	}

	private void CommitSelection() {
		var roots = SceneManager.GetActiveScene().GetRootGameObjects();
		var newSelect = roots.Where(x => !selectedLayers.enabled || selectedLayers.mask.ContainsLayer(x.layer))
			.Where(x => !selectedTag.enabled || x.CompareTag(selectedTag.tag))
			.ToArray();

		Selection.objects = newSelect;
	}

	#region UI structure

	private VisualElement UI_LayerTagContainer() {
		var horizontalContainer = new VisualElement();
		horizontalContainer.style.alignItems = Align.Stretch;
		horizontalContainer.style.flexDirection = FlexDirection.Row;
		{
			horizontalContainer.Add(UI_Toggler_LayerSelector());
			horizontalContainer.Add(UI_LayerSelector());
			horizontalContainer.Add(new ToolbarSpacer());
			horizontalContainer.Add(UI_Toggler_TagSelector());
			horizontalContainer.Add(UI_TagSelector());
		}

		return horizontalContainer;
	}

	private Toggle UI_Toggler_LayerSelector() {
		var toggler = new Toggle();
		toggler.RegisterValueChangedCallback(x => selectedLayers.enabled = x.newValue);

		return toggler;
	}

	private MaskField UI_LayerSelector() {
		var layers = new MaskField(new List<string>(InternalEditorUtility.layers), 0);
		layers.RegisterValueChangedCallback(x => selectedLayers.mask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(x.newValue));
		layers.SetFlex(0.5f);
		return layers;
	}

	private Toggle UI_Toggler_TagSelector() {
		var toggler = new Toggle();
		toggler.RegisterValueChangedCallback(x => selectedTag.enabled = x.newValue);
		return toggler;
	}
	private TagField UI_TagSelector() {
		var tags = new TagField();
		tags.RegisterValueChangedCallback(x => selectedTag.tag = x.newValue);
		tags.SetFlex(0.5f);
		return tags;
	}

	#endregion
}

public static class UiUtils {
	public static void SetFlex(this VisualElement elem, float amount) {
		elem.style.flexGrow = amount;
		elem.style.flexShrink = amount;
	}

	public static Label QuickLabel(string text, float size) {
		Label label = new Label(text);
		label.style.fontSize = size;
		return label;
	}
}
