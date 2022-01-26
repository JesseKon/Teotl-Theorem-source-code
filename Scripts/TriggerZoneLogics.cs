using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneLogics : MonoBehaviour
{
    public bool Enter {
        get { return m_Enter; }
    }
    private bool m_Enter = false;


    public bool Exit {
        get { return m_Exit; }
    }
    private bool m_Exit = false;


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            m_Enter = true;
            m_Exit = false;
        }
    }


    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            m_Enter = false;
            m_Exit = true;
        }
    }
}
