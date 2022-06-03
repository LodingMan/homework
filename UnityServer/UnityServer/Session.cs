using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections.Generic;

namespace UnityServer
{
    class Session
    {
        Socket _serverSocket;
        SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();

        List<ArraySegment<byte>> bufferList = new List<ArraySegment<byte>>();

        public void SessionOn(Socket serverSocket)
        {
            _serverSocket = serverSocket;
            System.Console.WriteLine("sessionStart");

            recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(ReceiveComplete);
            sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(SendComplete);

            RegistReceive();


        }


        public void Send(Packet packet)
        {
            RegistSend(sendArgs, packet);
        }



        public void RegistReceive()
        {
            byte[] recvBuffer = new byte[1024];
            recvArgs.SetBuffer(recvBuffer, 0, 1024); //나중에 버퍼 컨트롤 하는 클래스 생성필요. 
           bool pending = _serverSocket.ReceiveAsync(recvArgs);
            Console.WriteLine("수신대기");
            if (!pending)
            {
                ReceiveComplete(null, recvArgs);
            }

        }
        public void RegistSend(SocketAsyncEventArgs args, Packet packet)
        {
            sendArgs.SetBuffer(new byte[packet.Data.Length], 0, packet.Data.Length);

            bool pending = _serverSocket.SendAsync(args);
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

        public void ReceiveComplete(object send, SocketAsyncEventArgs args)
        {
            if(args.SocketError == SocketError.Success)
            {
                if(args.BytesTransferred > 0)
                {
                    string receiveMessage = Encoding.UTF8.GetString(args.Buffer, 0, args.Buffer.Length);
                    Console.WriteLine(receiveMessage);
                    RegistReceive();
                }
            }
            else
            {
                Thread.Sleep(500);
                RegistReceive();
            }
        }
    }
}
