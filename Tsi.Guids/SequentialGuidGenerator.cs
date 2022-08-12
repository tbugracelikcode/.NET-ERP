using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Tsi.Guids
{
    public class SequentialGuidGenerator : IGuidGenerator
    {
        private static readonly RandomNumberGenerator RandomNumberGenerator = RandomNumberGenerator.Create();

        public Guid CreateGuid()
        {
            var randomBytes = new byte[8];
            RandomNumberGenerator.GetBytes(randomBytes);

            long timestamp = DateTime.UtcNow.Ticks;

            long milliseconds = timestamp / 10000L;
            long remainderTicks = timestamp % 10000L;

            //byte[] timestampBytes = BitConverter.GetBytes(timestamp);

            byte[] timestampBytes = new byte[8];
            byte[] millisecondsBytes = BitConverter.GetBytes(milliseconds);
            byte[] remainderTicksBytes = BitConverter.GetBytes(remainderTicks);

            // Merge two arrays
            Buffer.BlockCopy(millisecondsBytes, 0, timestampBytes, 2, 6);
            Buffer.BlockCopy(remainderTicksBytes, 0, timestampBytes, 0, 2);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }

            byte[] guidBytes = new byte[16];

            Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 8);
            Buffer.BlockCopy(timestampBytes, 6, guidBytes, 8, 2);
            Buffer.BlockCopy(timestampBytes, 0, guidBytes, 10, 6);

            return new Guid(guidBytes);
        }
    }
}
