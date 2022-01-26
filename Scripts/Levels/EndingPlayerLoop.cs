using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPlayerLoop : MonoBehaviour
{
    private GameObject m_Player;

    private GameObject m_EndPoint1;
    private GameObject m_EndPoint2;
    private GameObject m_EndPoint3;
    private GameObject m_EndPoint4;


    private void Start() {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_EndPoint1 = transform.GetChild(0).gameObject;
        m_EndPoint2 = transform.GetChild(1).gameObject;
        m_EndPoint3 = transform.GetChild(2).gameObject;
        m_EndPoint4 = transform.GetChild(3).gameObject;
    }


    private void FixedUpdate() {
        if (m_EndPoint1.GetComponent<TriggerZoneLogics>().Enter) {
            m_Player.transform.position = new Vector3(
                m_Player.transform.position.x - 60.0f,
                m_Player.transform.position.y,
                m_Player.transform.position.z
            );
        }

        if (m_EndPoint2.GetComponent<TriggerZoneLogics>().Enter) {
            m_Player.transform.position = new Vector3(
                m_Player.transform.position.x + 60.0f,
                m_Player.transform.position.y,
                m_Player.transform.position.z
            );
        }

        if (m_EndPoint3.GetComponent<TriggerZoneLogics>().Enter) {
            m_Player.transform.position = new Vector3(
                m_Player.transform.position.x - 60.0f,
                m_Player.transform.position.y,
                m_Player.transform.position.z
            );
        }

        if (m_EndPoint4.GetComponent<TriggerZoneLogics>().Enter) {
            m_Player.transform.position = new Vector3(
                m_Player.transform.position.x + 60.0f,
                m_Player.transform.position.y,
                m_Player.transform.position.z
            );
        }
    }
}
