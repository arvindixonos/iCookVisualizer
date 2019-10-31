using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioIK;

namespace iCook
{
    public class iCookVisualizer : Singleton<iCookVisualizer>
    {
        public static string remoteIPAddress = "192.168.0.31";
        public static int remotePORT = 5555;

        public iCookHand cookHand;
        private UDPConnection udpConnection;

        private RecipeManager recipeManager;

        void Start()
        {
            InitializeiCook();
        }

        void InitializeiCook()
        {
            udpConnection = new UDPConnection(remoteIPAddress, remotePORT);
            recipeManager = new RecipeManager();
            recipeManager.Init();

            cookHand.OnAngleChanged += JointAngleChanged;
        }

        public void JointAngleChanged(eJointType jointType, float currentAngle)
        {
            byte[] jointPayload = GetJointPayload(jointType, currentAngle);
            udpConnection.SendNetworkMessage(eNetworkMessageType.NM_JOINT_INFO, jointPayload);
        }

        public byte[] GetJointPayload(eJointType jointType, float currentAngle)
        {
            byte[] jointTypeBytes = SerializerDeserializer.ToByteArray((int)jointType);
            byte[] currentAngleBytes = SerializerDeserializer.ToByteArray(currentAngle);

            byte[] accumulatedBytes = new byte[jointTypeBytes.Length + currentAngleBytes.Length];

            int destinationIndex = 0;
            int copyLength = 0;

            copyLength = SerializerDeserializer.AddByteArray(accumulatedBytes, jointTypeBytes, destinationIndex);
            destinationIndex += copyLength;

            copyLength = SerializerDeserializer.AddByteArray(accumulatedBytes, currentAngleBytes, destinationIndex);
            destinationIndex += copyLength;

            return accumulatedBytes;
        }

        void Update()
        {
            cookHand.UpdateHand();
        }
    }
}
