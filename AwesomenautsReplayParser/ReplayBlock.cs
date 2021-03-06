﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using AwesomenautsReplayParser.Model;

namespace AwesomenautsReplayParser
{
	public class ReplayBlock : ReplayBase
	{
		private FileInfo file;

        public uint VersionNumber { get; }

        public List<CharacterEntity> Characters { get; } = new List<CharacterEntity>();
        public List<TurretEntity> Turrets { get; } = new List<TurretEntity>();

        public ReplayBlock(FileInfo file)
		{
			this.file = file;

			BitStream b = new BitStream(File.ReadAllBytes(file.FullName));

            VersionNumber = b.ReadUInt(16);
            var blockCount = b.ReadLong(30);

            var startTime = b.ReadFloat(24, 0, 18000);
            var endTime = b.ReadFloat(24, 0, 18000);

            BaseRange = new AbsoluteTimeRange(startTime, endTime);

            Console.WriteLine($"Version {VersionNumber:X}, block count: {blockCount}");
            Console.WriteLine($"Time range: {startTime:00.00}s - {endTime:00.00}s");

            var timePointNearStartTime = b.ReadFloat(20, 0, 5400);
            Console.WriteLine(timePointNearStartTime);

            Console.WriteLine(b.ReadBool());
            Console.WriteLine(b.ReadUInt(2));

            ReadFilePart0(b);
            ReadCharacters(b);
            ReadTurrets(b);
            ReadBases(b);
            ReadTiledAnimations(b);
            ReadLasers(b);
            ReadFilePart6(b);
            ReadFilePart7(b);
            ReadFilePart8(b);
            ReadFilePart9(b);
            ReadFilePart10(b);
            ReadFilePart11(b);
            ReadFilePart12(b);
            ReadFilePart13(b);
            ReadElectrocuteStrikes(b);
        }

        private void ReadElectrocuteStrikes(BitStream b)
        {
            var count = b.ReadUInt(10);

            for (var i = 0; i < count; i++)
            {
                ReadTimeRange(b);

                var u1 = b.ReadUInt(2);
                var u2 = b.ReadUInt(14);
                var u3 = b.ReadUInt(14);

                ReadPositionVotList(b, 5, 13);
                ReadPositionVotList(b, 5, 13);

                var f3 = b.ReadFloat(6, 0, 1);
            }
        }

        private void ReadFilePart13(BitStream b)
        {
            var count = b.ReadUInt(5);

            for (var i = 0; i < count; i++)
            {
                var timePoint = ReadTimePoint(b);
                var u1 = b.ReadUInt(6);

                var u2 = b.ReadUInt(3);
                var b1 = b.ReadBool();

                ReadHeader(b);
                ReadHeader(b);

                var b2 = b.ReadBool();
                var b3 = b.ReadBool();
                var b4 = b.ReadBool();

                var u3 = b.ReadUInt(2);
                var u4 = b.ReadUInt(14);
            }
        }

        private void ReadFilePart12(BitStream b)
        {
            var count = b.ReadUInt(4);

            for (var i = 0; i < count; i++)
            {
                ReadTimeRange(b);

                ReadPositionVotList(b, 5, 8);

                var str = b.ReadString();
            }
        }

        private void ReadFilePart11(BitStream b)
        {
            var count = b.ReadUInt(8);

            for (var i = 0; i < count; i++)
            {
                ReadTimeRange(b);

                var f3 = b.ReadFloat(7, 0, 10);

                var u1 = b.ReadUInt(12);
                if (VersionNumber <= 0x1f) // TODO: Maybe 0x1e?
                {
                    var u2 = b.ReadUInt(13);
                }
                else
                {
                    var u2 = b.ReadUInt(14);
                }

                ReadPositionVotList(b, 5, 8);
                ReadFloatVotList(b, 5, 6, 0, 1);

                var b1 = b.ReadBool();
            }
        }

        private void ReadFilePart10(BitStream b)
        {
            var count = b.ReadUInt(4);

            for (var i = 0; i < count; i++)
            {
                // "hidingmid". Invisible areas?

                var str = b.ReadString();
                ReadVoidVotList(b, 5);
            }
        }

        private void ReadFilePart9(BitStream b)
        {
            var count = b.ReadUInt(8);

            for (var i = 0; i < count; i++)
            {
                var str = b.ReadString();
                ReadFloatVotList(b, 7, 4, 0, 0.5);
            }
        }

        private void ReadFilePart8(BitStream b)
        {
            var count = b.ReadUInt(4);

            for (var i = 0; i < count; i++)
            {
                ReadTimeRange(b);

                var u1 = b.ReadUInt(14);
                var u2 = b.ReadUInt(2);

                var f3 = b.ReadFloat(5, 0, 2);
                var f4 = b.ReadFloat(5, 0, 2);
                var f5 = b.ReadFloat(4, -100, 10);

                ReadPositionVotList(b, 4, 8);
                ReadFloatVotList(b, 4, 6, 0, 360);

                var b1 = b.ReadBool();
            }
        }

        private void ReadFilePart7(BitStream b)
        {
            var count = b.ReadUInt(10);

            for (var i = 0; i < count; i++)
            {
                ReadTimeRange(b);

                var u1 = b.ReadUInt(14);
                var b1 = b.ReadBool();
                if(b1)
                {
                    var f3 = b.ReadFloat(10, 60, 70);
                }

                var f4 = b.ReadFloat(10, -15, 31);

                ReadPositionVotList(b, 7, 13);
                ReadFloatVotList(b, 8, 6, 0, 360);
                ReadVoidVotList(b, 2);

                var u2 = b.ReadUInt(3);

                var f5 = b.ReadFloat(10, 0, 2);
                var f6 = b.ReadFloat(10, 0, 2);
                var f7 = b.ReadFloat(12, 0, 40);
                var f8 = b.ReadFloat(12, 0, 40);
                var f9 = b.ReadFloat(12, 0, 40);
                var f10 = b.ReadFloat(12, 0, 40);

                var u3 = b.ReadUInt(2);
            }
        }

        private void ReadFilePart6(BitStream b)
        {
            var count = b.ReadUInt(10);

            for (var i = 0; i < count; i++)
            {
                ReadTimeRange(b);

                var u1 = b.ReadUInt(14);
                var f3 = b.ReadFloat(7, 0, 1);
                var b1 = b.ReadBool();

                var u2 = b.ReadUInt(2);
                var b2 = b.ReadBool();
                if(b2)
                {
                    var f4 = b.ReadFloat(10, 0, 1);
                }

                var f5 = b.ReadFloat(16, -15, 31);
                var b3 = b.ReadBool();

                ReadPositionVotList(b, 7, 13);
                ReadFloatVotList(b, 8, 6, 0, 360);
                ReadVoidVotList(b, 2);

                ReadUIntVotList(b, 3, 2);
            }
        }

        private void ReadLasers(BitStream b)
        {
            var count = b.ReadUInt(6);

            for (var i = 0; i < count; i++)
            {
                var u1 = b.ReadUInt(7);
                ReadUIntVotList(b, 7, 3);

                var u2 = b.ReadUInt(2);
                ReadUIntVotList(b, 5, 14);
                ReadFloatVotList(b, 5, 5, 0, 1);

                var f1 = b.ReadFloat(11, -0.11, 0.22);
                var f2 = b.ReadFloat(4, 0, 2);
                var f3 = b.ReadFloat(4, 0, 1);

                ReadUIntVotList(b, 5, 14);
                ReadUIntVotList(b, 5, 14);
            }
        }

        private void ReadTiledAnimations(BitStream b)
        {
            var count = b.ReadUInt(5);

            for (var i = 0; i < count; i++)
            {
                ReadTimeRange(b);

                var u1 = b.ReadUInt(2);
                var f3 = b.ReadFloat(8, -0.1, 0.201);
                var u2 = b.ReadUInt(14);
                var u3 = b.ReadUInt(14);
                var u4 = b.ReadUInt(14);

                ReadPositionVotList(b, 5, 13);
                ReadPositionVotList(b, 5, 13);
            }
        }

        private void ReadBases(BitStream b)
        {
            var count = b.ReadUInt(3);

            for (var i = 0; i < count; i++)
            {
                ReadHeader(b);

                var u1 = b.ReadUInt(2);
                var f1 = b.ReadFloat(8, 0, 30000);

                ReadFloatVotList(b, 8, 7, 0, 30000);
                ReadFloatVotList(b, 8, 4, 0, 1);

                var f2 = b.ReadFloat(13, -10, 20); // Possibly the position?
                var f3 = b.ReadFloat(13, -10, 30);
                var f4 = b.ReadFloat(8, 0, 1.5);

                ReadUIntVotList(b, 3, 14);
            }
        }

        private void ReadTurrets(BitStream b)
        {
            var count = b.ReadUInt(6);

            for(var i = 0; i < count; i++)
            {
                ReadTimeRange(b);
                ReadHeader(b);

                var teamId = b.ReadUInt(2);
                var f3 = b.ReadFloat(8, 0, 30000);

                ReadFloatVotList(b, 8, 7, 0, 30000);
                ReadFloatVotList(b, 8, 4, 0, 1);

                var position = ReadPosition(b, 13);
                var f6 = b.ReadFloat(8, 0, 1.5);

                ReadUIntVotList(b, 3, 14);
                var aim = ReadFloatVotList(b, 8, 7, 0, 360);

                ReadVoidVotList(b, 4);

                var f7 = b.ReadFloat(6, 0, 1.5);
                ReadUIntVotList(b, 5, 14);

                Turrets.Add(new TurretEntity
                {
                    Position = position,
                    Aim = aim,
                    TeamId = teamId,
                });
            }
        }

        #region Part 1
        private void ReadCharacters(BitStream b)
        {
            var numItems = b.ReadUInt(16);
            for(var i = 0; i < numItems; i++)
            {
                var timeRange = ReadTimeRange(b);

                var l1 = b.ReadLong(30);
                var b1 = b.ReadBool();

                if (b1)
                {
                    var ul1 = b.ReadULong(64);
                }

                ReadHeader(b);

                ReadFloatVotList(b, 4, 10, 0.0, 120); // ?

                var numSubItems = b.ReadUInt(4);
                for (var j = 0; j < numSubItems; j++)
                {
                    var visibleTime = ReadTimeRange(b);

                    /*
                     * Bools to find: isAlive (1), solar (13/14), itemsBought (6), damageDone (10)
                     */

                    var teamId = b.ReadUInt(2);
                    var b2 = b.ReadBool();
                    var b3 = b.ReadBool();
                    var b4 = b.ReadBool();
                    var b5 = b.ReadBool();
                    var b6 = b.ReadBool();

                    var f5 = b.ReadFloat(5, -0.008, 0.02);
                    var f6 = b.ReadFloat(7, 0.0, 0.5);
                    var f7 = b.ReadFloat(6, 0.0, 1.5);
                    var f8 = b.ReadFloat(8, -0.1, 0.21);

                    var userName = b.ReadString();
                    var characterName = b.ReadString();

                    var b7 = b.ReadBool();
                    
                    var b8 = b.ReadBool();
                    if(b8) // correct until here
                    { 
                        ReadUIntVotList(b, 5, 14); // Possibly reading floats between -1 and 10000000000
                        ReadUIntVotList(b, 5, 14);
                        ReadUIntVotList(b, 5, 14);
                        ReadUIntVotList(b, 5, 14);

                        // Health is probably stored in 7 bits.

                        ReadFloatVotList(b, 5, 8, 0, 20);
                        ReadFloatVotList(b, 5, 8, 0, 20);
                        ReadFloatVotList(b, 5, 8, 0, 20);
                        ReadFloatVotList(b, 5, 7, 0, 1);
                        ReadFloatVotList(b, 5, 7, 0, 1);
                        ReadFloatVotList(b, 5, 7, 0, 1);

                        ReadIntVotList(b, 5, 5);
                        ReadIntVotList(b, 5, 5);
                        ReadIntVotList(b, 5, 5);
                    }

                    var skinId = b.ReadUInt(3);
                    var u3 = b.ReadUInt(3);

                    ReadHeader(b, 5);

                    var positions = ReadPositionVotList(b, 7, 13);
                    var aim = ReadFloatVotList(b, 8, 7, 0.0, 360.0);

                    ReadVoidVotList(b, 6);
                    ReadVoidVotList(b, 2);
                    ReadVoidVotList(b, 2);
                    ReadVoidVotList(b, 4);
                    ReadVoidVotList(b, 4);
                    ReadVoidVotList(b, 4);
                    ReadVoidVotList(b, 5);
                    ReadVoidVotList(b, 4);
                    ReadVoidVotList(b, 5);
                    ReadVoidVotList(b, 1);

                    ReadUIntVotList(b, 3, 13);
                    ReadUIntVotList(b, 7, 13);
                    ReadUIntVotList(b, 6, 5);

                    ReadFilePart1_1(b);
                    ReadFilePart1_2(b);
                    ReadFilePart1_3(b);
                    ReadFilePart1_4(b);
                    ReadBowStrings(b);
                    ReadStatusEffectIcons(b);

                    Characters.Add(new CharacterEntity(userName, characterName)
                    {
                        Position = positions,
                        Aim = aim,
                        VisibleTime = visibleTime,
                        TeamId = teamId,
                    });
                }
            }
        }

        private void ReadStatusEffectIcons(BitStream b)
        {
            var count = b.ReadUInt(6);

            for (var i = 0; i < count; i++)
            {
                ReadTimeRange(b);
                var u1 = b.ReadUInt(14);

                var f3 = b.ReadFloat(7, 0.0, 1.0);
                var b1 = b.ReadBool();

                ReadUIntVotList(b, 7, 9);
            }
        }

        private void ReadBowStrings(BitStream b)
        {
            var count = b.ReadUInt(2);

            for (var i = 0; i < count; i++)
            {
                var u1 = b.ReadUInt(4);
                var f1 = ReadTimePoint(b);
                var f2 = ReadTimePoint(b);

                ReadUIntVotList(b, 2, 14);
                ReadUIntVotList(b, 2, 14);

                var f3 = b.ReadFloat(2, 0.01, 0.02);
                var f4 = b.ReadFloat(2, 0.005, 0.01);
            }
        }

        private void ReadFilePart1_4(BitStream b)
        {
            var count = b.ReadUInt(7);

            for (var i = 0; i < count; i++)
            {
                ReadTimeRange(b);

                var u1 = b.ReadUInt(14);
                var b1 = b.ReadBool();

                var f3 = b.ReadFloat(7, 0.0, 1.0);

                var b2 = b.ReadBool();
                if(b2)
                {
                    var f4 = b.ReadFloat(6, 0.5, 2.5);
                }

                ReadUIntVotList(b, 4, 7);

                var u2 = b.ReadUInt(2);
                var f5 = b.ReadFloat(11, -0.01, 0.01);
                var b3 = b.ReadBool();
                var b4 = b.ReadBool();
                if(!b4)
                {
                    var b5 = b.ReadBool();
                }

                ReadFloatTupleVotList(b, 9, 10, -0.5, 0.5, -0.5, 0.5);
                ReadFloatVotList(b, 8, 7, 0.0, 360.0);
            }
        }

        private void ReadFilePart1_3(BitStream b)
        {
            var count = b.ReadUInt(10);

            for (var i = 0; i < count; i++)
            {
                var timePoint = ReadTimePoint(b);
                var u1 = b.ReadUInt(12);
                var u2 = b.ReadUInt(12);
                var u3 = b.ReadUInt(3);

                ReadHeader(b, 5);
                ReadHeader(b, 5);
            }
        }

        private void ReadFilePart1_2(BitStream b)
        {
            var count = b.ReadUInt(7);

            for (var i = 0; i < count; i++)
            {
                var timePoint = ReadTimePoint(b);
                var u1 = b.ReadUInt(14);
                var b1 = b.ReadBool();
                var f1 = b.ReadFloat(7, 0.0, 1.0);

                var b2 = b.ReadBool();
                if (b2)
                {
                    var f2 = b.ReadFloat(7, 0.0, 2.5);
                }

                var b3 = b.ReadBool();
                if(b3)
                {
                    var u2 = b.ReadUInt(6);
                }
            }
        }

        private void ReadFilePart1_1(BitStream b)
        {
            var count = b.ReadUInt(3);

            // Gnaw's weedlings, voltar's healbots, leon's tongue?

            for(var i = 0; i < count; i++)
            {
                var n = ReadVoidVotList(b, 2);

                if (n)
                {
                    var u1 = b.ReadUInt(12);
                    var f1 = b.ReadFloat(5, -0.008, 0.02);
                    var f2 = b.ReadFloat(7, 0.0, 0.5);

                    ReadUIntVotList(b, 2, 5);
                    ReadUIntVotList(b, 5, 7);
                }
            }
        }
#endregion

        private void ReadFilePart0(BitStream b)
        {
            // According to https://joostdevblog.blogspot.nl/2014/01/bitcrunching-numbers-for-bandwidth.html
            // - Solar is 14 bits
            ReadUIntVotList(b, 2, 5); // These are maybe floats?
            ReadUIntVotList(b, 4, 14);
            ReadUIntVotList(b, 2, 14);
            ReadUIntVotList(b, 2, 5);
            ReadUIntVotList(b, 4, 14);
            ReadUIntVotList(b, 2, 14);
        }
    }
}
