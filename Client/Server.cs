using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class Server
    {
        private int port;
        private TcpClient client;
        private NetworkStream stream;

        public TcpClient Client { get => client; set => client = value; }
        public NetworkStream Stream { get => stream; set => stream = value; }

        private void connectToServer()
        {
            Console.WriteLine(String.Format("Attempting to connect to port {0}...", port));
            try
            {
                Client = new TcpClient("127.0.0.1", port);
            }
            catch (SocketException exception)
            {
                string error = exception.Message;
                Console.WriteLine(String.Format("Could not connect to server at {0} ({1})", port, error));
            }
        }

        private void setPort()
        {
            Console.WriteLine("Please enter port number of the server (default: 8888):");
            string portInput = Console.ReadLine();
            port = int.TryParse(portInput, out port) ? port : 8888;
        }

        private void acceptNewClient()
        {
            stream = Client.GetStream();
            Console.Clear();
            Console.WriteLine("Welcome to the chat... Please enter a username:");
        }

        public void start()
        {
            // Choose port
            setPort();

            // Connect to server
            connectToServer();

            // Accept a new client
            acceptNewClient();
        }
    }
}
