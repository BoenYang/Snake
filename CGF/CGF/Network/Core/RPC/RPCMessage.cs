using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace CGF.Network.Core.RPC
{
    [ProtoContract]
    public class RPCMessage
    {
        [ProtoMember(1)]
        public string name;

        [ProtoMember(2)]
        public List<RPCRawArg> raw_args = new List<RPCRawArg>();

        public object[] args
        {
            get
            {
                List<object> list = new List<object>();
                for (int i = 0; i < raw_args.Count; i++)
                {
                    list.Add(raw_args[i].value);
                }

                return list.ToArray();
            }
            set
            {
                raw_args = new List<RPCRawArg>();
                object[] list = value;
                for (int i = 0; i < list.Length; i++)
                {
                    RPCRawArg arg = new RPCRawArg();
                    arg.value = list[i];
                    raw_args.Add(arg);
                }
            }
        }
    }

    [ProtoContract]
    public class RPCRawArg
    {
        [ProtoMember(1)]
        public RPCArgType type;
        [ProtoMember(2)]
        public byte[] raw_value;

        public object value
        {
            get
            {
                if (raw_value == null || raw_value.Length == 0)
                {
                    return null;
                }
                NetBufferReader reader = new NetBufferReader();
                switch (type)
                {
                    case RPCArgType.Int: return reader.ReadInt();
                    case RPCArgType.UInt: return reader.ReadUInt();
                    case RPCArgType.Long: return reader.ReadLong();
                    case RPCArgType.ULong: return reader.ReadULong();
                    case RPCArgType.Short: return reader.ReadShort();
                    case RPCArgType.UShort: return reader.ReadUShort();
                    case RPCArgType.Double: return reader.ReadDouble();
                    case RPCArgType.Float: return reader.ReadFloat();
                    case RPCArgType.Bool: return reader.ReadBool();
                    case RPCArgType.Byte: return reader.ReadByte();
                    case RPCArgType.ByteArray: return raw_value;
                    case RPCArgType.String: return Encoding.UTF8.GetString(raw_value);
                    case RPCArgType.PBObject: return raw_value;
                    default: return raw_value;
                }
            }
            set
            {
                object v = value;
                if (v is int)
                {
                    type = RPCArgType.Int;
                    raw_value = BitConverter.GetBytes((int) v);
                    NetBuffer.ReverseOrder(raw_value);
                }
                else if (v is uint)
                {
                    type = RPCArgType.UInt;
                    raw_value = BitConverter.GetBytes((uint)v);
                    NetBuffer.ReverseOrder(raw_value);
                }
                else if (v is short)
                {
                    type = RPCArgType.Short;
                    raw_value = BitConverter.GetBytes((short)v);
                    NetBuffer.ReverseOrder(raw_value);
                }
                else if (v is ushort)
                {
                    type = RPCArgType.UShort;
                    raw_value = BitConverter.GetBytes((ushort)v);
                    NetBuffer.ReverseOrder(raw_value);
                }
                else if (v is long)
                {
                    type = RPCArgType.Long;
                    raw_value = BitConverter.GetBytes((long)v);
                    NetBuffer.ReverseOrder(raw_value);
                }
                else if (v is ulong)
                {
                    type = RPCArgType.ULong;
                    raw_value = BitConverter.GetBytes((ulong)v);
                    NetBuffer.ReverseOrder(raw_value);
                }
                else if (v is double)
                {
                    type = RPCArgType.Double;
                    raw_value = BitConverter.GetBytes((double)v);
                    NetBuffer.ReverseOrder(raw_value);
                }
                else if (v is float)
                {
                    type = RPCArgType.Float;
                    raw_value = BitConverter.GetBytes((float)v);
                    NetBuffer.ReverseOrder(raw_value);
                }
                else if (v is byte)
                {
                    type = RPCArgType.Byte;
                    raw_value = new[] { (byte)v };
                }
                else if (v is bool)
                {
                    type = RPCArgType.Bool;
                    raw_value = new[] { (bool)v ? (byte)1 : (byte)0 };
                }
                else if (v is byte[])
                {
                    type = RPCArgType.ByteArray;
                    raw_value = new byte[((byte[])v).Length];
                    Buffer.BlockCopy((byte[])v, 0, raw_value, 0, raw_value.Length);
                }else if (v is string)
                {
                    type = RPCArgType.String;
                    raw_value = Encoding.UTF8.GetBytes((string)v);
                }
                else
                {

                }
            }
        }
    }

    public enum RPCArgType
    {
        Unkown = 0,
        Byte   = 1,
        Bool   = 2,
        Short  = 3,
        UShort = 4,
        Int    = 5,
        UInt   = 6,
        Long   = 7,
        ULong  = 8,
        Float  = 9,
        Double = 10,
        String = 11,
        ByteArray = 31,
        PBObject = 32
    }
}