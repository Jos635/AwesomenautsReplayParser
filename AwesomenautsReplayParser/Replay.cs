using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AwesomenautsReplayParser.Model;

namespace AwesomenautsReplayParser
{
	public class Replay
	{
		private DirectoryInfo directory;
		private List<Lazy<ReplayBlock>> blocks;
        private List<Lazy<ReplayContinuous>> continuous;

		public Replay(DirectoryInfo directory)
		{
			this.directory = directory;

            continuous = directory
                .GetFiles("*.c")
                .Select(file => new Lazy<ReplayContinuous>(() => new ReplayContinuous(file)))
                .ToList();

            blocks = directory
				.GetFiles("*.b")
				.Select(file => new Lazy<ReplayBlock>(() => new ReplayBlock(file)))
				.ToList();
		}

        public void Preload()
        {
            foreach(var item in blocks)
            {
                var x = item.Value;
            }
        }

        public ReplayBlock GetBlockData(AbsoluteTime time)
        {
            return blocks.FirstOrDefault(item => item.Value.BaseRange.Contains(time)).Value;
        }
	}
}
