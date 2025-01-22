using System.Numerics;
using System;

namespace Engine
{
    public class Flags
    {
        public BigInteger BitFlags { get; private set; }

        public Flags(int bitToSet = 0)
        {
            if (bitToSet > 0)
                BitFlags = BigInteger.Pow(2, bitToSet - 1);
            else
                BitFlags = 0;
        }

        // Add a new flag value by increasing the number of available bits by one.
        public void NewFlag()
        {
            if (BitFlags == 0)
                BitFlags = 1;
            else
                BitFlags *= 2;
        }

        // Perform a bitwise OR to set flags using the compare flags
        public void SetFlags(Flags compareFlags)
        {
            BitFlags |= compareFlags.BitFlags;
        }

        // Perform a bitwise AND to remove flags using the negated compare flags
        public void RemoveFlags(Flags compareFlags)
        {
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
        public void Clear() { BitFlags = 0; }
    }
}