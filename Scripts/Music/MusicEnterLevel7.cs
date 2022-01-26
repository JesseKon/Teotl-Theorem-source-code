using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEnterLevel7 : MonoBehaviour
{
    public GameObject m_Door1;
    public GameObject m_Door2;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {

            // Force these doors closed
            m_Door1.GetComponent<ElectricDoor>().ForceCloseDoor();
            m_Door2.GetComponent<ElectricDoor>().ForceCloseDoor();

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
