using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEnterDarkRoom : MonoBehaviour
{
    public GameObject exitDarkRoom;

    public bool EnteredDarkRoom {
        get { return m_EnteredDarkRoom; }
        set { m_EnteredDarkRoom = value; }
    }
    private bool m_EnteredDarkRoom = false;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            m_EnteredDarkRoom = true;

            if (exitDarkRoom.GetComponent<MusicExitDarkRoom>().ExitedDarkRoom) {
                GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>().FadeOutCurrent(0.5f);
                //GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>().SetMusic(GetComponent<AudioSource>());

                exitDarkRoom.GetComponent<MusicExitDarkRoom>().ExitedDarkRoom = false;
            }
        } 
    }

}
