using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPage : UIPanel {

    protected void Back()
    {
        this.OnClose();
        UIManager.Instance.CloseTopPage();
    }
}
