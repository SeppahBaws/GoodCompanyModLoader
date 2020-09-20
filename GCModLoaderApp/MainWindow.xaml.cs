using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Assimp;
using Microsoft.Win32;
using Newtonsoft.Json;
using GCAssetReader;

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
			string json = File.ReadAllText(@"D:\GoodCompany\GoodCompanyModLoader\GCModLoaderApp\GCModLoaderConfig.json");

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

		private string _modelToEncode = "";
		private string _textureToEncode = "";

		private void Converter_LoadModel_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog filePicker = new OpenFileDialog { Filter = "3d models|*.dae;*.ply;*.fbx;*.glb;*.gltf;*.obj;*.x3d" };

			if (filePicker.ShowDialog() == true)
			{
				_modelToEncode = filePicker.FileName;
				ModelToEncodeBox.Text = _modelToEncode;
			}
		}

		private void Converter_LoadTexture_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog filePicker = new OpenFileDialog { Filter = "Image Files|*.JPG;*.PNG;*.BMP;*.TGA;*.PSD" };

			if (filePicker.ShowDialog() == true)
			{
				_textureToEncode = filePicker.FileName;
				TextureToEncodeBox.Text = _textureToEncode;
			}
		}

		private void Converter_ConvertModel_Click(object sender, RoutedEventArgs e)
		{
			if (_modelToEncode == "")
			{
				MessageBox.Show("Please select a model to encode!");
				return;
			}

			if (_textureToEncode == "")
			{
				MessageBox.Show("Please select a texture to encode!");
				return;
			}

			AssimpContext context = new AssimpContext();
			Scene scene = context.ImportFile(_modelToEncode,
				PostProcessSteps.Triangulate | PostProcessSteps.MakeLeftHanded | PostProcessSteps.FlipWindingOrder |
				PostProcessSteps.JoinIdenticalVertices);

			if (!scene.HasMeshes)
				throw new Exception("This model has no meshes!");

			Mesh mesh = scene.Meshes[0];

			if (!mesh.HasVertices)
			{
				MessageBox.Show("This model doesn't have any vertices!");
				return;
			}

			if (!mesh.HasFaces)
			{
				MessageBox.Show("This model doesn't have any faces!");
				return;
			}

			if (!mesh.HasNormals)
			{
				MessageBox.Show("This model doesn't have any normals!");
				return;
			}

			if (!mesh.HasTextureCoords(0))
			{
				MessageBox.Show("This model doesn't have any UV coordinates!");
				return;
			}

			// Generate the model to load.
			Model model = new Model();

			foreach (Vector3D vec in mesh.Vertices)
			{
				model.Vertices.Add(new Vec3(vec.X, vec.Y, vec.Z));
			}

			foreach (Face face in mesh.Faces)
			{
				model.Indices.AddRange(face.Indices);
			}

			foreach (Vector3D normal in mesh.Normals)
			{
				model.Normals.Add(new Vec3(normal.X, normal.Y, normal.Z));
			}

			foreach (Vector3D uvw in mesh.TextureCoordinateChannels[0])
			{
				model.UVs.Add(new Vec2(uvw.X, uvw.Y));
			}

			model.Texture = _textureToEncode;

			string fileToWrite = _modelToEncode.Substring(0, _modelToEncode.LastIndexOf('.')) + ".gcm";
			Reader.WriteFile(fileToWrite, model);
		}
	}
}
