using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Program
{

    public class Program
    {
        private static Client client = new Client();
        
        public static void Main()
        {
            // Chat loop
            client.chatLoop();
        }
    }
}
