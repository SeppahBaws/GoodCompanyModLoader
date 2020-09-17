using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodCompanyModLoader
{
	class ModConfig
	{
		public string GamePath;
		public Profile[] Profiles;
	}

	class Profile
	{
		public string Name;
		public Mod[] Mods;
	}

	class Mod
	{
		public string Name;
		public string Path;
		public bool Enabled;
	}
}
