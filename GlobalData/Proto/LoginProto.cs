using ProtoBuf;

namespace GlobalData.Proto
{
    [ProtoContract]
    public class LoginReq
    {
        [ProtoMember(1)]
        public uint id;

        [ProtoMember(1)]
        public string name;
    }

    public class LoginRsp
    {

    }
}