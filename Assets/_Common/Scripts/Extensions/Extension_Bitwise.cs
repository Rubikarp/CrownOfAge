using UnityEngine;

public static class Extension_Bitwise
{
    public static int AsBitShift(this int value) => 1 << value;

    public static int SetBit(this int value, int bitPosition) =>  value | (1 << bitPosition);
    public static int ClearBit(this int value, int bitPosition) => value & ~(1 << bitPosition);
    public static int ToggleBit(this int value, int bitPosition) => value ^ (1 << bitPosition);

    public static bool HasBitSet(this int value, int bitPosition) => (value & (1 << bitPosition)) != 0;
    public static bool ContainBit(this int value, int bitPosition) => (value & (1 << bitPosition)) != 0;

    public static int CountSetBits(this int value)
    {
        int count = 0;
        while (value != 0)
        {
            count += value & 1;
            value >>= 1;
        }
        return count;
    }


    public static bool ContainsLayer(this LayerMask layerMask, int layer) => ((int)layerMask).HasBitSet(layer);
}