
public class SnakeTail : SnakeNode
{
    private SnakeNode m_prev;

    public SnakeNode Prev { get { return m_prev; } }

    public void SetPre(SnakeNode node)
    {
        m_prev = node;
    }

}
