using ServerCore1;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;


namespace Server
{


    class Program
    {
        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.Init(endPoint, () => { return new ClientSession(); });
            while (true)
            {
                //  Console.WriteLine("Listening...");
            }


        }


    }
}
