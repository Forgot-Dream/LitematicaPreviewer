using System;
using System.Collections;
using System.Collections.Generic;

namespace LitematicaPreviewer.LitematicaLib.Common
{
    

    public class LitematicaBitArray : IEnumerable<int>
    {
        private readonly int size;
        private readonly int _nbits;
        private readonly BitArray _bitArray;

        public LitematicaBitArray(int size, int nbits,BitArray array)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size));
            if (nbits <= 0 || nbits > 64)
                throw new ArgumentOutOfRangeException(nameof(nbits));

            this.size = size;
            _nbits = nbits;
            _bitArray = array;
        }

        public static LitematicaBitArray FromNbtLongArray(long[] arr, int size, int nbits)
        {
            int expectedLength = (size * nbits + 63) / 64;
            if (expectedLength != arr.Length)
                throw new ArgumentException($"Long array length does not match expected length. Expected: {expectedLength}, Actual: {arr.Length}");

            var bytes = new byte[arr.Length * sizeof(long)];
            Buffer.BlockCopy(arr, 0, bytes, 0, bytes.Length);

            // 使用字节数组创建BitArray
            var bitArray = new BitArray(bytes);

            return new LitematicaBitArray(size, nbits, bitArray);
        }

        public long[] ToLongArray()
        {
            int byteCount = (_bitArray.Count + 7) / 8;
            byte[] bytes = new byte[byteCount];
            _bitArray.CopyTo(bytes, 0);

            long[] longs = new long[byteCount / sizeof(long)];
            Buffer.BlockCopy(bytes, 0, longs, 0, bytes.Length);
            return longs;
        }

        public int this[int index]
        {
            get
            {
                int startBit = index * _nbits;
                if (startBit + _nbits > _bitArray.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                int value = 0;
                for (int i = 0; i < _nbits; i++)
                {
                    int bitIndex = startBit + i;
                    if (_bitArray[bitIndex])
                    {
                        value |= 1 << i; // 低位在前
                    }
                }
                return value;
            }
            set
            {
                int startBit = index * _nbits;
                if (startBit + _nbits > _bitArray.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                if (value < 0 || value >= (1 << _nbits))
                    throw new ArgumentOutOfRangeException(nameof(value), "Value exceeds bit capacity.");

                for (int i = 0; i < _nbits; i++)
                {
                    int bitIndex = startBit + i;
                    bool bitValue = (value & (1 << i)) != 0; // 低位在前
                    _bitArray[bitIndex] = bitValue;
                }
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < size; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public int Size => size;
    }
}
