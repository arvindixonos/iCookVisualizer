using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iCook
{
    public enum eNetworkMessageType
    {
        NM_JOINT_INFO
    }

    public class NetworkMessage
    {
        public eNetworkMessageType messageType;
        public byte[] payload;

        public NetworkMessage(eNetworkMessageType messageType, byte[] payload)
        {
            this.messageType = messageType;
            this.payload = payload;
        }

        public static NetworkMessage FromBytes(byte[] bytes)
        {
            int sizeofint = sizeof(Int32);

            int currentIndex = 0;

            eNetworkMessageType messageType = (eNetworkMessageType)SerializerDeserializer.GetInt32(bytes, currentIndex);
            currentIndex += sizeofint;

            int payloadLength = SerializerDeserializer.GetInt32(bytes, currentIndex);
            currentIndex += sizeofint;

            byte[] payload = SerializerDeserializer.GetByteArray(bytes, currentIndex, payloadLength);

            return new NetworkMessage(messageType, payload);
        }

        public static byte[] Bytes(NetworkMessage networkMessage)
        {
            byte[] messageTypeBytes = SerializerDeserializer.ToByteArray((int)networkMessage.messageType);

            int payloadSizeBytesLength = 0;
            byte[] payloadSizeLengthBytes = new byte[4];
            if (networkMessage.payload != null)
            {
                payloadSizeLengthBytes = SerializerDeserializer.ToByteArray(networkMessage.payload.Length);
                payloadSizeBytesLength = networkMessage.payload.Length;
            }

            byte[] accumulatedBytes = null;

            if (payloadSizeBytesLength == 0)
            {
                accumulatedBytes = new byte[messageTypeBytes.Length + payloadSizeLengthBytes.Length];
            }
            else
            {
                accumulatedBytes = new byte[messageTypeBytes.Length + payloadSizeLengthBytes.Length + networkMessage.payload.Length];
            }

            int destinationIndex = 0;
            int copyLength = 0;

            copyLength = SerializerDeserializer.AddByteArray(accumulatedBytes, messageTypeBytes, destinationIndex);
            destinationIndex += copyLength;
            copyLength = SerializerDeserializer.AddByteArray(accumulatedBytes, payloadSizeLengthBytes, destinationIndex);
            destinationIndex += copyLength;

            if (payloadSizeBytesLength > 0)
            {
                copyLength = SerializerDeserializer.AddByteArray(accumulatedBytes, networkMessage.payload, destinationIndex);
                destinationIndex += copyLength;
            }

            return accumulatedBytes;
        }
    }
}