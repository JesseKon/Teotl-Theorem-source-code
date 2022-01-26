using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteManager : MonoBehaviour
{
    [TextArea]
    public string[] m_NoteTexts;

    private void Start() {
        GameObject[] notes = GameObject.FindGameObjectsWithTag("Note");
        int index = 0;
        foreach (GameObject note in notes) {
            note.transform.GetChild(1).GetComponent<TextMeshPro>().text = m_NoteTexts[index++];
        }
    }
}
