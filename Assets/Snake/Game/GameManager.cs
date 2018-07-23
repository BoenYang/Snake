using System.Collections;
using System.Collections.Generic;
using FrameWork.Core;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class GameManager : ServiceModule<GameManager> {

    private List<SnakePlayer> m_playerList = new List<SnakePlayer>();

    public void CreateGame()
    {
        LoadGameMap();

        CreatePlayer();
    }

    public void CreatePlayer()
    {
        SnakePlayer player = new SnakePlayer();
        player.Create();

        m_playerList.Add(player);
    }

    private void LoadGameMap()
    {
        GameObject mapObj = Resources.Load<GameObject>("map/map_0");

        GameObject mapGo = GameObject.Instantiate(mapObj);
    }

}
