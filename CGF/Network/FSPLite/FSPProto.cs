using System.Collections.Generic;
using ProtoBuf;

namespace CGF.CGF.Network.FSPLite
{

    public class FSPParam
    {
        public string host;
        public int port;
        public bool useLocal = false;
        public int serverFrameInterval = 66;
        public int clientFrameRateMultiple = 2;
        public bool enableSpeedUp;
        public int defaultSpeed;
        public bool enableAutoBuffer;
        public int jetBufferSize;
        public int maxFrame;
    }

    [ProtoContract]
    public class FSPMessage
    {
        public int cmd;
        public int[] args;
        public int custom;

        public uint playerId
        {
            get { return (uint)custom; }
            set { custom = (int)value; }
        }

        public int clientFrameId
        {
            get { return custom; }
            set { custom = value; }
        }
    }

    [ProtoContract]
    public class FSPDataC2S
    {
        [ProtoMember(1)]
        public List<FSPMessage> msgs = new List<FSPMessage>();
    }

    [ProtoContract]
    public class FSPDataS2C
    {
        [ProtoMember(1)]
        public List<FSPFrame> frames = new List<FSPFrame>();
    }

    [ProtoContract]
    public class FSPFrame
    {
        public int frameId;
        public List<FSPMessage> msgs = new List<FSPMessage>();
    }
}