using CGF.Module;
using UnityEngine;

public class PVEModule : GeneralModule
{

    private PVEGame m_game;

    public PVEGame GetCurrentGame()
    {
        return m_game;
    }

    public override void Show(object arg)
    {
        base.Show(arg);
        StartGame();
    }

    private void StartGame()
    {
        m_game = new PVEGame();
        m_game.Start();

        UIManager.Instance.OpenPage("PVEGamePage");
    }
}
