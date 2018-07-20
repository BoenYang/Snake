using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPage : UIPage {

    public void OnLoginClick()
    {
        UIManager.Instance.OpenPage("HomePage");
    }

}
