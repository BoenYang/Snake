﻿namespace CGF.Network.Core
{
    public class ProtocalHead
    {
        public const int HeadSize = 16;
        public uint cmd = 0;
        public uint uid = 0;
        public uint dataSize;
        public uint index = 0;

        public ProtocalHead Deserialize(NetBuffer buffer)
        {
            cmd = buffer.ReadUInt();
            dataSize = buffer.ReadUInt();
            index = buffer.ReadUInt();
            uid = buffer.ReadUInt();
            return this;
        }
    }
}