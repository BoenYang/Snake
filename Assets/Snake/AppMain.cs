using CGF;
using CGF.Module;
using Snake.Services;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AppMain : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void InitDebuger()
    {
        //初始化Debuger的日志开关
        Debuger.Init(Application.persistentDataPath + "/DebugerLog/", new UnityDebugerConsole());
        Debuger.EnableLog = true;
        Debuger.EnableSave = true;
        Debuger.Log();
    }


    void Start()
    {
        InitDebuger();

        InitServiceModule();

        UIManager.Instance.OpenPage("LoginPage");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.playModeStateChanged -= OnEditorPlayModeChanged;
        UnityEditor.EditorApplication.playModeStateChanged += OnEditorPlayModeChanged;
#endif

    }


#if UNITY_EDITOR
    private void OnEditorPlayModeChanged(PlayModeStateChange mode)
    {
        if (Application.isPlaying == false)
        {
            UnityEditor.EditorApplication.playModeStateChanged -= OnEditorPlayModeChanged;
            //退出游戏逻辑
            Exit("Editor的播放模式变化！");
        }
    }
#endif

    private void Exit(string reason)
    {
        //清理模块管理器
        ModuleManager.Instance.Clean();
        //清理UI管理器
//        UIManager.Instance.Clean();
        //清理在线管理器
        OnlineManager.Instance.Clean();
//        //清楚IRL管理器
//        ILRManager.Instance.Clean();
        //清楚版本管理器
    }

    private void InitServiceModule()
    {
        UIManager.Instance.Init();

        OnlineManager.Instance.Init();
    }

    void Update()
    {
        OnlineManager.Instance.Tick();
    }

}
