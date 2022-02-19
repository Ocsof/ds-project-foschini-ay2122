using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace RethinkDbLib

{
    class Program
    {
        static void Main(string[] args)
        {
            IList<string> hostsPorts = new List<string>();

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    string hostPort = ip.ToString() + ":28016";
                    hostsPorts.Add(hostPort);
                    Console.WriteLine(hostPort);
                }
            }
        }
    }
}
