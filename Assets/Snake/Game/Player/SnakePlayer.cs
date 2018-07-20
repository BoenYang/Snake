using UnityEngine;
using UnityEngine.Purchasing;

public class SnakePlayer
{
    private SnakeHead m_head;

    private SnakeTail m_tail;

    private GameObject m_container;


    public void Create()
    {
        m_container = new GameObject("Player");

        m_head = new SnakeHead();
        m_head.Create(0,m_container.transform);

        m_tail = new SnakeTail();
        m_tail.Create(0,m_container.transform);

        m_head.SetNext(m_tail);
        m_tail.SetPre(m_head);

        AddNode(50);
    }

    private void AddNode(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SnakeNode node = new SnakeNode();
            node.Create((i+1),m_container.transform);

            m_tail.Prev.SetNext(node);
            node.SetNext(m_tail);
            m_tail.SetPre(node);
        }
    }


    public void MoveTo(Vector3 pos)
    {
        m_head.MoveTo(pos);
    }



}

