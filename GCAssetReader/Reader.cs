using System;
using System.Collections.Generic;
using System.IO;

namespace GCAssetReader
{
	public class Model
	{
		public List<Vec3> Vertices;
		public List<int> Indices;
		public List<Vec3> Normals;
		public List<Vec2> UVs;
		public string Texture;

		public Model()
		{
			Vertices = new List<Vec3>();
			Indices = new List<int>();
			Normals = new List<Vec3>();
			UVs = new List<Vec2>();
			Texture = "";
		}
	}

	public class Vec3
	{
		public float X, Y, Z;

		public Vec3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}
	}

	public class Vec2
	{
		public float X, Y;

		public Vec2(float x, float y)
		{
			X = x;
			Y = y;
		}
	}

	enum BlockType
	{
		Vertices = 0,
		Indices = 1,
		Normals = 2,
		UVs = 3,
		Texture = 4,
	}

	public static class Reader
	{
		public static Model ReadFile(string filePath)
		{
			try
			{
				BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open));
				return Serializer.ReadModel(reader);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Exception triggered while reading model file: {e}");
				throw;
			}
		}

		public static void WriteFile(string filePath, Model model)
		{
			try
			{
				using (BinaryWriter binWriter = new BinaryWriter(File.Open(filePath, FileMode.Create)))
				{
					model.Write(binWriter);
				}
			}
			catch (IOException e)
			{
				Console.WriteLine($"Exception triggered while writing model file: {e}");
				throw;
			}
		}
	}
}
