using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Program
{
    class Client
    {
        private List<KeyValuePair<string, string>> history = new List<KeyValuePair<string, string>>();
        private string text = String.Empty;
        private bool redrawUI = true;

        private NetworkStream stream;
        private TcpClient client;

        private string receivedUsername;
        private string receivedText;

        private GUI gui = new GUI();
        private Server server = new Server();
        private User user;

        public void chatLoop()
        {
            server.start();

            stream = server.Stream;
            client = server.Client;

            user = new User(server.Stream, server.Client);

            user.getUsername();

            while (true)
            {
                // No need to iterate too quickly
                Thread.Sleep(10);

                // Limit history to screen size
                limitHistory();

                // Print user input
                gui.printUserInput(text, history, redrawUI);

                // Check if user has pressed key
                UserKeyPressed();

                // Receive new messages
                receiveNewMessages();
            }
        }

        private void receiveNewMessages()
        {
            if (stream.DataAvailable)
            {
                redrawUI = true;

                // Get message
                getMessage();

                // Add to history
                addMessageToHistory();
            }
        }

        private void addMessageToHistory()
        {
            history.Insert(0, new KeyValuePair<string, string>(
                  receivedUsername, receivedText));
        }

        private void limitHistory()
        {
            int overflow = history.Count - Console.WindowHeight + 6;
            if (overflow > 0)
            {
                history.Reverse();
                history.RemoveRange(0, overflow);
                history.Reverse();
            }
        }

        private string sendMessage()
        {
            history.Insert(0, new KeyValuePair<string, string>("You", text));
            string message = String.Format("{0}:{1}", user.Username, text);
            byte[] payload = Encoding.Unicode.GetBytes(message);
            try
            {
                stream.Write(payload, 0, payload.Length);
            }
            catch (SocketException exception)
            {
                string error = exception.Message;
                Console.WriteLine(String.Format("Could not send message to server ({0})", error));
            }
            text = String.Empty;
            return text;
        }

        private void UserKeyPressed()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                redrawUI = true;

                // Choose action
                switch (key.Key)
                {
                    // Delete character
                    case ConsoleKey.Backspace:
                        if (text.Length > 0)
                            text = text.Substring(0, text.Length - 1);
                        break;

                    // Send message
                    case ConsoleKey.Enter:
                        text = sendMessage();
                        break;

                    // Add character to user input string
                    default:
                        text += key.KeyChar;
                        break;
                }
            }
            else
            {
                redrawUI = false;
            }
        }

        private void getMessage()
        {
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int data = stream.Read(buffer, 0, client.ReceiveBufferSize);
            string receivedLine = Encoding.Unicode.GetString(buffer, 0, data);

            List<string> parts = receivedLine.Split(':').ToList();
            receivedUsername = parts[0];
            parts.RemoveAt(0);
            receivedText = String.Join(":", parts).Trim();
        }
    }
}
