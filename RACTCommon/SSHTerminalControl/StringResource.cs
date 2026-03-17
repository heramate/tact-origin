using System;
using System.Globalization;
using System.Resources;
using System.Diagnostics;
using System.Reflection;

namespace Routrek.SSHC 
{
	/// <summary>
	/// StringResource 궻둜뾴궻먣뼻궳궥갃
	/// </summary>
	internal class StringResources {
		private string _resourceName;
		private ResourceManager _resMan;

		public StringResources(string name, Assembly asm) {
			_resourceName = name;
			LoadResourceManager(name, asm);
		}

		public string GetString(string id) 
		{
			try
			{
				return _resMan.GetString(id); //귖궢궞귢궕뭯궋귝궎궶귞궞궻긏깋긚궳긌긿긞긘깄궳귖궰궘귢궽궋궋궬귣궎
			}
			catch
			{
				return "error loading string";
			}
		}

		private void LoadResourceManager(string name, Assembly asm) {
			//뱰뽋궼뎟뚭갋볷?뚭궢궔궢궶궋
			CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentUICulture;
			//if(ci.Name.StartsWith("ja"))
				//_resMan = new ResourceManager(name+"_ja", asm);
			//else
			_resMan = new ResourceManager(name, asm);
		}
	}
}
