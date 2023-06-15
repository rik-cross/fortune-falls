using System.Numerics;
using System;

namespace AdventureGame.Engine
{
    public class Flags
    {
        public int Count { get; private set; }
        public BigInteger BitFlags { get; private set; }

        public Flags(int size = 0, int bitToSet = 0)
        {
            if (size > 0)
                Count = size;
            else
                Count = 0;

            if (bitToSet > 0)
                BitFlags = BigInteger.Pow(2, bitToSet - 1);
            else
                BitFlags = 0;
        }

        // Add a new flag value by increasing the number of available bits by one.
        public int NewFlag()
        {
            Count++;

            if (BitFlags == 0)
                BitFlags = 1;
            else
                BitFlags *= 2;

            //Console.WriteLine($"Add flag: count {Count}, bitFlag {BitFlags}");
            return Count;
        }

        // Perform a bitwise OR to set flags using the compare flags
        public void SetFlags(Flags compareFlags)
        {
            // Perform a bitwise OR on the compare flags
            BitFlags |= compareFlags.BitFlags;
        }

        // Perform a bitwise AND to remove flags using the compare flags
        public void RemoveFlags(Flags compareFlags)
        {
            // Perform a bitwise AND on the negated compare flags
            BitFlags &= ~compareFlags.BitFlags;
        }

        // Check if this flag has ALL of the compare flags
        public bool HasFlags(Flags compareFlags)
        {
            return (BitFlags & compareFlags.BitFlags) == compareFlags.BitFlags;
        }

        // Check if this flag has AT LEAST ONE of the compare flags
        public bool HasAtLeastOneFlag(Flags compareFlags)
        {
            return (BitFlags & compareFlags.BitFlags) > 0;
        }

        // Clears all flags
        public void Clear()
        {
            Count = 0;
            BitFlags = 0;
        }
    }
}