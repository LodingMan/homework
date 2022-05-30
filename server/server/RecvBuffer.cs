using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    public class RecvBuffer
    {
        ArraySegment<byte> _buffer;
        int _readPos;
        int _writePos;

        public RecvBuffer(int bufferSize)
        {
            _buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
        }

        public int DataSize { get { return _writePos - _readPos; } }
        public int FreeSize { get { return _buffer.Count - _writePos; } }

        public ArraySegment<byte> DataSegment
        {
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset+_readPos, DataSize); }
        }
        public ArraySegment<byte> RecvSegment
        {
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _writePos, FreeSize); }
        }

        public void Clean()
        {
            int dataSize = DataSize;
            if(dataSize == 0)
            {
                _readPos = _writePos = 0;
            }
            else
            {
                Array.Copy()
            }
        }
    }
}
