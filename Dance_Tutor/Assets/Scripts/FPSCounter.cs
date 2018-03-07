using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public Text uiElement;

    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;

    float max = 0;
    float min = 10000;

    void Update()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            //This code will break if you set your m_refreshTime to 0, which makes no sense.
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;

            max = Mathf.Max(max, m_lastFramerate);
            min = Mathf.Min(min, m_lastFramerate);

            uiElement.text = Mathf.RoundToInt(m_lastFramerate) + " FPS, " + Mathf.RoundToInt(min) + " Min Fps, " + Mathf.RoundToInt(max) + " Max Fps";
        }
    }

}
