using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class InuTime
{
    static int m_frame = -1;
    static InuFloat m_deltaTime = new InuFloat(0.01f);
    static InuFloat m_time = new InuFloat(0);

    static int m_replayFrameIndex = -1;
    static List<short> m_replayDeltaTimeList = null;
    static List<short> m_ReplayDeltaTimeListToRecord = null;

    public static void ResetFrameInfo()
    {
        m_frame = -1;
    }

    public static void Refresh(long _i = 0)
    {
        if (Time.frameCount != m_frame)
        {
            m_frame = Time.frameCount;           
            m_deltaTime.Set(Time.deltaTime);
            m_time += m_deltaTime;
        }
    }

    public static InuFloat deltaTime
    {
        get
        {
            Refresh();
            return m_deltaTime;
        }
    }
    public static InuFloat time
    {
        get
        {
            Refresh();
            return m_time;
        }
    }
   
}