using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoomVolume : MonoBehaviour
{
    private FogSystem m_FogSystem;

    private void Start() {
        m_FogSystem = GameObject.Find("FogSystem").GetComponent<FogSystem>();
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            StartCoroutine(m_FogSystem.EnterDarkRoom());
        }
    }


    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            StartCoroutine(m_FogSystem.ExitDarkRoom());
        }
    }
}
