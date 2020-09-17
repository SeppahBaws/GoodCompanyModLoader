﻿using HarmonyLib;

namespace GCModLoader
{
	// [HarmonyPatch(typeof(GameRoot), "Awake")]
	// public class GameRoot_Awake_Patch
	// {
	// 	static void Postfix(GameRoot __instance)
	// 	{
	// 		ModLogger.Log("Game Root initialized!");
	// 	}
	// }
	
	// [HarmonyPatch(typeof(GameRoot), "GetVersion")]
	public class GameRoot_GetVersion_Patch
	{
		static void Postfix(GameRoot __instance, ref string __result)
		{
			ModLogger.Log("GameRoot::GetVersion called!");
	
			__result += " (Mods Ready)";
			// __result = "Mod Loader";
		}
	}
}
