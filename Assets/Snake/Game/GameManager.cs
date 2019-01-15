using System.Collections.Generic;
using UnityEngine;

public class GameManager : ServiceModule<GameManager> {

    private List<SnakePlayer> m_playerList = new List<SnakePlayer>();


    public bool IsRunning { get { return this.m_isRunning; } }

    private bool m_isRunning;


    public void CreateGame()
    {
        LoadGameMap();

        GameCamera.Create();

        m_isRunning = true;
    }

    public void CreatePlayer(uint playerId)
    {
        SnakePlayer player = new SnakePlayer();
        player.Create(playerId);
        m_playerList.Add(player);
    }

    private void LoadGameMap()
    {
        GameObject mapObj = Resources.Load<GameObject>("map/map_0");

        GameObject mapGo = GameObject.Instantiate(mapObj);
    }

    private int GetPlayerIndex(uint playerId)
    {
        for (int i = 0; i < m_playerList.Count; i++)
        {
            if (m_playerList[i].Id == playerId)
            {
                return i;
            }
        }
        return -1;
    }

    public SnakePlayer GetPlayer(uint playerId)
    {
        int index = GetPlayerIndex(playerId);
        if (index >= 0)
        {
            return m_playerList[index];
        }

        return null;
    }

    public void InputVKey(int vkey, float args,uint playerId)
    {
      
        if (playerId == 0)
        {
            HandleOtherVkey(vkey, args, playerId);
        }
        else
        {
            SnakePlayer player = GetPlayer(playerId);
            if (player != null)
            {
                player.InputVKey(vkey,args);
            }
            else
            {
                HandleOtherVkey(vkey, args, playerId);
            }
        }
    }

    private void HandleOtherVkey(int vkey, float arg, uint playerId)
    {
        //全局的VKey处理
        bool hasHandled = false;
        hasHandled = hasHandled || DoVKeyCreatePlayer(vkey, arg, playerId);
    }

    private bool DoVKeyCreatePlayer(int vkey, float arg, uint playerId)
    {
        if (vkey == GameVKey.CreatePlayer)
        {
            CreatePlayer(playerId);
            return true;
        }
        return false;
    }


    public void EnterFrame(int frameIndex)
    {
        for (int i = 0; i < m_playerList.Count; i++)
        {
            m_playerList[i].EnterFrame(frameIndex);
        }
    }

}
