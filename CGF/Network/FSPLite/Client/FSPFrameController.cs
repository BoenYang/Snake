using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CGF.CGF.Network.FSPLite;

namespace CGF.Network.FSPLite.Client
{
    public class FSPFrameController
    {

        private FSPParam m_param;

        private int m_ClientFrameRateMutiple = 2;
        private int m_JitterBufferSize = 0;
        private int m_NewestFrameId;
        private int m_AutoBuffCnt = 0;
        private int m_AutoBuffIntercal = 15;
        private bool m_EnableAutoBuff = false;
        private bool m_EnableSpeedUp = false;
        private bool m_IsInSpeedUp = false;

        private bool m_IsBuffering = false;

        private int m_DefaultSpeed = 1;


        public void Init(FSPParam param)
        {
            SetParam(param);
        }

        public void SetParam(FSPParam param)
        {
            m_ClientFrameRateMutiple = param.clientFrameRateMultiple;
            m_JitterBufferSize = param.jetBufferSize;
            m_EnableSpeedUp = param.enableSpeedUp;
            m_DefaultSpeed = param.defaultSpeed;
            m_EnableAutoBuff = param.enableAutoBuffer;
        }

        public void AddFrame(int frameId)
        {
            m_NewestFrameId = frameId;
        }

        public int GetFrameSpeed(int curFrameId)
        {
            int speed = 0;

            int newFrameNum = m_NewestFrameId - curFrameId;

            //不在缓冲中
            if (!m_IsBuffering)
            {
                if (newFrameNum == 0)
                {
                    m_IsBuffering = true;
                    m_AutoBuffCnt = m_AutoBuffIntercal;
                }
                else
                {
                    newFrameNum -= m_DefaultSpeed;
                    int speedUpFrameNum = newFrameNum - m_JitterBufferSize;

                    //需要加速
                    if (speedUpFrameNum >= m_ClientFrameRateMutiple)
                    {
                        if (m_EnableSpeedUp)
                        {
                            speed = 2;
                            if (speedUpFrameNum > 100)
                            {
                                speed = 8;
                            }
                            else if (speedUpFrameNum > 50)
                            {
                                speed = 4;
                            }
                        }
                        else
                        {
                            speed = m_DefaultSpeed;
                        }
                    
                    }
                    else
                    {
                        speed = m_DefaultSpeed;
                        if (m_EnableAutoBuff)
                        {
                            //缓冲间隔
                            m_AutoBuffCnt--;
                            if (m_AutoBuffCnt <= 0)
                            {
                                m_AutoBuffCnt = m_AutoBuffIntercal;
                                if (speedUpFrameNum < m_ClientFrameRateMutiple - 1)
                                {
                                    speed = 0;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                int speedUpFrameNum = newFrameNum - m_JitterBufferSize;
                if (speedUpFrameNum > 0)
                {
                    m_IsBuffering = false;
                }
            }

            m_IsInSpeedUp = speed > m_DefaultSpeed;
            return speed;
        }
    }
}
