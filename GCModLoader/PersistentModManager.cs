using System.Collections;
using System.Collections.Generic;
using System.IO;
using GCAssetReader;
using UnityEngine;

namespace GCModLoader
{
	public class PersistentModManager : MonoBehaviour
	{
		private GameObject _obj = null;

		private string _testModelPath = @"E:\SSDLibrary\steamapps\common\Good Company\GCModLoader\Assets\TestMachine.gcm";

		void Awake()
		{
			_obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			_obj.transform.position = new Vector3(0, 0, 0);

			LoadModel();
		}

		IEnumerator LoadTexture(string path)
		{
			WWW www = new WWW("file://" + path);
			yield return www;
			Texture2D tex = new Texture2D(1, 1);
			www.LoadImageIntoTexture(tex);
			tex.filterMode = FilterMode.Point;
			_obj.GetComponent<MeshRenderer>().material.mainTexture = tex;
		}


		void LoadModel()
		{
			GCAssetReader.Model testModel = GCAssetReader.Reader.ReadFile(_testModelPath);

			ModLogger.LogInfo($"Loaded test model with {testModel.Vertices.Count} vertices and {testModel.Indices.Count} indices.");

			List<Vector3> vertices = new List<Vector3>(testModel.Vertices.Capacity);
			foreach (Vec3 v in testModel.Vertices)
			{
				vertices.Add(new Vector3(v.X, v.Y, v.Z));
			}

			List<Vector3> normals = new List<Vector3>(testModel.Normals.Capacity);
			foreach (Vec3 normal in testModel.Normals)
			{
				normals.Add(new Vector3(normal.X, normal.Y, normal.Z));
			}

			List<Vector2> uvs = new List<Vector2>(testModel.UVs.Capacity);
			foreach (Vec2 uv in testModel.UVs)
			{
				uvs.Add(new Vector2(uv.X, uv.Y));
			}

			StartCoroutine(LoadTexture(testModel.Texture));

			MeshFilter mf = _obj.GetComponent<MeshFilter>();
			Mesh mesh = new Mesh();
			mesh.vertices = vertices.ToArray();
			mesh.normals = normals.ToArray();
			mesh.SetIndices(testModel.Indices.ToArray(), MeshTopology.Triangles, 0);
			mesh.uv = uvs.ToArray();
			mf.sharedMesh = mesh;
		}

		private Rect _clientRect = new Rect(100, 100, 250, 300);

		private string _posX = "-39.0";
		private string _posY = "0.0";
		private string _posZ = "-3.0";

		private string _rotX = "90.0";
		private string _rotY = "0.0";
		private string _rotZ = "0.0";

		void OnGUI()
		{
			_clientRect = GUI.Window(1, _clientRect, (int id) =>
			{
				GUILayout.Label("Position");
				using (var positionScope = new GUILayout.HorizontalScope("box"))
				{
					_posX = GUILayout.TextField(_posX);
					_posY = GUILayout.TextField(_posY);
					_posZ = GUILayout.TextField(_posZ);
				}

				GUILayout.Label("Rotation");
				using (var rotationScope = new GUILayout.HorizontalScope("box"))
				{
					_rotX = GUILayout.TextField(_rotX);
					_rotY = GUILayout.TextField(_rotY);
					_rotZ = GUILayout.TextField(_rotZ);
				}

				Vector3 pos = new Vector3();
				Vector3 rot = new Vector3();

				TryParse(ref pos, _posX, _posY, _posZ);
				TryParse(ref rot, _rotX, _rotY, _rotZ);

				_obj.transform.position = pos;
				_obj.transform.rotation = Quaternion.Euler(rot);

				if (GUILayout.Button("Reload model"))
				{
					LoadModel();
				}

				GUILayout.Label("Loaded texture:");
				GUI.DrawTexture(new Rect(10, 200, 100, 100), _obj.GetComponent<MeshRenderer>().material.mainTexture);

				GUI.DragWindow();
			}, "Test Model Mover");
		}

		void TryParse(ref Vector3 vec, string x, string y, string z)
		{
			Vector3 temp = vec;
			if (float.TryParse(x, out temp.x) &&
			    float.TryParse(y, out temp.y) &&
			    float.TryParse(z, out temp.z))
			{
				vec = temp;
			}
		}
	}
}