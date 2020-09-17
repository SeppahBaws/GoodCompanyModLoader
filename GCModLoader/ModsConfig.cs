using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCModLoader
{
	class ModsConfig
	{
		public Mod[] Mods;
	}

	class Mod
	{
		public string Name;
		public string File;
		public bool Enabled;
	}
}
