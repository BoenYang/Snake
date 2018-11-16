



using System;
using UnityEngine;

public class MonoHelper : MonoSingleton<MonoHelper>
{
    public delegate void MonoUpdateEvent();

    private event MonoUpdateEvent UpdateEvent;

    private event MonoUpdateEvent FixedUpdateEvent;

    public void AddUpdateEvent(MonoUpdateEvent listener)
    {
        UpdateEvent += listener;
    }

    public void RemoveUpdateEvent(MonoUpdateEvent listener)
    {
        UpdateEvent += listener;
    }

    public void AddFixedUpdateEvent(MonoUpdateEvent listener)
    {
        FixedUpdateEvent += listener;
    }

    public void RemoveFixedUpdateEvent(MonoUpdateEvent listener)
    {
        FixedUpdateEvent += listener;
    }

    void Update()
    {
        if (UpdateEvent != null)
        {
            try
            {
                UpdateEvent();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    void FixedUpdate()
    {
        if (FixedUpdateEvent != null)
        {
            try
            {
                FixedUpdateEvent();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}
