using UnityEngine;

public class VSnakeNode : MonoBehaviour
{

    private EnityObject m_entity;

    private SpriteRenderer m_renderer;

    protected Vector3 m_entityPosition;

    public void Create(EnityObject enity)
    {
        m_entity = enity;
        m_renderer = GetComponent<SpriteRenderer>();

        if (m_renderer)
        {
            m_renderer.sortingOrder = 10000;
        }
    }


    void Update()
    {
        if (m_entity != null && m_renderer != null)
        {

            m_entityPosition = m_entity.Position();
            Vector3 pos = new Vector3(m_entityPosition.x - 0.5f, m_entityPosition.y,0);
            this.transform.localPosition = pos;
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
