using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liminal3_Portal : MonoBehaviour
{
    public GameObject m_PortalToDestroy;
    public GameObject m_DoorToDestroy;


    private void Update() {
        if (GetComponent<Portal>().EnteredPortal()) {
            m_PortalToDestroy.SetActive(false);
            m_DoorToDestroy.SetActive(false);
        }
    }
}
