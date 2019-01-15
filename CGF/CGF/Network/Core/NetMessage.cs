using System;

namespace CGF.Network.Core
{
    public class NetMessage
    {
        public ProtocalHead head = new ProtocalHead();
        public byte[] content;

        public static NetBufferReader DefaultReader = new NetBufferReader();
        public static NetBufferReader DefaultWriter = new NetBufferReader();

        public NetMessage Deserialize(NetBuffer buffer)
        {
            head.Deserialize(buffer);
            content = new byte[head.dataSize];
            buffer.ReadBytes(content, 0, (int)head.dataSize);
            return this;
        }

        public NetMessage Deserialize(byte[] buffer, int len)
        {
            lock (DefaultReader)
            {
                DefaultReader.Attach(buffer, len);
                return Deserialize(DefaultReader);
            }
        }

        public NetBuffer Serialize(NetBuffer buffer)
        {
            head.Serialize(buffer);
            buffer.WriteBytes(content, 0, (int)head.dataSize);
            return buffer;
        }

        public int Serialize(out byte[] tempBuffer)
        {
            lock (head)
            {
                DefaultWriter.Clear();
                this.Deserialize(DefaultWriter);
                tempBuffer = DefaultWriter.GetBytes();
                return DefaultWriter.Length;
            }
        }
    }
}