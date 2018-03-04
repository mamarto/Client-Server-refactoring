using Program;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.states
{
    abstract class State
    {
        public ClientAdder clientAdder;

        public abstract void getPendingConnections(IDictionary<string, TcpClient> clients);

        public abstract void getUsername();

        public abstract void validateUsername();

        public abstract void addClient();

        public abstract void sendUsernameResponse();
        
    }
}
