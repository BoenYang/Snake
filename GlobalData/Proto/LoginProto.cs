using ProtoBuf;

namespace GlobalData.Proto
{
    [ProtoContract]
    public class LoginReq
    {
        [ProtoMember(1)]
        public string account;

        [ProtoMember(2)]
        public string password;
    }

    [ProtoContract]
    public class LoginRsp
    {
        [ProtoMember(1)]
        public string name;
    }
}