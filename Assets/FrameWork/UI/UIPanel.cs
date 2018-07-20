using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour {

    [System.NonSerialized]
    public int ID;

    protected object[] m_data;

    public virtual void Open(params object[] args)
    {
        this.m_data = args;
        this.OnOpen();
    }

    public virtual void Close()
    {

    }

    protected virtual void OnOpen()
    {

    }

    protected virtual void OnClose()
    {

    }

    protected virtual void OnDestroy()
    {

    }
}
