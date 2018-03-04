using Program;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.states
{
    class UsernameReceivedState : State
    {
        private User user = new User();

        public UsernameReceivedState(ClientAdder clientAdder)
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
            clientAdder.State = clientAdder.WaitingState;
        }

        public override void validateUsername()
        {
            bool isOk = user.isUsernameValid(clientAdder.Username, clientAdder.Clients);

            if (isOk)
            {
                clientAdder.State = clientAdder.UsernameValidState;
            }
            else
            {
                clientAdder.State = clientAdder.WaitingState;
            }
        }

    }
}
