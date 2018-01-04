using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AwesomenautsReplayParser
{
	public class BitStream
	{
		private bool[] data;
		private int position = 0;
		public int Length 
		{ 
			get 
			{ 
				return data.Length; 
			} 
		}

        public int RemainingLength => Length - position;

        public BitStream(byte[] bytes)
		{
			BitArray barray = new BitArray(bytes);
			data = new bool[barray.Length];
			barray.CopyTo(data, 0);
        }

        internal int ReadLong(int length)
        {
            if (length <= 1) throw new ArgumentOutOfRangeException("length");
            bool sign = ReadBool();
            ulong value = ReadULong(length - 1);

            return sign ? -(int)value : (int)value;
        }

        public ulong ReadULong(int length)
        {
            if (length > 64) throw new ArgumentOutOfRangeException("length");

            ulong value = 0;
            for (int i = 0; i < length; i++)
            {
                value |= (ulong)ReadBit() << i;
            }

            return value;
        }

        public BitStream(bool[] data)
		{
			this.data = data;
		}

        internal int ReadInt(int length)
        {
            if (length <= 1) throw new ArgumentOutOfRangeException("length");
            bool sign = ReadBool();
            uint value = ReadUInt(length - 1);

            return sign ? -(int)value : (int)value;
        }

        internal double ReadFloat(int length, double min, double max)
        {
            uint val = ReadUInt(length);
            uint maxVal = ((uint)1 << length) - 1;

            var difference = (max - min);

            return min + difference * (val / (double)maxVal);
        }

        public uint ReadUInt(int length)
		{
			if (length > 32) throw new ArgumentOutOfRangeException("length");

			uint value = 0;
			for(int i = 0; i < length; i++)
			{
				value |= ReadBit() << i;
			}

			return value;
		}

        public bool ReadBool()
        {
            return ReadBit() == 1;
        }

		public uint ReadBit()
		{
			return data[position++] ? (uint)1 : 0;
		}

		public BitStream ReadStream(int length)
		{
			bool[] newData = new bool[length];
			Buffer.BlockCopy(data, position, newData, 0, newData.Length);

			position += length;
			return new BitStream(newData);
		}
        
		public string ReadString()
		{
            var bytes = new List<byte>();
			byte c;
			while ((c = (byte)ReadUInt(8)) != 0)
			{
                bytes.Add(c);
			}

			return UTF8Encoding.UTF8.GetString(bytes.ToArray());
		}

		public string ReadBlock(int length)
		{
			string s = "";
			for (int i = 0; i < length; i++)
			{
				s += ReadBit().ToString();
			}

			return s;
		}
	}
}