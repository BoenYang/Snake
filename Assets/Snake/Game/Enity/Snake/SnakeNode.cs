using System.Collections;
using UnityEngine;

public class SnakeNode : EnityObject
{

    protected SnakeNode m_next;

    protected int m_index;

    protected Vector3 m_pos;

    protected Vector3 m_angles;

    public SnakeNode Next{ get { return m_next; } }

    public void SetNext(SnakeNode node)
    {
        this.m_next = node;
        m_next.MoveTo(m_pos);
    }

    public void Create(int index,Transform container)
    {
        m_index = index;

        if (IsKeyNode())
        {
            GameObject go = null;
            GameObject prefab = null;
            if (this is SnakeHead)
            {
                prefab = Resources.Load<GameObject>("snake/0/head");
            }else if (this is SnakeTail)
            {
                prefab = Resources.Load<GameObject>("snake/0/tail");
            }
            else
            {
                prefab = Resources.Load<GameObject>("snake/0/node");
            }

            go = GameObject.Instantiate(prefab);

            VSnakeNode vNode = go.AddComponent<VSnakeNode>();
            vNode.Create(this);
            vNode.transform.SetParent(container);
        }
    }

    private bool IsKeyNode()
    {
        return m_index % 2 == 0;
    }

    public void MoveTo(Vector3 pos)
    {
        Vector3 oldPos = m_pos;
        m_pos = pos;

        Vector3 dir = m_pos - oldPos;

        m_angles.z = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg - 90;

        if (m_next != null)
        {
            m_next.MoveTo(oldPos);
        }
    }

}
