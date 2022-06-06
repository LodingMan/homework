using System; 
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace UnityServer
{
    public class LobbyData
    {
        public Session _session;

        public List<User> lobbyUsers = new List<User>();
        public void SendLobbyData(Session session, string name)
        {
            for (int i = 0; i < lobbyUsers.Count; i++)
            {
                PacketHub packet = new PacketHub(lobbyUsers[i]._name, (int)PacketType.Name);

                for (int j = 0; j < lobbyUsers.Count; j++)
                {
                    lobbyUsers[j]._userSession.Send(packet);
                    Console.WriteLine(lobbyUsers[j]._name + " 에게" +lobbyUsers[i]._name +"데이터 보냄"); 
                }
            }
            Console.WriteLine("현재 서버에 " + lobbyUsers.Count + " 명이 있습니다");
        }


        public void DisConnectLobyUser(EndPoint userEndPoint)
        {
            for(int i = 0; i < lobbyUsers.Count; i++)
            {
                if(lobbyUsers[i]._userEndPoint == userEndPoint)
                {
                    Console.WriteLine(lobbyUsers[i]._name + "가 지워짐");
                    lobbyUsers.RemoveAt(i);
                }
            }

        }




    }
} 