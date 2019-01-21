using System.Collections.Generic;
using CGF.Network.General.Server;

namespace CGF.Network.FSPLite.Server
{
    public class FSPGame
    {
        public uint Id
        {
            get { return m_gameId; }
        }

        private uint m_gameId;

        public List<FSPPlayer> m_playerList = new List<FSPPlayer>();

        private FSPFrame m_lockedFrame = new FSPFrame();

        public void Create(uint gameId)
        {
            m_gameId = gameId;
        }

        public void Release()
        {
            m_playerList.Clear();
        }

        public FSPPlayer AddPlayer(uint uid, ISession session)
        {
            FSPPlayer player = new FSPPlayer();
            player.Create(uid,session, OnReveivePlayerMsg);
            m_playerList.Add(player);
            return player;
        }


        private void OnReveivePlayerMsg(FSPPlayer player,FSPMessage msg)
        {
            switch (msg.cmd)
            {
                case 0:
                    break;
                default:
                    AddCmdToCurrentFrame(player.uid,msg);
                    break;
            }
        }

        private void AddCmdToCurrentFrame(uint playerId,FSPMessage msg)
        {
            msg.playerId = playerId;
            m_lockedFrame.msgs.Add(msg);
        }

        public void EnterFrame()
        {
            if (m_lockedFrame.frameId != 0)
            {
                for (int i = 0; i < m_playerList.Count; i++)
                {
                    FSPPlayer player = m_playerList[i];
                    player.SendToClient(m_lockedFrame);
                }
            }
        }
    }
}