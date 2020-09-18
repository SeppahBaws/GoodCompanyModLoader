namespace GoodCompanyModLoader
{
	class ModConfig
	{
		public string GamePath;
		public Mod[] Mods;
	}

	class Mod
	{
		public string Name;
		public string Path;
		public bool Enabled;
	}
}
