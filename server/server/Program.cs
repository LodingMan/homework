using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace server
{
    class Program
    {
        TcpListener listener;
        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 3000);
            listener.Start();

            TcpClient client = listener.AcceptTcpClient();



            listener.Stop();

            Console.WriteLine(IPAddress.Any);


         }



        private IPAddress GetExternalIPAddress()
        {
            string externalIPString = new WebClient().DownloadString("http://icanhazip.com");

            char[] externalIPArray = externalIPString.ToCharArray();

            string externalIP = string.Empty;

            for (int i = 0; i < externalIPArray.Length; i++)
            {
                if (externalIPArray[i] == '\n')
                {
                    continue;
                }

                externalIP = externalIP + externalIPArray[i];
            }

            return IPAddress.Parse(externalIP);
        }

    }
}
