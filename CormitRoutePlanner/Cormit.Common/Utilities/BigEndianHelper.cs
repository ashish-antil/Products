using System;

namespace Imarda.Lib
{
	public static class BigEndian
	{
		public static int ReadInt16(byte[] bytes)
		{
			return (bytes[0] << 8) | bytes[0 + 1];
		}

		public static int ReadInt32(byte[] bytes)
		{
			return (bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3];
		}

        public static int ReadInt(byte[] bytes)
		{
			switch (bytes.Length)
			{
				case 1:
					return bytes[0];

				case 2:
					return ReadInt16(bytes);

				case 4:
					return ReadInt32(bytes);
			}
			return 0;
		}

        public static long ReadLong(byte[] bytes)
        {
            switch (bytes.Length)
            {
                case 8:
                    return 
                        (bytes[0] << 56) | 
                        (bytes[1] << 48) | 
                        (bytes[2] << 40) | 
                        (bytes[3] << 32) | (bytes[4] << 24) | (bytes[5] << 16) | (bytes[6] << 8) | bytes[7];
            }
            return 0;
        }
        
        public static byte[] WriteInt16(short number)
		{
			var bytes = new byte[2];
			bytes[0] = (byte)(number >> 8);
			bytes[1] = (byte)(number & 255);
			return bytes;
		}

        public static byte[] WriteInt32(int number)
		{
			var bytes = new byte[4];
			bytes[0] = (byte)(number >> 24);
			bytes[1] = (byte)((number >> 16) & 255);
			bytes[2] = (byte)((number >> 8) & 255);
			bytes[3] = (byte)(number & 255);
			return bytes;
		}

	}

}
