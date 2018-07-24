using UnityEngine;

public class PVEGame
{

    private uint m_mainPlayerId = 1;


    private int m_frameIndex;

    public void Start()
    {
        m_frameIndex = 0;

        GameManager.Instance.CreateGame();
        GameInput.Create();
        GameInput.OnVkey += OnVKey;

        GameCamera.FocusPlayerId = m_mainPlayerId;
        MonoHelper.instance.AddFixedUpdateEvent(this.FixedUpdate);
    }

    private void OnVKey(int vkey, float args)
    {
        GameManager.Instance.InputVKey(vkey,args,m_mainPlayerId);
    }

    public void CreatePlayer()
    {
        GameManager.Instance.InputVKey(GameVKey.CreatePlayer,0,m_mainPlayerId);
    }

    public void FixedUpdate()
    {
        m_frameIndex ++;
        GameManager.Instance.EnterFrame(m_frameIndex);
    }
}
