
using System.IO;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class EnterRoomRequest
{
    public uint cmd = Command.EnterRoom;

    [ProtoMember(1)]
    public uint uid;

    public byte[] Serialize()
    {
        MemoryStream ms = new MemoryStream();
        BinaryWriter wb = new BinaryWriter(ms);
        wb.Write(cmd);
        Serializer.Serialize(ms, this);
        return ms.ToArray();
    }
}

[ProtoContract]
public class EnterRoomResponse
{
    [ProtoMember(1)]
    public int code;
}

[ProtoContract]
public class FSPFrame
{

}

[ProtoContract]
public class FSPVkey
{
    public int vkey;

    public int[] args;


}