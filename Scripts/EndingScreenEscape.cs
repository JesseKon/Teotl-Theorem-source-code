using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingScreenEscape : MonoBehaviour
{
    private float m_ExitTimer = 0.0f;
    private const float m_ExitTime = 2.0f;
    private bool m_ShuttingDown = false;

    MessagesAndNoteTexts m_Texts;

    private void Start() {
        m_Texts = GameObject.Find("MessagesAndNoteTexts").GetComponent<MessagesAndNoteTexts>();
    }

    // Update is called once per frame
    void Update()
    {
        // Player on ending screen
        if (GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealth>().OnEndingScreen) {
            if (Input.GetKey(KeyCode.Escape)) {
                m_ExitTimer += Time.deltaTime;
                transform.GetChild(0).GetComponent<TextMeshPro>().text = m_Texts.HoldEscToExit;
            }
            else {
                m_ExitTimer = 0.0f;
                transform.GetChild(0).GetComponent<TextMeshPro>().text = "";
            }

            if (m_ExitTimer > m_ExitTime && !m_ShuttingDown) {
                Debug.Log("Shutting down from EndingScreenEscape");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                m_ShuttingDown = true;
            }
        }
    }
}
