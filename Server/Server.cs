using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Program
{
    class Server
    {
        private GUI gui = new GUI();
        private ClientAdder clientAdder;
        private IDictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();

        private TcpListener server;
        private int port;

        public void startChat()
        {
            configureServer();
            clientAdder = new ClientAdder(server, clients);

            while (true)
            {
                Thread.Sleep(100);

                // Accept new clients:
                acceptNewClient();

                // Remove disconnected clients:
                handlePendingDeletion();

                // Check if new messages have arrived and broadcast:
                broadcastToClients();

                // Close connection to client
                //client.Close();
            }
        }

        private void configureServer()
        {
            // Choose port
            setPort();

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(ip, port);

            // Create and start server
            startServer();
        }

        private void acceptNewClient()
        {
            clientAdder.getPendingConnections(clients);
            clientAdder.getUsername();
            clientAdder.validateUsername();
            clientAdder.addClient();
            clientAdder.sendUsernameResponse();
        }

        private void broadcast(string senderName, string text)
        {
            foreach (var receiver in clients)
            {
                string receiverName = receiver.Key;
                if (receiverName != senderName)
                {
                    TcpClient receiverClient = receiver.Value;
                    Console.WriteLine(String.Format("[Send] from [{0}] to [{1}]", senderName, receiverName));
                    try
                    {
                        NetworkStream receiverStream = receiverClient.GetStream();
                        byte[] message = Encoding.Unicode.GetBytes(text);
                        receiverStream.Write(message, 0, message.Length);
                    }
                    catch (IOException exception)
                    {
                        string error = exception.Message;
                        Console.WriteLine(String.Format("[ERROR]: Broadcast from [{0}] to [{1}] failed.", senderName, receiverName));
                    }
                }
            }
        }

        private void broadcastToClients()
        {
            foreach (var sender in clients)
            {
                string senderName = sender.Key;
                TcpClient senderClient = sender.Value;
                NetworkStream senderStream = senderClient.GetStream();

                // Is there a new message?
                if (senderStream.DataAvailable)
                {
                    string text = readMessage(senderName, senderClient, senderStream);

                    gui.displayMessage(text);

                    // Broadcast to all other clients
                    broadcast(senderName, text);
                }
            }
        } 

        private void startServer()
        {
            try
            {
                server.Start();
                Console.WriteLine(String.Format("[Server]: Started at port {0}", port));
            }
            catch (SocketException exception)
            {
                string error = exception.Message;
                Console.WriteLine(String.Format("Could not start server: {0}", error));
            }
        }

        private void setPort()
        {
            Console.WriteLine("Please enter port number to start server at (default: 8888)");
            string portInput = Console.ReadLine();
            port = int.TryParse(portInput, out port) ? port : 8888;
        }

        private void handlePendingDeletion()
        {
            bool hadStaleClients = clients.Any(c => !c.Value.Connected);
            foreach (var client in clients.Where(c => !c.Value.Connected).ToList())
            {
                Console.WriteLine(String.Format("[Remove]: [{0}]", client.Key));
            }
            clients = clients.Where(c => c.Value.Connected)
              .ToDictionary(c => c.Key, c => c.Value);
            if (hadStaleClients)
            {
                Console.WriteLine("===MEMBERS===");
                foreach (var c in clients)
                {
                    Console.WriteLine(c.Key);
                }
                Console.WriteLine("=============");
            }
        }

        private string readMessage(string senderName, TcpClient senderClient, NetworkStream senderStream)
        {
            string text = "";

            try
            {
                // Read the message
                byte[] buffer = new byte[senderClient.ReceiveBufferSize];
                int data = senderStream.Read(buffer, 0, senderClient.ReceiveBufferSize);
                text = Encoding.Unicode.GetString(buffer, 0, data);
            }
            catch (SocketException exception)
            {
                string error = exception.Message;
                Console.WriteLine(String.Format("[ERROR]: Could not read message from [{0}]", senderName));

            }

            return text;
        }
    }
}
