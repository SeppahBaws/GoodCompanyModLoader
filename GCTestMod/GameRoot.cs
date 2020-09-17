using System;
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
		}
	}
}
