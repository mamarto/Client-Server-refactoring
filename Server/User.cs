using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class User
    {
        public bool isUsernameValid(string username, IDictionary<string, TcpClient> clients)
        {
            bool isOk = String.IsNullOrEmpty(username) ? false : true;
            // Uniqueness
            foreach (var user in clients)
            {
                if (user.Key == username)
                {
                    isOk = false;
                }
            }
            // Length
            if (username.Length > 10)
            {
                isOk = false;
            }

            return isOk;
        }
    }
}
