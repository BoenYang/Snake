using CGF.Core;
using UnityEngine;

public class AppMain : MonoBehaviour {

    void Awake()
    {
        InitServiceModule();
        InitBuniessModule();
    }

    private void InitBuniessModule()
    {
        ModuleManager.Instance.CreateModule("PVEModule");
    }

    private void InitServiceModule()
    {
        UIManager.Instance.Init();
        UIManager.Instance.OpenPage("LoginPage");
    }

}
