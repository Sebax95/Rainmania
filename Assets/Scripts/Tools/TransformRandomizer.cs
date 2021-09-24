using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
//using UnityEngine.UIElements.StyleEnums;
using UnityEngine.UIElements.StyleSheets;
using CustomMSLibrary.Core;
using CustomMSLibrary.Unity;

public class TransformRandomizer : EditorWindow {

	private static (Vector3 from, Vector3 to, bool active) positionValues;
	private static (Vector3 from, Vector3 to, bool active) rotationValues;
	private static (Vector3 from, Vector3 to, bool active) scaleValues;

	private const int PADDING = 4;
	private const int HALF_PADDING = 2;

	[MenuItem("Custom Tools/Null's Kit/Transform Randomizer")]
	public static void ShowExample() {
		TransformRandomizer wnd = GetWindow<TransformRandomizer>();
		wnd.titleContent = new GUIContent("TransformRandomizer");
		wnd.minSize = new Vector2(200, 300);
	}

	public void OnEnable() {
		VisualElement root = rootVisualElement;
		root.style.paddingLeft = PADDING;
		root.style.paddingRight = PADDING;
		root.style.paddingTop = PADDING;
		root.style.paddingBottom = PADDING;


		#region Position
		root.Add(new Label("Position") {
			style = {
				fontSize = 14,
				paddingTop = PADDING,
				paddingBottom = PADDING
			}
		});

		var posTab = new VisualElement() {
			style = {
				flexDirection = FlexDirection.Row,
				alignContent = Align.Center,
				justifyContent = Justify.Center,
				paddingTop = PADDING,
				paddingBottom = PADDING
			}
		};

		var posTabFrom = new VisualElement() {
			style = {
				flexDirection = FlexDirection.Column,
				flexShrink = 0.5f,
				flexGrow =0.5f,
				paddingRight = PADDING
			}
		};
		var subGroupStyle = posTabFrom.style;

		posTabFrom.Add(new Label("From") {
			style = {
				fontSize = 10,
				paddingTop = PADDING,
				paddingBottom = PADDING
			}
		});
		var vecFrom = new Vector3Field {
			value = positionValues.from
		};
		vecFrom.RegisterValueChangedCallback(x => positionValues.from = x.newValue);
		//vecFrom.OnValueChanged(x => positionValues.from = x.newValue);
		posTabFrom.Add(vecFrom);
		posTab.Add(posTabFrom);
		var posTabTo = new VisualElement() {
			style = {
				flexDirection = FlexDirection.Column,
				flexShrink = 0.5f,
				flexGrow =0.5f,
				paddingLeft = PADDING
			}
		};
		posTabTo.Add(new Label("To") {
			style = {
				fontSize = 10,
				paddingTop = PADDING,
				paddingBottom = PADDING,
				paddingLeft = PADDING,
			}
		});
		var vecTo = new Vector3Field {
			value = positionValues.to
		};
		vecTo.RegisterValueChangedCallback(x => positionValues.to = x.newValue);
		//vecTo.OnValueChanged(x => positionValues.to = x.newValue);

		posTabTo.Add(vecTo);
		posTab.Add(posTabTo);
		root.Add(posTab);
		#endregion
		//----------
		#region Rotation
		root.Add(new Label("Rotation") {
			style = {
				fontSize = 14,
				paddingTop = PADDING,
				paddingBottom = PADDING
			}
		});

		var rotTab = new VisualElement() {
			style = {
				flexDirection = FlexDirection.Row,
				alignContent = Align.Center,
				justifyContent = Justify.Center,
				paddingTop = PADDING,
				paddingBottom = PADDING
			}
		};

		var rotTabFrom = new VisualElement() {
			style = {
				flexDirection = FlexDirection.Column,
				flexShrink = 0.5f,
				flexGrow =0.5f,
				paddingRight = PADDING
			}
		};
		rotTabFrom.Add(new Label("From") {
			style = {
				fontSize = 10,
				paddingTop = PADDING,
				paddingBottom = PADDING,
				paddingRight = PADDING,
			}
		});
		var rotFrom = new Vector3Field {
			value = rotationValues.from

		};
		rotFrom.RegisterValueChangedCallback(x => rotationValues.from = x.newValue);
		//rotFrom.OnValueChanged(x => rotationValues.from = x.newValue);
		rotTabFrom.Add(rotFrom);
		rotTab.Add(rotTabFrom);

		var rotTabTo = new VisualElement() {
			style = {
				flexDirection = FlexDirection.Column,
				flexShrink = 0.5f,
				flexGrow =0.5f,
				paddingLeft = PADDING
			}
		};
		rotTabTo.Add(new Label("To") {
			style = {
				fontSize = 10,
				paddingTop = PADDING,
				paddingBottom = PADDING,
				paddingLeft = PADDING,
			}
		});
		var rotTo = new Vector3Field {
			value = rotationValues.to
		};
		rotTo.RegisterValueChangedCallback(x => rotationValues.to = x.newValue);

		rotTabTo.Add(rotTo);
		rotTab.Add(rotTabTo);
		root.Add(rotTab);
		#endregion
		//----------
		#region Scale
		root.Add(new Label("Scale") {
			style = {
				fontSize = 14,
				paddingTop = PADDING,
				paddingBottom = PADDING
			}
		});

		var scalTab = new VisualElement() {
			style = {
				flexDirection = FlexDirection.Row,
				alignContent = Align.Center,
				justifyContent = Justify.Center,
				paddingTop = PADDING,
				paddingBottom = PADDING
			}
		};

		var scalTabFrom = new VisualElement() {
			style = {
				flexDirection = FlexDirection.Column,
				flexShrink = 0.5f,
				flexGrow =0.5f,
				paddingRight = PADDING
			}
		};
		scalTabFrom.Add(new Label("From") {
			style = {
				fontSize = 10,
				paddingTop = PADDING,
				paddingBottom = PADDING,
				paddingRight = PADDING,
			}
		});
		var scalFrom = new Vector3Field {
			value = scaleValues.from

		};
		scalFrom.RegisterValueChangedCallback(x => scaleValues.from = x.newValue);
		//scalFrom.OnValueChanged(x => scaleValues.from = x.newValue);

		scalTabFrom.Add(scalFrom);
		scalTab.Add(scalTabFrom);

		var scalTabTo = new VisualElement() {
			style = {
				flexDirection = FlexDirection.Column,
				flexShrink = 0.5f,
				flexGrow =0.5f,
				paddingLeft = PADDING
			}
		};
		scalTabTo.Add(new Label("To") {
			style = {
				fontSize = 10,
				paddingTop = PADDING,
				paddingBottom = PADDING,
				paddingLeft = PADDING,
			}
		});
		var scalTo = new Vector3Field {
			value = scaleValues.to
		};
		scalTo.RegisterValueChangedCallback(x => scaleValues.to = x.newValue);
		//scalTo.OnValueChanged(x => scaleValues.to = x.newValue);

		scalTabTo.Add(scalTo);
		scalTab.Add(scalTabTo);
		root.Add(scalTab);
		#endregion

		#region Randomize button
		float aThird = 1f / 3f;
		root.Add(new Label("Randomize") { style = { fontSize = 14 } });

		var buttonArea = new VisualElement() {
			style = {
				flexDirection = FlexDirection.Row,
				
			}
		};
		buttonArea.Add(new Button(RandomizePos) {
			text = "Position",

			style = {
				flexShrink = aThird,
				flexGrow = aThird,
				marginRight = HALF_PADDING,
				unityBackgroundImageTintColor = MiscUnityUtilities.ParseColor("FF3B31")
			}
		});

		buttonArea.Add(new Button(RandomizeRot) {
			text = "Rotation",
			style = {
				flexShrink = aThird,
				flexGrow = aThird,
				marginLeft = HALF_PADDING,
				marginRight = HALF_PADDING,
				unityBackgroundImageTintColor = MiscUnityUtilities.ParseColor("73E83A")

			}
		});

		buttonArea.Add(new Button(RandomizeScal) {
			text = "Scale",
			style = {
				flexShrink = aThird,
				flexGrow = aThird,
				marginLeft = HALF_PADDING,
				unityBackgroundImageTintColor = MiscUnityUtilities.ParseColor("3B8DEB")
			}
		});
		root.Add(buttonArea);

		root.Add(new Button(RandomizeAll) {
			text = "All"
		});

		#endregion
	}

	private void Randomize(BoolByte affected) {
		var selection = Selection.transforms;
		int c = selection.Length;
		for(int i = 0; i < c; i++)
		{
			if(affected[0])
				selection[i].position = GetRandomVector(positionValues.from, positionValues.to);
			if(affected[1])
				selection[i].rotation = Quaternion.Euler(GetRandomVector(rotationValues.from, rotationValues.to));
			if(affected[2])
				selection[i].localScale = GetRandomVector(scaleValues.from, scaleValues.to);
		}
	}

	private Vector3 GetRandomVector(Vector3 from, Vector3 to) =>
		new Vector3(Random.Range(from.x, to.x),
					Random.Range(from.y, to.y),
					Random.Range(from.z, to.z));

	private void RandomizePos() => Randomize(new BoolByte(true, false, false));
	private void RandomizeRot() => Randomize(new BoolByte(false, true, false));
	private void RandomizeScal() => Randomize(new BoolByte(false, false, true));
	private void RandomizeAll() => Randomize(new BoolByte(true, true, true));
}