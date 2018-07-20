using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageWindowData
{
    public Action<int> btnCallback;
}

public class MessageWindow : UIWindow
{

    private MessageWindowData m_winData;

    protected override void OnOpen()
    {
        base.OnOpen();
        m_winData = null;
        if (this.m_data != null && this.m_data.Length > 0 && this.m_data[0] is MessageWindowData)
        {
            m_winData = this.m_data[0] as MessageWindowData;
        }
    }

    public void OnBtnClick(int index)
    {
        Close();
        if (m_winData != null && m_winData.btnCallback != null)
        {
            m_winData.btnCallback(index);
        }
    }

}
