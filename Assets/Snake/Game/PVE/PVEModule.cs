using System.Collections;
using System.Collections.Generic;
using FrameWork.Core;
using UnityEngine;

public class PVEModule : BusinessModule
{

    private PVEGame m_game;

    public PVEGame GetCurrentGame()
    {
        return m_game;
    }

    public override void Show()
    {
        base.Show();
        StartGame();
    }

    private void StartGame()
    {
        m_game = new PVEGame();
        m_game.Start();

        UIManager.Instance.OpenPage("PVEGamePage");
    }
}
