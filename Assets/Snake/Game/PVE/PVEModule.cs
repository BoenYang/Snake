using System.Collections;
using System.Collections.Generic;
using FrameWork.Core;
using UnityEngine;

public class PVEModule : BusinessModule {


    public override void Show()
    {
        base.Show();
        UIManager.Instance.OpenPage("PVEGamePage");
    }
}
