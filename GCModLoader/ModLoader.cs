using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using GoodCompany.GUI;
using BepInEx;
using HarmonyLib;
using Newtonsoft.Json;
using UnityEngine;

namespace GCModLoader
{
	[BepInPlugin("com.github.seppahbaws.gcmodloader", "Good Company Mod Loader", "1.0.0")]
	[BepInProcess("GoodCompany.exe")]
	public class ModLoader : BaseUnityPlugin
	{
		public static int ModsLoaded { get; private set; }
		public static readonly string ModLoaderVersion = "0.1.0";

		void Awake()
		{
			InitializeModLoader();
		}

		// [DllImport("kernel32.dll", SetLastError = true)]
		// [return: MarshalAs((UnmanagedType.Bool))]
		// static extern bool AllocConsole();
		//
		// public static void Main(string[] args)
		// {
		// 	AllocConsole();
		//
		// 	InitializeModLoader();
		// }

		public static void InitializeModLoader()
		{
			ModLogger.Init(GetLoaderRootFolder() + "\\Output.log");

			ModLogger.Log("Good Company Mod Loader initialized!");

			Harmony harmony = new Harmony("com.github.seppahbaws.gcmodloader");
			Assembly assembly = Assembly.GetExecutingAssembly();
			harmony.PatchAll(assembly);
			// harmony.PatchAll();

			ModLogger.LogInfo($"Patched {harmony.GetPatchedMethods().ToArray().Length} methods.");

			foreach (var method in harmony.GetPatchedMethods())
			{
				ModLogger.Log($"Patched method: {method}");
			}

			ModLogger.LogInfo("All mods loaded");

			// Harmony harmony = new Harmony("com.github.seppahbaws.gcmodloader");
			// Assembly assembly = Assembly.LoadFile("E:\\SSDLibrary\\steamapps\\common\\Good Company\\BepInEx\\plugins\\GoodCompanyTestMod.dll");
			// ModLogger.Log($"Mod name: {assembly.FullName}");
			// harmony.PatchAll(assembly);

			LogLoadedAssemblies();

			// ModLogger.Log($"DOORSTOP_PROCESS_PATH = {Environment.GetEnvironmentVariable("DOORSTOP_PROCESS_PATH")}");

			ModLogger.LogInfo($"mod loader rood folder: \"{GetLoaderRootFolder()}\"");
			ModLogger.LogInfo($"mods folder: \"{GetModsFolder()}\"");

			LoadMods(harmony);
		}

		public static void LogLoadedAssemblies(bool list = true)
		{
			ModLogger.Log($"There are currently {AppDomain.CurrentDomain.GetAssemblies().ToArray().Length} assemblies loaded:");

			if (!list) return;

			foreach (var loadedAssembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				ModLogger.Log(loadedAssembly.FullName);
			}
		}

		public static string GetModsFolder()
		{
			string exePath;
			try
			{
				exePath = Environment.GetEnvironmentVariable("DOORSTOP_PROCESS_PATH");
			}
			catch (System.NullReferenceException e)
			{
				ModLogger.LogException(e.ToString());
				return "";
			}

			return exePath.Substring(0, exePath.LastIndexOf('\\')) + "\\GCModLoader\\Mods";
		}

		public static string GetLoaderRootFolder()
		{
			string exePath;
			try
			{
				exePath = Environment.GetEnvironmentVariable("DOORSTOP_PROCESS_PATH");
			}
			catch (System.NullReferenceException e)
			{
				ModLogger.LogException(e.ToString());
				return "";
			}

			return exePath.Substring(0, exePath.LastIndexOf('\\')) + "\\GCModLoader";
		}

		public static void LoadMods(Harmony harmony)
		{
			List<string> files = Directory.GetFiles(GetModsFolder()).ToList();
			List<string> mods = new List<string>();

			string configText = File.ReadAllText(GetLoaderRootFolder() + "\\ModsConfig.json");
			ModsConfig config = JsonConvert.DeserializeObject<ModsConfig>(configText);

			foreach (string file in files)
			{
				if (file.LastIndexOf(".dll") > -1)
				{
					mods.Add(file);
				}
			}

			List<Mod> configMods = config.Mods.ToList();
			List<string> loadedMods = new List<string>();

			ModLogger.LogInfo($"Mods found in config: {configMods.Count}");

			// Load mods
			foreach (Mod mod in configMods)
			{
				ModLogger.Log($"{mod.Name} ({mod.File}) enabled: {mod.Enabled}");
				if (!mod.Enabled) continue;

				string fullModFile = GetModsFolder() + "\\" + mod.File;
				int i = mods.FindLastIndex(x => x == fullModFile);
				if (i <= -1) continue;

				Assembly loadedMod = Assembly.LoadFile(fullModFile);
				harmony.PatchAll(loadedMod);
				loadedMods.Add(mod.Name);
			}

			ModsLoaded = loadedMods.Count;
			ModLogger.LogInfo($"Loaded {ModsLoaded} mods:");
			foreach (string loadedMod in loadedMods)
			{
				ModLogger.LogInfo(loadedMod);
			}

			LogLoadedAssemblies();
		}
	}
}
