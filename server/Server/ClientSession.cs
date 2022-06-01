using ServerCore1;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace Server
{
    public abstract class Packet
    {
        public ushort size;
        public ushort pakcetId;

        public abstract ArraySegment<byte> Write();
        public abstract void Read(ArraySegment<byte> s);
    }

    class PlayerInfoReq : Packet
    {
        public long playerId;

        public PlayerInfoReq()
        {
            this.pakcetId = (ushort)PacketID.PlayerInfoReq;
        }

        public override void Read(ArraySegment<byte> s)
        {
            ushort count = 0;

            //    ushort size = BitConverter.ToUInt16(s.Array, s.Offset);
            count += 2;
            //   ushort id = BitConverter.ToUInt16(s.Array, s.Offset + count);
            count += 2;
            this.playerId = BitConverter.ToInt64(new ReadOnlySpan<byte>(s.Array, s.Offset + count, s.Count - count));
            count += 8;
        }


        public override ArraySegment<byte> Write()
        {
            ushort count = 0;
            ArraySegment<byte> s = SendBufferHelper.Open(4096);
            bool success = true;


            count += 2;
            success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + count, s.Count - count), this.pakcetId);
            count += 2;
            success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + count, s.Count - count), this.playerId);
            count += 8;
            success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset, s.Count), count);

            ArraySegment<byte> sendBuff = SendBufferHelper.Close(count);

            if (success == false)
            {
                return null;
            }

            return SendBufferHelper.Close(count);

        }

    }

    public enum PacketID
    {
        PlayerInfoReq = 1,
        PlayerInfoOk = 2
    }

    class ClientSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            //  Packet Packet = new Packet() { size = 100, pakcetId = 10 };
            //  ArraySegment<byte> openSegment =  SendBufferHelper.Open(4096);

            //  byte[] buffer = BitConverter.GetBytes(Packet.size);
            //  byte[] buffer2 = BitConverter.GetBytes(Packet.pakcetId);
           //  Array.Copy(buffer, 0, openSegment.Array, openSegment.Offset, buffer.Length);
            //  Array.Copy(buffer2, 0, openSegment.Array, openSegment.Offset + buffer.Length, buffer2.Length);
            //  ArraySegment<byte> sendBuff = SendBufferHelper.Close(buffer.Length + buffer2.Length);

            //  Send(sendBuff);
            Thread.Sleep(5000);
            Disconnect();

        }
        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            ushort count = 0;

            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            count += 2;
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;

            switch ((PacketID)id)
            {
                case PacketID.PlayerInfoReq:
                    {

                        PlayerInfoReq p = new PlayerInfoReq();
                        p.Read(buffer);

                        Console.WriteLine($"PlayerInfoReq : {p.playerId}");
                    }
                    break;
                case PacketID.PlayerInfoOk:
                    break;
                default:
                    break;
            }


            Console.WriteLine($"RecvPacketId : {id}, size : {size}");
        }
        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");

        }

        public override void OnSend(int numOfbytes)
        {
            Console.WriteLine($"Transferred Bytes: {numOfbytes}");

        }
    }
}
