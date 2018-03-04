using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class User
    {
        private string username;
        private NetworkStream stream;
        private string response;
        private TcpClient client;

        public User(NetworkStream stream, TcpClient client)
        {
            this.stream = stream;
            this.client = client;
        }

        public string Username { get => username; set => username = value; }

        public void getUsername()
        {
            do
            {
                // Choose username
                Username = Console.ReadLine().Trim();

                // Validate username
                if (String.IsNullOrEmpty(Username))
                {
                    continue;
                }

                sendUsername();
                isAvailble();

                if (response != "ACCEPT")
                {
                    Console.WriteLine("Username denied... Max 10 chars. Must be unqiue. Try again:");
                }

            } while (response != "ACCEPT");
        }

        private void isAvailble()
        {
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int data = stream.Read(buffer, 0, client.ReceiveBufferSize);
            response = Encoding.Unicode.GetString(buffer, 0, data);
        }

        private void sendUsername()
        {
            byte[] message = Encoding.Unicode.GetBytes(Username);
            try
            {
                stream.Write(message, 0, message.Length);
            }
            catch (SocketException exception)
            {
                string error = exception.Message;
                Console.WriteLine(String.Format("Could not send username to server ({0})", error));
            }
        }
    }
}
