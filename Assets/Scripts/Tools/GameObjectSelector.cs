using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using CustomMSLibrary.Core;
using CustomMSLibrary.Unity;

public class GameObjectSelector : EditorWindow {

	private (bool enabled, LayerMask mask) selectedLayers;
	private (bool enabled, string tag) selectedTag;
	private bool isRecursiveSearch;

	private enum Axis { x, y, z }

	/// <summary>
	/// Enabled axises:  [0: Any] [1: x] [2: y] [3: z]
	///</summary>
	private (BoolByte enabledAxises, Vector3 min, Vector3 max) rotationRange;

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
			root.Add(UI_RotationContainer());

			root.Add(UI_CommitButton());
		}
	}

	private void CommitSelection() {
		var currentSelection = Selection.objects;
		IEnumerable<GameObject> roots;
		IEnumerable<GameObject> newSelect;
		if(isRecursiveSearch)
		{
			roots = SceneManager.GetActiveScene().GetRootGameObjects();
			var temp = roots.Select(go => go.transform).SelectMany(RecursiveSearch).Select(tra => tra.gameObject);
			newSelect = Filter(temp);
		} else
		{
			roots = currentSelection == null || currentSelection.Length == 0 ?
				SceneManager.GetActiveScene().GetRootGameObjects() :
				currentSelection.Cast<GameObject>().SelectMany(x => x.transform.GetAllChildrenArray()).Select(x => x.gameObject);

			newSelect = Filter(roots);
		}
		Selection.objects = newSelect.ToArray();
	}

	private IEnumerable<Transform> RecursiveSearch(Transform parent) {
		IEnumerable<Transform> children = Enumerable.Empty<Transform>();
		foreach(Transform child in parent)
		{
			children.Append(child);
		}
		children.Concat(children.SelectMany(RecursiveSearch));
		return children;
	}

	private IEnumerable<GameObject> Filter(IEnumerable<GameObject> items) {
		return items.Where(x => CheckLayer(x) && CheckTag(x) && CheckRotation(x));
	}

	#region UI structure
	#region LayerTag
	private VisualElement UI_LayerTagContainer() {
		var horizontalContainer = new VisualElement();
		horizontalContainer.style.alignItems = Align.Stretch;
		horizontalContainer.style.flexDirection = FlexDirection.Row;
		{
			horizontalContainer.Add(UiUtils.QuickToggle(x => selectedTag.enabled = x.newValue));
			horizontalContainer.Add(UI_TagSelector());
			horizontalContainer.Add(new ToolbarSpacer());
			horizontalContainer.Add(UiUtils.QuickToggle(x => selectedLayers.enabled = x.newValue));
			//horizontalContainer.Add(UI_Toggler_LayerSelector());
			horizontalContainer.Add(UI_LayerSelector());
		}

		return horizontalContainer;
	}

	/*
	//Deprecated
	private Toggle UI_Toggler_LayerSelector() {
		var toggler = new Toggle();
		toggler.RegisterValueChangedCallback(x => selectedLayers.enabled = x.newValue);

		return toggler;
	}
	private Toggle UI_Toggler_TagSelector() {
		var toggler = new Toggle();
		toggler.RegisterValueChangedCallback(x => selectedTag.enabled = x.newValue);
		return toggler;
	}
	*/

	private MaskField UI_LayerSelector() {
		var layers = new MaskField(new List<string>(InternalEditorUtility.layers), 0);
		layers.RegisterValueChangedCallback(x => selectedLayers.mask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(x.newValue));
		layers.SetFlex(0.5f);
		return layers;
	}

	private TagField UI_TagSelector() {
		var tags = new TagField();
		tags.RegisterValueChangedCallback(x => selectedTag.tag = x.newValue);
		tags.SetFlex(0.5f);
		return tags;
	}
	#endregion

	#region Rotation
	private VisualElement UI_RotationContainer() {
		var rotationContainer = new VisualElement();
		{
			rotationContainer.style.flexDirection = FlexDirection.Column;
			rotationContainer.Add(UiUtils.QuickLabel("Rotation", 16));
			rotationContainer.Add(UI_RotationEnabled());
			rotationContainer.Add(UI_RotationAxisesContainer());
		}
		return rotationContainer;
	}

	private VisualElement UI_RotationEnabled() {
		var container = new VisualElement();
		container.style.alignItems = Align.Stretch;
		container.style.flexDirection = FlexDirection.Row;

		container.Add(UiUtils.QuickToggle(x => rotationRange.enabledAxises[0] = x.newValue));
		container.Add(new Label("Rotation enabled status"));

		return container;
	}

	private VisualElement UI_RotationAxisesContainer() {
		var container = new VisualElement();
		container.style.alignItems = Align.Stretch;
		container.style.flexDirection = FlexDirection.Column;

		container.Add(UI_AxisRow("x:",
			(t => rotationRange.enabledAxises[1] = t.newValue),
			(f => UpdateVector(Axis.x, ref rotationRange.min, f.newValue)),
			(t => UpdateVector(Axis.x, ref rotationRange.max, t.newValue))
			));

		container.Add(UI_AxisRow("y:",
			(t => rotationRange.enabledAxises[2] = t.newValue),
			(f => UpdateVector(Axis.y, ref rotationRange.min, f.newValue)),
			(t => UpdateVector(Axis.y, ref rotationRange.max, t.newValue))
			));

		container.Add(UI_AxisRow("z:",
			(t => rotationRange.enabledAxises[3] = t.newValue),
			(f => UpdateVector(Axis.z, ref rotationRange.min, f.newValue)),
			(t => UpdateVector(Axis.z, ref rotationRange.max, t.newValue))
			));

		return container;
	}
	#endregion


	private static VisualElement UI_AxisRow(string label,
		EventCallback<ChangeEvent<bool>> axisToggle,
		EventCallback<ChangeEvent<float>> fromEvent,
		EventCallback<ChangeEvent<float>> toEvent) {

		var container = new VisualElement();
		container.style.alignItems = Align.Stretch;
		container.style.flexDirection = FlexDirection.Row;

		container.Add(UiUtils.QuickToggle(axisToggle));
		container.Add(new Label(label));
		container.Add(UiUtils.QuickFloatField(fromEvent).SetFlex(0.5f));
		container.Add(UiUtils.QuickFloatField(toEvent).SetFlex(0.5f));

		return container;
	}


	private Button UI_CommitButton() =>
		new Button(CommitSelection).SetText("Commit selection");

	#endregion

	#region Logic
	private bool CheckLayer(GameObject item) => !selectedLayers.enabled || selectedLayers.mask.ContainsLayer(item.layer);
	private bool CheckTag(GameObject item) => !selectedTag.enabled || item.CompareTag(selectedTag.tag);

	private bool CheckRotation(GameObject item) {
		BoolByte enableds = rotationRange.enabledAxises;
		if(!enableds[0]) //Should rotation be considered? return true if not
			return true;


		Vector3 rotation = item.transform.localRotation.eulerAngles;
		Vector3 from = rotationRange.min;
		Vector3 to = rotationRange.max;
		return
			(!enableds[1] || IsBetween(from.x, to.x, rotation.x) || IsBetween(from.x, to.x, rotation.x - 360f)) && //If "check for x axis" is disabled, use true for x-axis. Otherwise, check it's between the values
			(!enableds[2] || IsBetween(from.y, to.y, rotation.y) || IsBetween(from.y, to.y, rotation.y - 360f)) && //Repeat for y
			(!enableds[3] || IsBetween(from.z, to.z, rotation.z) || IsBetween(from.z, to.z, rotation.z - 360f)); //Repeat for z
	}

	private void UpdateVector(Axis axis, ref Vector3 vector, float value) {
		switch(axis)
		{
			case Axis.x:
				vector.x = value;
				break;
			case Axis.y:
				vector.y = value;
				break;
			case Axis.z:
				vector.z = value;
				break;
		}
	}

	private static bool IsBetween(float from, float to, float value) => from < value && value < to;
	#endregion
}

public static class UiUtils {
	public static T SetFlex<T>(this T elem, float amount) where T : VisualElement {
		elem.style.flexGrow = amount;
		elem.style.flexShrink = amount;
		return elem;
	}

	public static Label QuickLabel(string text, float size) {
		Label label = new Label(text);
		label.style.fontSize = size;
		return label;
	}

	public static T SetText<T>(this T element, string text) where T : TextElement {
		element.text = text;
		return element;
	}

	public static Toggle QuickToggle(EventCallback<ChangeEvent<bool>> action) {
		var toggler = new Toggle();
		toggler.RegisterValueChangedCallback(action);
		return toggler;
	}

	public static FloatField QuickFloatField(EventCallback<ChangeEvent<float>> action) {
		var field = new FloatField();
		field.RegisterValueChangedCallback(action);
		return field;
	}
}
