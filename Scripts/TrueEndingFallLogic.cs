using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueEndingFallLogic : MonoBehaviour
{
    SpriteRenderer m_SpriteBlackScreen;
    MessagesAndNoteTexts texts;

    private const float m_FadeTime = 2.0f;

    private float m_Alpha = 0.0f;
    private bool m_Fading = false;
    private bool m_StartEnding = false;

    private void Start() {
        m_SpriteBlackScreen = transform.GetChild(0).GetComponent<SpriteRenderer>();
        m_SpriteBlackScreen.color = new Color(0.0f, 0.0f, 0.0f, m_Alpha);

        texts = GameObject.Find("MessagesAndNoteTexts").GetComponent<MessagesAndNoteTexts>();
    }


    private void Update() {
        if (m_Fading) {
            m_Alpha += Time.deltaTime;
            m_SpriteBlackScreen.color = new Color(0.0f, 0.0f, 0.0f, m_Alpha / m_FadeTime);

            if (m_Alpha / m_FadeTime > 1.0f && !m_StartEnding) {
                GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealth>().ShutPlayerDown(false, true, true, texts.TrueExitMessage, texts.TrueExitQuote);
                m_StartEnding = true;
            }
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player fell down at the true ending");
            m_Fading = true;
        }
    }

}
