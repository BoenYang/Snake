
public class FSPPlayer
{
    public uint uid;

    private FSPSession m_session = null;

    private int m_state;

    public FSPSession Session
    {
        set
        {
            if (m_session != null)
            {
                m_session.RemoveReciveListener(this.onRecive);
            }
            m_session = value;
            m_session.AddRecviveListener(this.onRecive);
        }
    }

    public FSPPlayer()
    {
        m_session = null;
    }

    private void onRecive(byte[] data)
    {

    }

}
