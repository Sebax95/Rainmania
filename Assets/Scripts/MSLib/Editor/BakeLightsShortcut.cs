using UnityEngine;
using UnityEditor;

namespace CustomMSLibrary.Unity.Editor {
	public static class LightingUtilities {

		public static bool CanBake => (!Lightmapping.isRunning) &&
			(Lightmapping.giWorkflowMode == Lightmapping.GIWorkflowMode.OnDemand);

		[MenuItem("Custom Tools/MS Tools/Lighting/Bake Lights", false, 0)]
		public static void BakeLights() {
			if(Lightmapping.isRunning)
				return;

			Lightmapping.BakeAsync();
		}

		[MenuItem("Custom Tools/MS Tools/Lighting/Bake Lights", true, 0)]
		public static bool CanBakeValidate() => CanBake;

		//------------------

		[MenuItem("Custom Tools/MS Tools/Lighting/Stop baking", false, 1)]
		public static void StopBake() {
			if(!Lightmapping.isRunning)
				return;

			Lightmapping.Cancel();
			if(Lightmapping.isRunning)
				Lightmapping.ForceStop();
		}

		[MenuItem("Custom Tools/MS Tools/Lighting/Stop baking", true, 1)]
		public static bool StopBakeValidate() => Lightmapping.isRunning;

		//------------------

		[MenuItem("Custom Tools/MS Tools/Lighting/Toggle Auto-Generate", false, 2)]
		public static void ToggleAutoBake() {
			if(Lightmapping.giWorkflowMode == Lightmapping.GIWorkflowMode.OnDemand)
			{
				Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.Iterative;
				Debug.Log("Baking mode set to Auto-Generate");
			} else
			{
				Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.OnDemand;
				Debug.Log("Baking mode set to Manual");
			}
		}


	}
}

