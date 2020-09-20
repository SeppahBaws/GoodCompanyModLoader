using System.Collections.Generic;
using System.IO;

namespace GCAssetReader
{
	public static class Serializer
	{
		internal static void Write(this Model model, BinaryWriter writer)
		{
			// Vertices block type
			writer.Write((char)BlockType.Vertices);
			// Vertices
			model.Vertices.Write(writer);

			// Indices block type
			writer.Write((char)BlockType.Indices);
			// Indices
			model.Indices.Write(writer);

			// Normals block type
			writer.Write((char)BlockType.Normals);
			// Normals
			model.Normals.Write(writer);

			// UVs block type
			writer.Write((char)BlockType.UVs);
			// UVs
			model.UVs.Write(writer);

			// Texture block type
			writer.Write((char)BlockType.Texture);
			// Texture
			writer.Write(model.Texture);
		}

		static void Write(this Vec2 vec, BinaryWriter writer)
		{
			writer.Write(vec.X);
			writer.Write(vec.Y);
		}

		static void Write(this Vec3 vec, BinaryWriter writer)
		{
			writer.Write(vec.X);
			writer.Write(vec.Y);
			writer.Write(vec.Z);
		}

		static void Write(this List<Vec2> list, BinaryWriter writer)
		{
			writer.Write(list.Count);
			foreach (Vec2 vec in list)
			{
				vec.Write(writer);
			}
		}

		static void Write(this List<Vec3> list, BinaryWriter writer)
		{
			writer.Write(list.Count);
			foreach (Vec3 vec in list)
			{
				vec.Write(writer);
			}
		}

		static void Write(this List<int> list, BinaryWriter writer)
		{
			writer.Write(list.Count);
			foreach (int i in list)
			{
				writer.Write(i);
			}
		}

		/////////////////////////////////////////////////////////////////////////////

		internal static Model ReadModel(BinaryReader reader)
		{
			Model model = new Model();

			while (reader.BaseStream.Position != reader.BaseStream.Length)
			{
				switch ((BlockType)reader.ReadChar())
				{
					case BlockType.Vertices:
						model.Vertices = ReadVec3List(reader);
						break;

					case BlockType.Indices:
						model.Indices = ReadIntList(reader);
						break;

					case BlockType.Normals:
						model.Normals = ReadVec3List(reader);
						break;

					case BlockType.UVs:
						model.UVs = ReadVec2List(reader);
						break;

					case BlockType.Texture:
						model.Texture = reader.ReadString();
						break;
				}
			}

			return model;
		}

		static List<Vec2> ReadVec2List(BinaryReader reader)
		{
			int count = reader.ReadInt32();

			List<Vec2> list = new List<Vec2>(count);
			for (int i = 0; i < count; i++)
			{
				list.Add(new Vec2(
					reader.ReadSingle(),
					reader.ReadSingle()
				));
			}

			return list;
		}

		static List<Vec3> ReadVec3List(BinaryReader reader)
		{
			int count = reader.ReadInt32();

			List<Vec3> list = new List<Vec3>(count);
			for (int i = 0; i < count; i++)
			{
				list.Add(new Vec3(
					reader.ReadSingle(),
					reader.ReadSingle(),
					reader.ReadSingle()
				));
			}

			return list;
		}

		static List<int> ReadIntList(BinaryReader reader)
		{
			int count = reader.ReadInt32();

			List<int> list = new List<int>(count);
			for (int i = 0; i < count; i++)
			{
				list.Add(reader.ReadInt32());
			}

			return list;
		}
	}
}