using System;
using System.Diagnostics;
using System.IO;

namespace GCModLoader
{
	public static class ModLogger
	{
		private static StreamWriter _streamWriter;
		public static void Init(string fileName)
		{
			FileStream fs = new FileStream(fileName, FileMode.Create);
			_streamWriter = new StreamWriter(fs)
			{
				AutoFlush = true
			};
		}

		private static void WriteMsg(string msg)
		{
			Console.WriteLine(msg);
			_streamWriter.WriteLine(msg);
		}

		public static void Log(string msg)
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Black;
			WriteMsg($"[ModLoader] | MESSAGE   | {msg}");
			ResetColors();
		}

		public static void LogInfo(string msg)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.BackgroundColor = ConsoleColor.Black;
			WriteMsg($"[ModLoader] | INFO      | {msg}");
			ResetColors();
		}

		public static void LogWarning(string msg)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.BackgroundColor = ConsoleColor.Black;
			WriteMsg($"[ModLoader] | WARNING   | {msg}");
			ResetColors();
		}

		public static void LogError(string msg)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.BackgroundColor = ConsoleColor.Black;
			WriteMsg($"[ModLoader] | ERROR     | {msg}");
			ResetColors();
		}

		public static void LogException(string msg)
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.DarkRed;
			WriteMsg($"[ModLoader] | EXCEPTION | {msg}");
			ResetColors();
		}

		private static void ResetColors()
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Black;
		}
	}
}
