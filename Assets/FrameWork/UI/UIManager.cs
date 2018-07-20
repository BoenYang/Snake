using System.Collections;
using System.Collections.Generic;
using FrameWork.Core;
using UnityEngine;

public class UIManager : ServiceModule<UIManager> {

    protected class PageTrack
    {
        public LinkedList<UIPanel> WindowsStack;
        public UIPanel Page;

        public PageTrack(UIPanel page)
        {
            this.Page = page;
            this.WindowsStack = new LinkedList<UIPanel>();
        }
    }

    private PageTrack m_currentPage;

    private Transform m_root;

    private LinkedList<PageTrack> m_pageStack;

    private int m_idCounter = 0;

    private string m_resPath = "ui/";

    public void Init()
    {
        GameObject go = GameObject.Find("UIRoot");
        if (go)
        {
            m_root = go.transform;
        }
        m_currentPage = null;
        m_idCounter = 0;
        m_pageStack = new LinkedList<PageTrack>();
    }

    #region Page

    public UIPanel OpenPage(string name,params object[] args)
    {
        GameObject uiPrefab = LoadUI(name);

        if (uiPrefab == null)
        {
            return null;
        }

        UIPanel panel = CreateUI(uiPrefab);
        panel.Open(args);
        m_currentPage = new PageTrack(panel);
        m_pageStack.AddLast(this.m_currentPage);
        return panel;
    }

    public void CloseTopPage()
    {
        if (m_pageStack.Count > 1)
        {
            PageTrack topPageTrack = m_pageStack.Last.Value;
            RemovePage(topPageTrack);
        }
    }

    public void ClosePage(int id)
    {
        PageTrack pageTrack = GetPage(id);
        RemovePage(pageTrack);
    }

    protected void RemovePage(PageTrack pageTrack)
    {
        if (pageTrack != null)
        {
            m_pageStack.Remove(pageTrack);
            GameObject.Destroy(pageTrack.Page.gameObject);
        }
    }

    protected PageTrack GetPage(int id)
    {
        foreach (PageTrack pageTrack in m_pageStack)
        {
            if (pageTrack.Page.ID == id)
            {
                return pageTrack;
            }
        }
        return null;
    }

    #endregion

    #region Window

    public UIPanel OpenWindow(string name, params object[] args)
    {
        GameObject uiPrefab = LoadUI(name);
        if (uiPrefab == null)
        {
            return null;
        }
        UIPanel panel = CreateUI(uiPrefab);
        panel.Open(args);
        m_currentPage.WindowsStack.AddLast(panel);
        return panel;
    }

    public void CloseWindow(int id)
    {
        UIPanel window = this.GetWindow(id);
        if (window)
        {
            RemoveWindow(window);
        }
    }

    protected void RemoveWindow(UIPanel panel)
    {
        m_currentPage.WindowsStack.Remove(panel);
        GameObject.Destroy(panel.gameObject);
    }

    protected UIPanel GetWindow(int id)
    {
        foreach (UIPanel win in m_currentPage.WindowsStack)
        {
            if (win.ID == id)
            {
                return win;
            }
        }
        return null;
    }


    #endregion


    protected GameObject LoadUI(string name)
    {
        GameObject go = Resources.Load<GameObject>(this.m_resPath + name);
        return go;
    }

    protected UIPanel CreateUI(GameObject uiPrefab)
    {
        GameObject uiGo = GameObject.Instantiate(uiPrefab);
        uiGo.transform.SetParent(m_root);
        uiGo.transform.localScale = Vector3.one;
        uiGo.transform.localPosition = Vector3.zero;

        UIPanel panel = uiGo.GetComponent<UIPanel>();
        panel.ID = GetID();

        return panel;
    }

    protected int GetID()
    {
        return ++m_idCounter;
    }

}
