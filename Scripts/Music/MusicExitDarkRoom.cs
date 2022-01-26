using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicExitDarkRoom : MonoBehaviour
{
    public GameObject enterDarkRoom;

    public bool ExitedDarkRoom {
        get { return m_ExitedDarkRoom; }
        set { m_ExitedDarkRoom = value; }
    }
    private bool m_ExitedDarkRoom = false;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            m_ExitedDarkRoom = true;

            if (enterDarkRoom.GetComponent<MusicEnterDarkRoom>().EnteredDarkRoom) {
                GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>().FadeOutCurrent(0.5f);
                //GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>().SetMusic(GetComponent<AudioSource>());

                enterDarkRoom.GetComponent<MusicEnterDarkRoom>().EnteredDarkRoom = false;
            }
        }
        
    }

}
