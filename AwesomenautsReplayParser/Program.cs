﻿using System;
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
            Console.Write("Enter replay name: ");
			var r = new Replay(new DirectoryInfo(Console.ReadLine()));
            r.Preload();
			Console.ReadLine();
		}
	}
}
