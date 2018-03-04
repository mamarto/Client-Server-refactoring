using Program;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.states
{
    class UsernameValidState : State
    {
        private GUI gui = new GUI();

        public UsernameValidState(ClientAdder clientAdder)
        {
            this.clientAdder = clientAdder;
        }

        public override void addClient()
        {
            clientAdder.addNewClient();

            // Print all current users
            gui.printAllClients(clientAdder.Clients);

            clientAdder.State = clientAdder.ClientAddedState;
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
            clientAdder.State = clientAdder.WaitingState;
        }

        public override void validateUsername()
        {
            clientAdder.State = clientAdder.WaitingState;
        }
    }
}
