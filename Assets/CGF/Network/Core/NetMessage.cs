using System;

namespace CGF.Network.Core
{
    public class NetMessage
    {
        public ProtocalHead head = new ProtocalHead();
        public byte[] content;

        public static NetBufferReader DefaultReader = new NetBufferReader();

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

    }
}