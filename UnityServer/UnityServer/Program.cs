using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

namespace UnityServer
{
    public abstract class Packet
    {
        public ushort size;
        public ushort type;
        public byte[] Data;
    }

    class PacketHub : Packet
    {
        public PacketHub(string message, int _type)
        {
            switch (_type)
            {
                case 0:
                    this.type = (ushort)PacketType.Text;
                    break;
                case 1:
                    this.type = (ushort)PacketType.Name;
                    break;
                case 2:
                    this.type = (ushort)PacketType.Port;
                    break;
                case 3:
                    this.type = (ushort)PacketType.Login;
                    break;
                case 4:
                    this.type = (ushort)PacketType.Logout;
                    break;
                default:
                    break;
            }
            int Count = 0;

            this.size = (ushort)(Encoding.UTF8.GetBytes(message).Length + sizeof(ushort) + sizeof(ushort));
            this.Data = new byte[size];

            Array.Copy(BitConverter.GetBytes(size), 0, Data, 0, 2);
            Count += 2;
            Array.Copy(BitConverter.GetBytes(type), 0, Data, Count, 2);
            Count += 2;
            Array.Copy(Encoding.UTF8.GetBytes(message), 0, Data, Count, Encoding.UTF8.GetBytes(message).Length);

        }

    }

    public enum PacketType
    {
        Text = 0,
        Name,
        Port,
        Login,
        Logout
    }

    public class User
    {
        public string _name;
        public Session _userSession;
        public EndPoint _userEndPoint;

        
        public User(string name, Session session, EndPoint endPoint)
        {
            _name = name;
            _userSession = session;
            _userEndPoint = endPoint;
        }
    }

    class Program
    {
        static Listener listener;
        //static Session session;
       // static public LobbyData lobbydata = new LobbyData();


        static void Main(string[] args)
        {
            // string host = Dns.GetHostName();
            IPHostEntry iPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAdress = iPHostEntry.AddressList[1];
            IPEndPoint endPoint = new IPEndPoint(iPAdress, 7777);
             Console.WriteLine(endPoint);

            listener = new Listener();
            listener.ListenOn(endPoint, () => { return new Session(); });
            while (true)
            {

            }

        }

    }
}
