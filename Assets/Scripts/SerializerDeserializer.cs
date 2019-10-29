using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace iCook
{
    public static class SerializerDeserializer
    {
        public static int GetInt32(byte[] bytes, int startIndex)
        {
            int sizeofint = sizeof(Int32);

            byte[] intBytes = new byte[sizeofint];
            Array.Copy(bytes, startIndex, intBytes, 0, sizeofint);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);

            return BitConverter.ToInt32(intBytes, 0);
        }


        public static float GetFloat(byte[] bytes, int startIndex)
        {
            int sizeoffloat = sizeof(float);

            byte[] floatBytes = new byte[sizeoffloat];
            Array.Copy(bytes, startIndex, floatBytes, 0, sizeoffloat);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(floatBytes);

            return BitConverter.ToSingle(floatBytes, 0);
        }

        public static byte[] GetByteArray(byte[] bytes, int startIndex, int readLength)
        {
            byte[] byteArray = new byte[readLength];
            Array.Copy(bytes, startIndex, byteArray, 0, readLength);

            return byteArray;
        }

        public static string GetString(byte[] bytes, int startIndex, int readLength)
        {
            byte[] stringBytes = new byte[readLength];
            Array.Copy(bytes, startIndex, stringBytes, 0, readLength);

            return Encoding.UTF8.GetString(stringBytes);
        }

        public static byte[] ToByteArray(int value)
        {
            byte[] resultBytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(resultBytes);

            return resultBytes;
        }

        public static byte[] ToByteArray(float value)
        {
            byte[] resultBytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(resultBytes);

            return resultBytes;
        }

        public static byte[] ToByteArray(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static int AddByteArray(byte[] destination, byte[] source, int destinationIndex)
        {
            Array.Copy(source, 0, destination, destinationIndex, source.Length);

            return source.Length;
        }
    }
}
