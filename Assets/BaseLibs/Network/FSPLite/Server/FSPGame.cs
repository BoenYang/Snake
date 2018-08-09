
using System.Collections.Generic;

public class FSPGame
{
    private Dictionary<uint,FSPPlayer> m_playerList = new Dictionary<uint, FSPPlayer>();

    public FSPGame()
    {

    }


    public void AddPlayer(uint uid, FSPSession session)
    {
        FSPPlayer player = null;
        if (m_playerList.ContainsKey(uid))
        {
            player = m_playerList[uid];
        }
        else
        {
            player = new FSPPlayer();
            player.uid = uid;
            m_playerList.Add(uid,player);
        }
        player.Session = session;
    }


    public void EnterFrame()
    {

    }

}

