using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStuckNoteLogics : MonoBehaviour
{
    public GameObject m_Door;
    public GameObject m_Note;
    public GameObject m_Plinth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Door.GetComponent<ElectricDoor>().IsOpen()) {
            m_Note.SetActive(false);
            if (m_Plinth)
                m_Plinth.SetActive(false);
        }
        else {
            m_Note.SetActive(true);
            if (m_Plinth)
                m_Plinth.SetActive(true);
        }
    }
}
