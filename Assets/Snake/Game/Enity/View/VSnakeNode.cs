using UnityEngine;

public class VSnakeNode : MonoBehaviour
{

    private SnakeNode m_entity;

    private SpriteRenderer m_renderer;

    protected Vector3 m_entityPosition;

    public void Create(EnityObject enity)
    {
        m_entity = enity as SnakeNode;
        m_renderer = GetComponent<SpriteRenderer>();

        if (m_renderer)
        {
            m_renderer.sortingOrder = 10000 - m_entity.Index;
        }
    }


    void Update()
    {
        if (m_entity != null && m_renderer != null)
        {

            m_entityPosition = m_entity.Position();
            Vector3 pos = new Vector3(m_entityPosition.x - 0.5f, m_entityPosition.y,0);
            this.transform.localEulerAngles = m_entity.Angle;
            this.transform.localPosition = pos;
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
