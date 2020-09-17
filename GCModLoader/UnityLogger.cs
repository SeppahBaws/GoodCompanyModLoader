// using System;
// using HarmonyLib;
// using Unity;
//
// namespace GCModLoader
// {
// 	class UnityLogger
// 	{
// 		public static void Main(string[] args)
// 		{
// 			UnityEngine.Debug.Log("Hello There");
// 		}
// 	}
//
// 	[HarmonyPatch(typeof(UnityEngine.Debug), "Log")]
// 	public class Debug_Log_Patch
// 	{
// 		static void Postfix(object message)
// 		{
// 			ModLogger.Log($"[UNITY MESSAGE]   {message}");
// 		}
// 	}
//
// 	[HarmonyPatch(typeof(UnityEngine.Debug), "LogFormat")]
// 	public class Debug_LogFormat_Patch
// 	{
// 		static void Postfix(string format, params object[] args)
// 		{
// 			ModLogger.Log($"AAAAAAAAH");
// 		}
// 	}
//
// 	[HarmonyPatch(typeof(UnityEngine.Debug), "LogFormat")]
// 	public class Debug_LogError_Patch
// 	{
// 		static void Postfix(object message)
// 		{
// 			ModLogger.Log($"[UNITY ERROR]     {message}");
// 		}
// 	}
//
// 	[HarmonyPatch(typeof(UnityEngine.Debug), "LogException")]
// 	public class Debug_LogException_Patch
// 	{
// 		static void Postfix(Exception exception)
// 		{
// 			ModLogger.Log($"[UNITY EXCEPTION] {exception}");
// 		}
// 	}
//
// 	[HarmonyPatch(typeof(UnityEngine.Debug), "LogWarning")]
// 	public class Debug_LogWarning_Patch
// 	{
// 		static void Postfix(object message)
// 		{
// 			ModLogger.Log($"[UNITY WARNING]   {message}");
// 		}
// 	}
// }
