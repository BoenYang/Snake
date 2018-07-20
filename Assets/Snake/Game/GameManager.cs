using System.Collections;
using System.Collections.Generic;
using FrameWork.Core;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class GameManager : ServiceModule<GameManager> {

    private List<SnakePlayer> m_playerList = new List<SnakePlayer>();

    public void CreateGame()
    {

    }

    public void CreatePlayer()
    {
        SnakePlayer player = new SnakePlayer();
        player.Create();

        m_playerList.Add(player);
    }

}
