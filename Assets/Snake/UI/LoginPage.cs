using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPage : UIPage {
    protected override void OnOpen()
    {
        base.OnOpen();
    }

    public void OnLoginClick()
    {
        UIManager.Instance.OpenPage("HomePage");
    }

}
