using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace iCook
{
    public class UDPConnection
    {
        private UdpClient udpClient;
        private IPEndPoint ipEndPoint;

        public UDPConnection(string ipaddress, int port)
        {
            CreateUDPConnection(ipaddress, port);
        }

        void CreateUDPConnection(string ipaddress, int port)
        {
            udpClient = new UdpClient();
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipaddress), port);
            udpClient.Connect(ipEndPoint);

            if(udpClient.Client.Connected)
            {
                Debug.Log("Connected to iCook");
            }
        }

        void CloseConnection()
        {
            Debug.Log("Closing UDP Connection");

            if (udpClient != null)
            {
                udpClient.Close();
                udpClient.Dispose();
                udpClient = null;
            }
        }

        public void SendNetworkMessage(eNetworkMessageType networkMessageType, byte[] payload)
        {
            NetworkMessage networkMessage = new NetworkMessage(networkMessageType, payload);
            SendData(NetworkMessage.Bytes(networkMessage));
        }

        public void SendData(byte[] dataToSend)
        {
            if (dataToSend != null)
            {
                udpClient.Send(dataToSend, dataToSend.Length);
            }
        }

        ~UDPConnection()
        {
            CloseConnection();
        }
    }
}

