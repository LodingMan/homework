using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace UnityServer
{

    public abstract class Packet
    {
        public ushort size;
        public ushort type;
        public byte[] Data;

    }

    class MessagePacket : Packet
    {
        public MessagePacket(string message)
        {
            GetStringToByte(message);
        }

        public void GetStringToByte(string messgae)
        {

            this.type = (ushort)0;
            this.size = (ushort)(2 + 2 + Encoding.UTF8.GetBytes(messgae).Length);
            this.Data = Encoding.UTF8.GetBytes(messgae);

        }
    }

    class Program
    {
        static Listener listener;
        //static Session session;

        static void Main(string[] args)
        {
            
            IPHostEntry iPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAdress = iPHostEntry.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(iPAdress, 7777);

            listener = new Listener();
            listener.ListenOn(endPoint, ()=> { return new Session(); });
            while(true)
            {

            }

        }







    }
}
