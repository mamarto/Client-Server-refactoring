using Program;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.states
{
    class WaitingState : State
    {
        public WaitingState(ClientAdder clientAdder)
        {
            this.clientAdder = clientAdder;
        }

        public override void addClient()
        {
            clientAdder.State = clientAdder.WaitingState;
        }

        public override void getPendingConnections(IDictionary<string, TcpClient> clients)
        {
            clientAdder.Clients = clients;
            if (clientAdder.Server.Pending())
            {
                // Connect to the client
                clientAdder.ClientToAdd = clientAdder.connectToClient();
                clientAdder.State = clientAdder.ClientConnectedState;
            }
        }

        public override void getUsername()
        {
            clientAdder.State = clientAdder.WaitingState;
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
