using UnityEngine;
using UnityEngine.Purchasing;

public class SnakePlayer
{
    private SnakeHead m_head;

    private SnakeTail m_tail;

    private GameObject m_container;

    public uint Id;

    public void Create(uint id)
    {

        Id = id;

        m_container = new GameObject("Player" + Id);

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

    public void EnterFrame(int frameIndex)
    {
        HandleMove();
    }


    private Vector3 m_MoveDirection = new Vector3();
    private Vector3 m_InputMoveDirection = new Vector3();
    private float m_MoveSpeed = 1;
    public Vector3 MoveDirection { get { return m_MoveDirection; } }


    public void InputVKey(int vkey, float args)
    {
        switch (vkey)
        {
            case GameVKey.MoveX:
                m_InputMoveDirection.x = args;
                break;
            case GameVKey.MoveY:
                m_InputMoveDirection.y = args;
                break;
            case GameVKey.SpeedUp:
                m_MoveSpeed = args;
                break;
        }
    }

    private void HandleMove()
    {

        for (int i = 0; i < m_MoveSpeed; i++)
        {
            if (m_InputMoveDirection.magnitude > 0)
            {
                m_MoveDirection = m_InputMoveDirection;
            }

            if (m_MoveDirection.magnitude > 0)
            {
                Vector3 curPos = m_head.Position() + m_MoveDirection.normalized * 2;
                MoveTo(curPos);
            }
        }
    }

}

