using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Assimp;
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
		private List<string> _assetPaths = new List<string>();
		
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

			Process.Start(_filePath);
		}


		private void OnLoadAssets_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog filePicker = new OpenFileDialog {Filter = "Any Assets|*.*"};

			if (filePicker.ShowDialog() == true)
			{
				string assetPath = filePicker.FileName;

				// Check for duplicates
				if (_assetPaths.LastIndexOf(assetPath) > -1)
				{
					MessageBox.Show("This file has already been loaded!");
					return;
				}

				_assetPaths.Add(assetPath);
				LoadedAssetsListbox.Items.Add(assetPath);

				AssimpContext importer = new AssimpContext();

				Scene scene = importer.ImportFile(assetPath, PostProcessSteps.None);
			}
		}
	}
}
