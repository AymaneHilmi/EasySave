using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServeurClient
{
    public class Client
    {
        private TcpClient client;

        private NetworkStream stream;
        
        public Client(string serverIp, int port)
        {
            client = new TcpClient(serverIp, port);
            stream = client.GetStream();
        }

        public void SendData(string data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            stream.Write(buffer, 0, buffer.Length);
        }

        public void Close()
        {
            client.Close();
        }
        
        public double GetProgress()
        {
            byte[] buffer = new byte[8];
            stream.Read(buffer, 0, 8);
            return BitConverter.ToDouble(buffer, 0);
        }

    }
}