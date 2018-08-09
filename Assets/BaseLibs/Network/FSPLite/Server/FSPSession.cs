
using System.Collections.Generic;
using System.IO;
using ProtoBuf;
using UnityEngine;

public class FSPSession
{
    private TCPSocket m_socket;

    private Queue<byte[]> m_productQueue = new Queue<byte[]>();

    private Queue<byte[]> m_consumeQueue = new Queue<byte[]>();

    public delegate void OnReciveData(byte[] data);

    private event OnReciveData m_OnReciveData;

    public FSPSession(TCPSocket socket)
    {
        m_socket = socket;
    }

    public void OnRecive(byte[] data)
    {
        MemoryStream ms = new MemoryStream(data);
        BinaryReader br = new BinaryReader(ms);
        uint cmd = br.ReadUInt32();

        Debug.Log("[FSPSession] Read Cmd " + cmd);
        if (cmd == Command.EnterRoom)
        {
            HandleEnterRoom(br.ReadBytes(data.Length - 4));
        }
        else if (cmd == Command.Ready)
        {

        }
        else
        {
            m_productQueue.Enqueue(data);
        }

    }

    public void AddRecviveListener(OnReciveData listener)
    {
        m_OnReciveData += listener;
    }

    public void RemoveReciveListener(OnReciveData listener)
    {
        m_OnReciveData -= listener;
    }

    public void HandleRecive()
    {
        SwitchQueue();
        while (m_consumeQueue.Count > 0)
        {
            byte[] data = m_consumeQueue.Dequeue();
            if (m_OnReciveData != null)
            {
                m_OnReciveData(data);
            }
        }
    }

    private void SwitchQueue()
    {
        Queue<byte[]> tempQuene = m_productQueue;
        m_productQueue = m_consumeQueue;
        m_consumeQueue = tempQuene;
    }

    private void HandleEnterRoom(byte[] data)
    {
        EnterRoomRequest request = Serializer.Deserialize<EnterRoomRequest>(new MemoryStream(data));
    }

    private void HandleReady()
    {

    }

    private void HandleExit()
    {

    }

}
