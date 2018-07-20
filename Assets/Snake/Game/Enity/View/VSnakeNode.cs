using UnityEngine;

public class VSnakeNode : MonoBehaviour
{

    private EnityObject m_enity;

    private SpriteRenderer m_renderer;

    public void Create(EnityObject enity)
    {
        m_enity = enity;

        m_renderer = GetComponent<SpriteRenderer>();


        if (m_renderer)
        {

        }
    }

}
