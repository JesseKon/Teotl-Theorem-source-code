using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone_RedRoom : MonoBehaviour
{
    public bool OnTriggerZone {
        get { return m_OnTriggerZone; }
    }
    private bool m_OnTriggerZone = false;


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
            m_OnTriggerZone = true;
    }


    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player"))
            m_OnTriggerZone = false;
    }
}
