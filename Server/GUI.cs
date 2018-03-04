using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class GUI
    {
        public void printAllClients(IDictionary<string, TcpClient> clients)
        {
            Console.WriteLine("===MEMBERS===");
            foreach (var c in clients)
            {
                Console.WriteLine(c.Key);
            }
            Console.WriteLine("=============");
        }

        public void displayMessage(string text)
        {
            Console.WriteLine(String.Format("[Broadcast]: {0}", text));
        }
    }
}
