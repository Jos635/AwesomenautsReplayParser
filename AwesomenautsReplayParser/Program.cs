using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AwesomenautsReplayParser
{
	class Program
	{
		static void Main(string[] args)
		{
			var r = new Replay(new DirectoryInfo("r2"));
            r.Preload();
			Console.ReadLine();
		}
	}
}
