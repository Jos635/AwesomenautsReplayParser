using System;
using System.IO;

namespace AwesomenautsReplayParser
{
    internal class ReplayContinuous : ReplayBase
    {
        private FileInfo file;

        public ReplayContinuous(FileInfo file)
        {
            this.file = file;

            BitStream b = new BitStream(File.ReadAllBytes(file.FullName));

            var versionNumber = b.ReadUInt(16);
            var blockCount = b.ReadUInt(16);

            Console.WriteLine($"Version {versionNumber:X}, block count: {blockCount}");

            for (var i = 0; i < blockCount; i++)
            {
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine($"=== Block {i} / {blockCount} ===");
                ReadHeader(b);

                ReadUIntVotList(b, 6, 13); // Current solar?
                ReadUIntVotList(b, 6, 13); // Total solar?
                ReadUIntVotList(b, 5, 13);
                ReadUIntVotList(b, 3, 7);
                ReadUIntVotList(b, 2, 7);
                ReadUIntVotList(b, 6, 10);
                ReadUIntVotList(b, 2, 3);
                ReadUIntVotList(b, 5, 12);

                Console.WriteLine(b.ReadUInt(2).ToString("X"));

                ReadUIntVotList(b, 2, 10);
                ReadUIntVotList(b, 1, 22);
                ReadUIntVotList(b, 1, 22);
                ReadUIntVotList(b, 1, 22);
                ReadUIntVotList(b, 1, 22);

                var additionalFloats = b.ReadUInt(6);
                for (int j = 0; j < additionalFloats; j++)
                {
                    Console.WriteLine(b.ReadFloat(12, 0.0, 10.0));
                }
            }

            Console.WriteLine($"EOF; Data remaining: {b.RemainingLength}");
        }
    }
}