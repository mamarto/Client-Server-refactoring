using System;
using System.Net;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Program
{

    public class Program
    {
        static public void Main()
        {
            Server server = new Server();

            server.startChat();
        }
    }
}