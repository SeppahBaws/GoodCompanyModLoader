using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HarmonyLib;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace GoodCompanyModLoader
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string _filePath = "";
		private ModConfig _modConfig;
		
		public MainWindow()
		{
			InitializeComponent();

			InitializeModLoader();
		}

		private void InitializeModLoader()
		{
			string json = File.ReadAllText(@"D:\GoodCompany\GoodCompanyModLoader\GoodCompanyModLoader\GCModLoaderConfig.json");

			_modConfig = JsonConvert.DeserializeObject<ModConfig>(json);
			_filePath = _modConfig.GamePath;
			OnFilePicked();
		}

		private void PickFile(object sender, RoutedEventArgs e)
		{
			OpenFileDialog filePicker = new OpenFileDialog {Filter = "Good Company|GoodCompany.exe"};

			if (filePicker.ShowDialog() == true)
			{
				_filePath = filePicker.FileName;
				OnFilePicked();
			}
		}

		private void OnFilePicked()
		{
			FilePickerText.Text = _filePath;
		}

		private void LaunchGame(object sender, RoutedEventArgs e)
		{
			if (_filePath == "")
			{
				MessageBox.Show("No exe file loaded!");
				return;
			}

			Process game = Process.Start(_filePath);

			var harmony = new Harmony("com.SeppahBaws.GoodCompanyModLoader");
			// var assembly = Assembly.LoadFile("<PATH TO MOD>");
			// var assembly = Assembly.LoadFile(@"E:\SSDLibrary\steamapps\common\Good Company\BepInEx\plugins\GoodCompanyTestMod.dll");
			var assembly = Assembly.GetExecutingAssembly();
			harmony.PatchAll(assembly);
		}
	}
}
