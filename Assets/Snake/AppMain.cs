using CGF.Module;
using Snake.Services;
using UnityEngine;

public class AppMain : MonoBehaviour {

    void Awake()
    {
        InitServiceModule();
    }

 

    private void InitServiceModule()
    {
        OnlineManager.Instance.Init();
    }

}
