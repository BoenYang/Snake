using System;
using System.Collections.Generic;
using CGF.Network.General.Server;

namespace CGF.Network.FSPLite.Server
{
    public class FSPManager
    {
        private FSPGateway m_gateWay;

        private Dictionary<uint,FSPGame> m_gameMap = null;

        private FSPParam m_param = new FSPParam();

        private long m_lastTicks;

        public void Init(int port)
        {
            Debuger.Log();

            m_gateWay = new FSPGateway();
            m_gateWay.Init(port, null);

            m_param.port = m_gateWay.Port;
            m_param.host = m_gateWay.Host;
           
            m_gameMap = new Dictionary<uint, FSPGame>();
        }

        public FSPGame CreateGame(uint gameId) { 
            FSPGame game = new FSPGame();
            game.Create(gameId);
            m_gameMap.Add(gameId,game);
            return game;
        }

        public FSPPlayer AddPlayer(uint gameId,uint playerId)
        {
            FSPPlayer player = null;
            FSPGame game = m_gameMap[gameId];
            if (game != null)
            {
                ISession session = m_gateWay.CreateSession();
                player = game.AddPlayer(playerId, session);
            }

            return player;
        }


        public void ReleaseGame(uint gameId)
        {
            FSPGame game = m_gameMap[gameId];
            if (game != null)
            {
                game.Release();
                m_gameMap.Remove(gameId);
            }
        }

        public void Clean()
        {
            if (m_gateWay != null)
            {
                m_gateWay.Clean();
                m_gateWay = null;
            }
        }
    

        public void Tick()
        {
            m_gateWay.Tick();

            uint current = (uint) TimeUtils.GetTotalMillisecondsSince1970();

            long nowticks = DateTime.Now.Ticks;
            long interval = nowticks - m_lastTicks;
            long frameIntervalTicks = m_param.serverFrameInterval * 10000;
            if (interval > frameIntervalTicks)
            {
                m_lastTicks = nowticks - (nowticks % frameIntervalTicks);
                foreach (FSPGame game in m_gameMap.Values)
                {
                    game.EnterFrame();
                }
            }
        }
    }
}