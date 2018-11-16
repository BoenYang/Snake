using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : UIPanel {

    public override void Close()
    {
        UIManager.Instance.CloseWindow(ID);
    }
}
