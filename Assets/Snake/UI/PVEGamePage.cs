using System.Collections;
using UnityEngine.UI;

public class PVEGamePage : UIPage
{

    private PVEGame m_game;

    public Button ReadyBtn;

    protected override void OnOpen()
    {
        base.OnOpen();

        PVEModule module = ModuleManager.Instance.GetModule("PVEModule") as PVEModule;
        m_game = module.GetCurrentGame();
    }

    public void OnReadyClick()
    {
        ReadyBtn.gameObject.SetActive(false);
       m_game.CreatePlayer();
    }
}
