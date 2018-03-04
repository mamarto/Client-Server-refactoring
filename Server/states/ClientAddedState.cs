using Program;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.states
{
    class ClientAddedState : State
    {
        public ClientAddedState(ClientAdder clientAdder)
        {
            this.clientAdder = clientAdder;
        }

        public override void addClient()
        {
            clientAdder.State = clientAdder.WaitingState;
        }

        public override void getPendingConnections(IDictionary<string, TcpClient> clients)
        {
            clientAdder.State = clientAdder.WaitingState;
        }

        public override void getUsername()
        {
            clientAdder.State = clientAdder.WaitingState;
        }

        public override void sendUsernameResponse()
        {
            try
            {
                string response = "ACCEPT";
                byte[] message = Encoding.Unicode.GetBytes(response);
                clientAdder.Stream.Write(message, 0, message.Length);
            }
            catch (IOException exception)
            {
                string error = exception.Message;
                Console.WriteLine(String.Format("[Handshake]: Could not send message back to client that requested {0} ({1})", clientAdder.Username, error));
            }
            clientAdder.State = clientAdder.WaitingState;
        }

        public override void validateUsername()
        {
            clientAdder.State = clientAdder.WaitingState;
        }
    }
}
