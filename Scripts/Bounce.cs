using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float m_Speed = 2.0f;
    public float m_Intensity = 0.3f;
    public float m_Phase = 0.0f;

    private float m_HalfHeight;
    private float m_PosY;


    private void Start() {
        m_HalfHeight = m_Intensity * 0.5f;
        m_PosY = transform.position.y + 0.12f;
    }


    private void Update() {
        transform.position = new Vector3(
            transform.position.x,
            m_PosY + Mathf.Sin(m_Speed * (Time.time + m_Phase)) * m_HalfHeight + m_HalfHeight,
            transform.position.z
        );
    }

}
