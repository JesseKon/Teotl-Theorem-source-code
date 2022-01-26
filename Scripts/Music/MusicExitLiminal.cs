using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicExitLiminal : MonoBehaviour
{
    public GameObject musicEnterLiminal;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && musicEnterLiminal.GetComponent<MusicEnterLiminal>().EnteredLiminal()) {
            GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>().FadeOutCurrent(
                2.0f
            );
        }
    }
}
