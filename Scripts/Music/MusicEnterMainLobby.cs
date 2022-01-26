using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEnterMainLobby : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>().SetMusic(GetComponent<AudioSource>());
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
