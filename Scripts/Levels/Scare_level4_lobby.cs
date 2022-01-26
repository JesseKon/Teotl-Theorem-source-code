using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scare_level4_lobby : MonoBehaviour
{
    public GameObject m_PickableCube1;
    public GameObject m_PickableCube2;
    public GameObject m_PickableCube3;

    private void Start() {
        m_PickableCube1.SetActive(false);
        m_PickableCube2.SetActive(false);
        m_PickableCube3.SetActive(false);
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("Scare_level4_lobby trigger entered.");
            m_PickableCube1.SetActive(true);
            m_PickableCube2.SetActive(true);
            m_PickableCube3.SetActive(true);

            // TODO: play scary sound
        }
    }
}
