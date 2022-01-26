using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEnterLiminal : MonoBehaviour
{
    private bool m_EnteredLiminal = false;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !m_EnteredLiminal) {
            GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>().SetMusic(GetComponent<AudioSource>());
            m_EnteredLiminal = true;
        }
    }

    public bool EnteredLiminal() {
        return m_EnteredLiminal;
    }
}
