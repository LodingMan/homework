using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections.Generic;

namespace UnityServer
{

    class Listener
    {
        Socket ServerSocket;

        Func<Session> _sessionOn;

        public void ListenOn(EndPoint endPoint, Func<Session> sessionOn)
        {
            _sessionOn = sessionOn;
            ServerSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(ListenSuccess);

            ServerSocket.Bind(endPoint);
            ServerSocket.Listen(10);

            RegistLintener(args);
            
        }


        public void RegistLintener(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            bool pending = ServerSocket.AcceptAsync(args);
            Console.WriteLine("연결대기");
            if(!pending)
            {
                ListenSuccess(null,args);
            }
        }

        public void ListenSuccess(object send, SocketAsyncEventArgs args)
        {
            if(args.SocketError == SocketError.Success)
            {
                Console.WriteLine("AcceptSuccess");
                Session session = _sessionOn.Invoke();
                session.SessionOn(args.AcceptSocket);
                Thread.Sleep(500);

                Console.WriteLine("로그인 이후");
            }
            else
            {
                Console.WriteLine("AcceptFail");
            }
            RegistLintener(args);
        }


    }



}
