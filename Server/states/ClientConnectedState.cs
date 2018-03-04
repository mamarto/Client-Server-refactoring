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
    class ClientConnectedState : State
    {
        public ClientConnectedState(ClientAdder clientAdder)
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
            try
            {
                //TODO: extract method
                clientAdder.Stream = clientAdder.ClientToAdd.GetStream();
                byte[] buffer = new byte[clientAdder.ClientToAdd.ReceiveBufferSize];
                int data = clientAdder.Stream.Read(buffer, 0, clientAdder.ClientToAdd.ReceiveBufferSize);
                clientAdder.Username = Encoding.Unicode.GetString(buffer, 0, data);
                Console.WriteLine(String.Format("[Handshake]: Requesting username '{0}' ", clientAdder.Username));
                clientAdder.State = clientAdder.UsernameReceivedState;
            }
            catch (IOException e)
            {
                string error = e.Message;
                Console.WriteLine(String.Format("[Handshake]: Did not receive username from new conncetion ({0})", error));
                clientAdder.State = clientAdder.WaitingState;
            }
        }

        public override void sendUsernameResponse()
        {
            clientAdder.State = clientAdder.WaitingState;
        }

        public override void validateUsername()
        {
            clientAdder.State = clientAdder.WaitingState;
        }
    }
}
