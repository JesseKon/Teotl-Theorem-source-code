// TODO: this file is a mess...

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Narrator : MonoBehaviour
{
    //public float startDelay = 0.0f;
    //public float textWriteSpeed = 15.0f;
    //public float prolongTimer = 3.0f; // For how long the text will stay on screen after it's fully written
    //public float fadeoutTimer = 0.8f;

    [Space]
    public Color fontColor = Color.white;


    public string NarratorText {
        get { return m_NarratorText; }
        set {
            m_NarratorText = value;
            m_TextMeshPro.text = m_NarratorText;
        }
    }
    private string m_NarratorText;

    public Color NarratorTextColor {
        get { return m_NarratorTextColor; }
        set {
            m_NarratorTextColor = value;
            m_TextMeshPro.color = m_NarratorTextColor;
        }
    }
    private Color m_NarratorTextColor;

    private TextMeshPro m_TextMeshPro;


    public enum WriteStyle {
        Fadein,
        Typewriter
    }


    private void Start() {
        m_TextMeshPro = gameObject.GetComponentInChildren<TextMeshPro>();
        m_TextMeshPro.color = fontColor;
        if (fontColor.a < 0.5f)
            Debug.LogWarning("Narrator font color alpha is less than 0.5f");

        transform.GetChild(0).transform.position = Vector3.zero;
    }


    /**
     * Shows narrator that has constant timings.
     */
    public IEnumerator ShowStaticNarrator(string message, WriteStyle writeStyle = WriteStyle.Fadein, float startDelay = 0.0f, float attackTime = 0.8f, float sustainTime = 3.5f, float fadeoutTime = 0.8f) {
        float elapsedTime = 0.0f;
        Color actualFontColor = fontColor;
        Color fadeoutColor;

        // Start delay
        while (elapsedTime < startDelay) {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Fade in effect
        if (writeStyle == WriteStyle.Fadein) {
            elapsedTime = 0.0f;
            m_TextMeshPro.text = message;
            fadeoutColor = actualFontColor;
            while (elapsedTime < attackTime + sustainTime) {
                fadeoutColor.a = fontColor.a * (elapsedTime / attackTime);
                m_TextMeshPro.color = fadeoutColor;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        // "Typewriter" effect
        else if (writeStyle == WriteStyle.Typewriter) {
            elapsedTime = 0.0f;
            while (elapsedTime < attackTime + sustainTime) {
                m_TextMeshPro.text = message.Substring(0, (int)Mathf.Clamp(elapsedTime / attackTime * message.Length, 0.0f, message.Length));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }


        // Fade out effect
        elapsedTime = 0.0f;
        fadeoutColor = actualFontColor;
        //while (elapsedTime < fadeoutTime) {
        //    fadeoutColor.a = fontColor.a * (1.0f - (elapsedTime / fadeoutTime));
        //    m_TextMeshPro.color = fadeoutColor;
        //    elapsedTime += Time.deltaTime;
        //    yield return null;
        //}

        float alpha = 1.0f;
        while (alpha > 0.0f) {
            alpha -= Time.deltaTime / fadeoutTime;

            fadeoutColor.a = alpha;
            m_TextMeshPro.color = fadeoutColor;

            yield return null;
        }

        m_TextMeshPro.text = "";
        yield break;
    }
}
