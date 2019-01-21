using System.Collections.Generic;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using CGF.Network.FSPLite.Client;

namespace CGF.CGF.Network.FSPLite.Client
{
    public class FSPManager
    {
        private FSPClient m_client;

        private Dictionary<int, FSPFrame> m_frameBuffer;

        private int m_currentFrameIndex;

        private int m_lockedFrameIndex;

        private FSPParam m_param;

        private FSPFrameController m_frameController;

        private FSPFrame m_nextLocalFrame;

        public void Start(FSPParam param)
        {
            m_param = param;

            if (param.useLocal)
            {
                m_lockedFrameIndex = param.maxFrame;
            }
            else
            {
                m_client = new FSPClient();
                m_client.Init(0);
                m_client.SetFSPListener(OnReceiveFrame);
                m_lockedFrameIndex = param.clientFrameRateMultiple;
            }

            m_frameController = new FSPFrameController();
            m_frameController.Init(m_param);

            m_currentFrameIndex = 0;
            m_frameBuffer = new Dictionary<int, FSPFrame>();
        }

        private void OnReceiveFrame(FSPFrame frame)
        {
            if (frame.frameId <= 0)
            {
                ExcuteFrame(frame.frameId,frame);
                return;
            }

            frame.frameId = frame.frameId * m_param.clientFrameRateMultiple;
            m_lockedFrameIndex = frame.frameId + m_param.clientFrameRateMultiple - 1;
            m_frameBuffer.Add(frame.frameId,frame);

            m_frameController.AddFrame(frame.frameId);
            
        }

        private void ExcuteFrame(int frameId,FSPFrame frame)
        {

        }

        public void SendFSP(int cmd, params int[] args)
        {
            if (m_param.useLocal)
            {
                SendFSPLocal(cmd,args);
            }
            else
            {
                m_client.SendFSP(m_currentFrameIndex, cmd, args);
            }
        }

        public void SendFSPLocal(int cmd, int[] args)
        {

            if (m_nextLocalFrame == null || m_nextLocalFrame.frameId != m_currentFrameIndex + 1)
            {
                m_nextLocalFrame = new FSPFrame();
                m_nextLocalFrame.frameId = m_currentFrameIndex + 1;
                m_frameBuffer.Add(m_nextLocalFrame.frameId, m_nextLocalFrame);
            }

            FSPMessage msg = new FSPMessage();
            msg.cmd = cmd;
            msg.args = args;
            msg.playerId = 0;
            m_nextLocalFrame.msgs.Add(msg);
        }

        public void Tick()
        {
            m_client.Tick();

            if (m_param.useLocal)
            {
                if (m_currentFrameIndex < m_lockedFrameIndex)
                {
                    m_currentFrameIndex++;
                    FSPFrame frame = m_frameBuffer[m_currentFrameIndex];
                    ExcuteFrame(m_currentFrameIndex, frame);
                }
            }
            else
            {
                m_client.Tick();

                int spped = m_frameController.GetFrameSpeed(m_currentFrameIndex);

                while (spped > 0)
                {
                    if (m_currentFrameIndex < m_lockedFrameIndex)
                    {
                        m_currentFrameIndex++;
                        FSPFrame frame = m_frameBuffer[m_currentFrameIndex];
                        ExcuteFrame(m_currentFrameIndex, frame);
                    }

                    spped--;
                }
            }
        }
    }
}