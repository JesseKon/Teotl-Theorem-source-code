using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEnterFakeExit: MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            StartCoroutine(Music());
        }
    }


    private IEnumerator Music() {
        GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>().FadeOutCurrent(1.0f);
        yield return new WaitForSeconds(1.0f);
        GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>().SetMusic(GetComponent<AudioSource>());
        GetComponent<BoxCollider>().enabled = false;
    }
}
