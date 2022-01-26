using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicExitMainLobby : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>().FadeOutCurrent(2.0f);
            Destroy(gameObject, 5.0f);
        }
    }
}
