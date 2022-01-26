using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueEndingPortalSpawn : MonoBehaviour
{
    public GameObject m_NoteToRead;
    public GameObject m_PortalToSpawn;


    private void Start() {
        m_PortalToSpawn.SetActive(false);
    }


    private void Update() {
        if (m_NoteToRead.GetComponent<Note>().IsNoteSeen())
            m_PortalToSpawn.SetActive(true);
    }
}
