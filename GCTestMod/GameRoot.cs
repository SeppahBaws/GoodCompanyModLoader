using System;
// using Assimp;
using HarmonyLib;
using UnityEngine;

namespace GCTestMod
{
	// [HarmonyPatch(typeof(GameRoot), "Awake")]
	// public class GameRoot_Awake_Patch
	// {
	// 	private static GameObject _testModManager;
	// 	
	// 	static void Postfix(GameRoot __instance)
	// 	{
	// 		_testModManager = UnityEngine.GameObject.Instantiate(new GameObject("SeppahBaws_TestModManager"));
	// 		ModManager manager = _testModManager.AddComponent<ModManager>();
	// 		manager.SetGameRoot(__instance);
	// 	}
	// }
	
	[HarmonyPatch(typeof(GameRoot), "GetVersion")]
	public class GameRoot_GetVersion_Patch
	{
		static void Postfix(GameRoot __instance, ref string __result)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("[TestMod] GameRoot::GetVersion called!");
			Console.ForegroundColor = ConsoleColor.White;
			__result += " TEST MOD!!!!";
			// __result = "Mod Loader";

			// string assetPath = @"D:\Program Files (x86)\Steam\steamapps\common\Good Company\GCModLoader\Assets\TestBox.fbx";
			// AssimpContext importer = new AssimpContext();
			//
			// Scene scene = importer.ImportFile(assetPath, PostProcessSteps.None);
			// Debug.Log($"[TestMod] loaded mesh with {scene.Meshes[0].Vertices.Count} vertices:");
			// foreach (Vector3D vector in scene.Meshes[0].Vertices)
			// {
			// 	Debug.Log($"({vector.X}, {vector.Y}, {vector.Z})");
			// }
		}
	}
}
