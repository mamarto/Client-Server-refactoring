using Server.states;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class ClientAdder
    {
        State waitingState;
        State clientConnectedState;
        State usernameReceivedState;
        State usernameValidState;
        State clientAddedState;
        State state;

        private GUI gui = new GUI();
        private Server serverObject = new Server();

        private TcpListener server;
        private TcpClient clientToAdd;
        private string username;
        private NetworkStream stream;
        private IDictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();

        public TcpListener Server { get => server; set => server = value; }
        public TcpClient ClientToAdd { get => clientToAdd; set => clientToAdd = value; }
        public string Username { get => username; set => username = value; }
        public NetworkStream Stream { get => stream; set => stream = value; }
        public IDictionary<string, TcpClient> Clients { get => clients; set => clients = value; }
        internal State State { get => state; set => state = value; }
        internal State WaitingState { get => waitingState; set => waitingState = value; }
        internal State ClientConnectedState { get => clientConnectedState; set => clientConnectedState = value; }
        internal State UsernameReceivedState { get => usernameReceivedState; set => usernameReceivedState = value; }
        internal State UsernameValidState { get => usernameValidState; set => usernameValidState = value; }
        internal State ClientAddedState { get => clientAddedState; set => clientAddedState = value; }

        public ClientAdder(TcpListener server, IDictionary<string, TcpClient> clients)
        {
            this.Server = server;
            this.Clients = clients;

            waitingState = new WaitingState(this);
            clientConnectedState = new ClientConnectedState(this);
            usernameReceivedState = new UsernameReceivedState(this);
            usernameValidState = new UsernameValidState(this);
            clientAddedState = new ClientAddedState(this);

            state = clientConnectedState;
        }
        
        public void addNewClient()
        {
            Console.WriteLine(String.Format("[Handshake]: Granting username '{0}' ", Username));
            Clients.Add(Username, ClientToAdd);
        }

        public TcpClient connectToClient()
        {
            TcpClient client = Server.AcceptTcpClient();
            Console.WriteLine("[Handshake]: New client connected");
            return client;
        }

        public void getPendingConnections(IDictionary<string, TcpClient> clients)
        {
            state.getPendingConnections(clients);
        }

        public void getUsername()
        {
            state.getUsername();
        }

        public void validateUsername()
        {
            state.validateUsername();
        }

        public void addClient()
        {
            state.addClient();
        }

        public void sendUsernameResponse()
        {
            state.sendUsernameResponse();
        }
    }
}
