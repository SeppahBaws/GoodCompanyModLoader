using System;
using Assimp;
using GCAssetReader;

namespace GCAssetReaderTests
{
	class Program
	{
		private static string _basePath = @"D:\GoodCompany\GoodCompanyModLoader\GCAssetReaderTests\TestAssets\";
		static void Main(string[] args)
		{
			AssimpContext context = new AssimpContext();
			Scene scene = context.ImportFile(_basePath + "cube.fbx", PostProcessSteps.Triangulate | PostProcessSteps.MakeLeftHanded);

			if (!scene.HasMeshes)
				throw new Exception("This model has no meshes!");

			// for (int i = 0; i < model.MeshCount; i++)
			// {
			// }

			Mesh mesh = scene.Meshes[0];

			if (!mesh.HasVertices)
				throw new Exception("One of the meshes doesn't have any vertices!");

			if (!mesh.HasFaces)
				throw new Exception("One of the meshes doesn't have any faces!");

			if (!mesh.HasNormals)
				throw new Exception("One of the meshes doesn't have any normals!");

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

			Reader.WriteFile(_basePath + "cube.gcm", model);


			// Test reading as well.
			Model readModel = Reader.ReadFile(_basePath + "cube.gcm");
		}
	}
}
