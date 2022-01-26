using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : MonoBehaviour
{
    public Vector3 m_PositionWhenClosed;
    public Vector3 m_PositionWhenOpen;

    public float m_PistonTimeOpen = 2.0f;
    public float m_PistonTimeClosed = 2.0f;

    public float m_PistonOpeningTime = 1.0f;
    public float m_PistonClosingTime = 1.0f;

    private float m_Timer;

    private bool m_PistonOpen = false;
    private bool m_PistonOpening = false;
    private bool m_PistonClosed = true;
    private bool m_PistonClosing = false;




    void Update()
    {
        m_Timer += Time.deltaTime;

        if (!m_PistonOpening || !m_PistonClosing) {
            if (m_PistonOpen && m_Timer > m_PistonTimeOpen) {
                m_PistonClosing = true;
                m_Timer = 0.0f;
            }
                
            if (m_PistonClosed && m_Timer > m_PistonTimeClosed) {
                m_PistonOpening = true;
                m_Timer = 0.0f;
            }
        }

        if (m_PistonOpening) {
            m_PistonClosed = false;
            transform.position = Vector3.Lerp(m_PositionWhenClosed, m_PositionWhenOpen, m_Timer / m_PistonOpeningTime);

            if (m_Timer > m_PistonOpeningTime) {
                m_PistonOpen = true;
                m_PistonOpening = false;
                m_Timer = 0.0f;
            }
        }

        if (m_PistonClosing) {
            m_PistonOpen = false;
            transform.position = Vector3.Lerp(m_PositionWhenOpen, m_PositionWhenClosed, m_Timer / m_PistonClosingTime);

            if (m_Timer > m_PistonClosingTime) {
                m_PistonClosed = true;
                m_PistonClosing = false;
                m_Timer = 0.0f;
            }
        }
    }
}
