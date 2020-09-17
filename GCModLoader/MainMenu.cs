using UnityEngine;
using HarmonyLib;
using GoodCompany.GUI;
using TMPro;

namespace GCModLoader
{
	[HarmonyPatch(typeof(MainMenu), "Initialize")]
	class MainMenu_Initialize_Patch
	{
		private static GameObject _modLoaderText;

		static void Postfix(MainMenu __instance,
			GUIButtonMainmenu ____settingsBtn,
			ref TMP_Text ____version)
		{
			// ____version.color = Color.magenta;
			// ____version.margin = Vector4.one;
			// ____version.transform.position = new Vector3(-10, 5, 1);

			MonoBehaviour[] components = ____version.transform.gameObject.GetComponents<MonoBehaviour>();
			foreach (var component in components)
			{
				Debug.Log($"Component: {component}");
			}
			// TMPro.TextMeshProUGUI

			Debug.Log("CLONING VERSION TEXT");
			_modLoaderText = UnityEngine.Object.Instantiate<GameObject>(____version.transform.gameObject, ____version.transform.parent);
			// _modLoaderText = UnityEngine.Object.Instantiate<GameObject>(new GameObject());
			_modLoaderText.name = "ModLoader Text";
			_modLoaderText.transform.position = new Vector3(10, 100, 1);
			RectTransform rect = _modLoaderText.GetComponent<RectTransform>();
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200.0f);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50.0f);

			TMP_Text text = _modLoaderText.GetComponent<TMP_Text>();
			text.text = $"ModLoader v{ModLoader.ModLoaderVersion}\n{ModLoader.ModsLoaded} mods loaded";
			text.color = Color.white;
			text.fontSize = 15;
			text.alignment = TextAlignmentOptions.Left;

			Debug.Log($"Cloned text: {text.text}");
			Debug.Log($"Cloned text position: {_modLoaderText.transform.position}");
			
			Debug.Log(_modLoaderText);
		}
	}
}
