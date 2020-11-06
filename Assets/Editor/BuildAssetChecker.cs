using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class BuildAssetChecker : EditorWindow
{
	[MenuItem("Custom Tools/MS Tools/Dependecy Check")]
	public static void Open() {
		var w = GetWindow<BuildAssetChecker>();
	}

	private void OnEnable() {
		var root = rootVisualElement;
		var button = new Button(Process);
		button.text = "Process non-dependencies";
		root.Add(button);
	}

	private void Process() {
		char l = System.IO.Path.DirectorySeparatorChar;
		string path = Directory.GetParent(Application.dataPath) + "/Build";
		//var date = System.DateTime.Now;
		//string filePath = path + $"{date.Year}-{date.Month}-{date.Day} {date.TimeOfDay}";
		if(!Directory.Exists(path))
			Directory.CreateDirectory(path);
		//var buildThread = new Thread(new ParameterizedThreadStart(DoBuild));
		//buildThread.Start(path);
		DoBuild(path);
	}

	private void DoBuild(object pathObj) {
		string path = pathObj as string;
		var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions() {
			locationPathName = path,
			target = BuildTarget.StandaloneWindows,
			scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
			targetGroup = BuildTargetGroup.Standalone,
			options = BuildOptions.Development
		});
		OnFinish(report);
	}

	private void OnFinish(BuildReport report) {
		StringBuilder longText = new StringBuilder();
		var assets = report.packedAssets;
		int length = assets.Length;
		for(int i = 0; i < length; i++)
		{
			longText.AppendLine(assets[i].shortPath);
		}

		string path = Application.dataPath + "/AssetData";
		if(!Directory.Exists(path))
			Directory.CreateDirectory(path);
		var date = System.DateTime.Now;
		string filePath = path + $"/{date.Year}-{date.Month}-{date.Day} {date.Hour}:{date.Minute}:{date.Second}:{date.Millisecond}.txt";
		using(var writer = File.CreateText(filePath))
		{
			writer.Write(longText.ToString());
			writer.Close();
		}
	}
}
