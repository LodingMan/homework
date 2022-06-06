using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net;

namespace UnityServer
{
    public class Session
    {
        Socket _clientSocket;
        SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();

        static LobbyData lobyData = new LobbyData();

        List<ArraySegment<byte>> bufferList = new List<ArraySegment<byte>>();


        public void SessionOn(Socket clientSocket)
        {
            lobyData._session = this;
            _clientSocket = clientSocket;
            System.Console.WriteLine("sessionStart");

            recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(ReceiveComplete);
            sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(SendComplete);

            RegistReceive();


        }

        public void Disconnect(Socket socket)
        {
            lobyData.DisConnectLobyUser(socket.RemoteEndPoint);
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

        }


        public void Send(Packet packet)
        {
            RegistSend(sendArgs, packet);
        }

        public void RegistReceive()
        {
            byte[] recvBuffer = new byte[1024];
            recvArgs.SetBuffer(recvBuffer, 0, 1024); //나중에 버퍼 컨트롤 하는 클래스 생성필요. 
            bool pending = _clientSocket.ReceiveAsync(recvArgs);
            Console.WriteLine("수신대기");
            if (!pending)
            {
                ReceiveComplete(null, recvArgs);
            }

        }
        public void RegistSend(SocketAsyncEventArgs args, Packet packet)
        {
            sendArgs.SetBuffer(packet.Data, 0, packet.Data.Length);

            bool pending = _clientSocket.SendAsync(args);
            if (!pending)
            {
                SendComplete(null, args);
            }

        }

        public void SendComplete(object send, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {

            }
            else
            {

            }
        }

        public void ReceiveComplete(object send, SocketAsyncEventArgs Args)
        {
            if (Args.SocketError == SocketError.Success) //데이터 받기 성공
            {
                if (Args.BytesTransferred > 0)
                {
                    //string str = Encoding.UTF8.GetString(args.Buffer);
                    //    Console.WriteLine(str);
                    //클라이언트 소켓이 데이터를 보냈으니 

                    string bufferData = DataExtract(Args.Buffer);
                    switch (PacketCheck(Args.Buffer)) //type확인 
                    {
                        case (int)PacketType.Text:
                            Console.WriteLine("textData");
                            break;
                        case (int)PacketType.Name:
                            Console.WriteLine("nameData");
                            break;
                        case (int)PacketType.Port:
                            Console.WriteLine("portData");
                            break;
                        case (int)PacketType.Login: //로그인시 사용한 Name가 버퍼에 저장됨
                            lobyData.lobbyUsers.Add(new User(bufferData, this, _clientSocket.RemoteEndPoint));
                            lobyData.SendLobbyData(this, bufferData);
                            Console.WriteLine("loginData");
                            break;
                        case (int)PacketType.Logout:
                            Console.WriteLine("logoutData");
                            break;
                        default:
                            Console.WriteLine("타입 불일치");
                            Disconnect(_clientSocket);
                            return;
                    }
                    RegistReceive();
                }
                else
                {
                    Thread.Sleep(500);
                    Disconnect(_clientSocket);
                    Console.WriteLine("DisConnectClient");
                    return;
                }
            }
            else
            {
                Thread.Sleep(500);
                Console.WriteLine("ReciveFail");
                Disconnect(_clientSocket);
                return;

            }
        }

        public ushort PacketCheck(byte[] packet)
        {
            ArraySegment<byte> search = new ArraySegment<byte>(packet, 2, 2);
            ushort TestBuff = BitConverter.ToUInt16(search.Array, search.Offset);
            Console.WriteLine("buff Type : " + TestBuff);

            return TestBuff;
        }
        public string DataExtract(byte[] data)
        {
            byte[] Data = new byte[data.Length - 4];
            Array.Copy(data, 4, Data, 0, data.Length - 4);
            return Encoding.UTF8.GetString(Data);
            
        }

    }
}
