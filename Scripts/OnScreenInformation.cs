using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnScreenInformation : MonoBehaviour
{
    private Coroutine m_BlackoutCoroutine = null;
    private bool m_BlackoutCoroutineActive = false;
    private Coroutine m_MessageCoroutine = null;
    private bool m_MessageCoroutineActive = false;
    private Coroutine m_SpriteCoroutine = null;
    private bool m_SpriteCoroutineActive = false;

    private SpriteRenderer m_BlackScreen;
    private TextMeshPro m_TMPLowMsg;
    private SpriteRenderer m_Sprite;
    private TextMeshPro m_TMPHighMsg;
    private TextMeshPro m_TMPQuoteMsg;

    private PlayerMovement m_PlayerMovement;
    private PlayerMouseLook m_PlayerMouseLook;


    private void Start() {
        m_BlackScreen = transform.GetChild(0).GetComponent<SpriteRenderer>();
        m_BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        m_TMPLowMsg = transform.GetChild(1).GetComponent<TextMeshPro>();

        m_Sprite = transform.GetChild(2).GetComponent<SpriteRenderer>();
        m_Sprite.color = new Color32(191, 19, 0, 255);

        m_TMPHighMsg = transform.GetChild(3).GetComponent<TextMeshPro>();
        m_TMPQuoteMsg = transform.GetChild(4).GetComponent<TextMeshPro>();

        m_PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        m_PlayerMouseLook = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerMouseLook>();
    }


    public void Blackout(float delay, float fadein, float active, float fadeout) {
        if (!m_BlackoutCoroutineActive) {
            m_BlackoutCoroutine = StartCoroutine(BlackoutEnumerator(delay, fadein, active, fadeout));
        }
        else {
            StopCoroutine(m_BlackoutCoroutine);
            m_BlackoutCoroutineActive = false;
            Blackout(delay, fadein, active, fadeout);
        }
    }


    public IEnumerator BlackoutEnumerator(float delay, float fadein, float active, float fadeout) {
        m_BlackoutCoroutineActive = true;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllowPauseMenu = false;

        // Freeze controls
        m_PlayerMouseLook.LockMouseControls = true;
        m_PlayerMovement.LockMovementControls = true;

        yield return new WaitForSeconds(delay);

        // TODO: the black sprite... should be more descriptive than "PlayerHealth".
        //PlayerHealth playerHealth = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealth>();

        m_BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        {   // Fade in
            float alpha = 0.0f;
            while (alpha < 1.0f) {
                alpha += Time.deltaTime / fadein;
                m_BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, alpha);

                yield return null;
            }
        }

        // Fully faded in
        m_BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        yield return new WaitForSeconds(active);

        // Give player controls back
        m_PlayerMouseLook.LockMouseControls = false;
        m_PlayerMovement.LockMovementControls = false;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllowPauseMenu = true;

        {   // Then fade out
            float alpha = 1.0f;
            while (alpha > 0.0f) {
                alpha -= Time.deltaTime / fadeout;
                m_BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, alpha);

                yield return null;
            }
        }

        // Fully faded out
        m_BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        m_BlackoutCoroutineActive = false;
    }


    public void ShowStaticMessage(string message, float alpha) {
        m_TMPHighMsg.color = new Color(m_TMPHighMsg.color.r, m_TMPHighMsg.color.g, m_TMPHighMsg.color.b, alpha);
        m_TMPHighMsg.text = message;
    }


    public void HideStaticMessage() {
        m_TMPHighMsg.color = new Color(m_TMPHighMsg.color.r, m_TMPHighMsg.color.g, m_TMPHighMsg.color.b, 0.0f);
        m_TMPHighMsg.text = "";
    }


    public void ShowStaticQuoteMessage(string message, float alpha) {
        m_TMPQuoteMsg.color = new Color(m_TMPQuoteMsg.color.r, m_TMPQuoteMsg.color.g, m_TMPQuoteMsg.color.b, alpha);
        m_TMPQuoteMsg.text = message;
    }


    public void HideStaticQuoteMessage() {
        m_TMPQuoteMsg.color = new Color(m_TMPQuoteMsg.color.r, m_TMPQuoteMsg.color.g, m_TMPQuoteMsg.color.b, 0.0f);
        m_TMPQuoteMsg.text = "";
    }


    public void ShowMessage(string message, float delay, float fadein, float active, float fadeout) {
        if (!m_MessageCoroutineActive) {
            m_MessageCoroutine = StartCoroutine(ShowMessageEnumerator(message, delay, fadein, active, fadeout));
        } else {
            if (m_MessageCoroutine != null) StopCoroutine(m_MessageCoroutine);
            m_MessageCoroutineActive = false;
            ShowMessage(message, delay, fadein, active, fadeout);
        }
    }


    public IEnumerator ShowMessageEnumerator(string message, float delay, float fadein, float active, float fadeout) {
        m_MessageCoroutineActive = true;
        yield return new WaitForSeconds(delay);

        m_TMPLowMsg.color = new Color(m_TMPLowMsg.color.r, m_TMPLowMsg.color.g, m_TMPLowMsg.color.b, 0.0f);
        m_TMPLowMsg.text = message;

        {   // Fade in
            float alpha = 0.0f;
            while (alpha < 1.0f) {
                alpha += Time.deltaTime / fadein;
                m_TMPLowMsg.color = new Color(m_TMPLowMsg.color.r, m_TMPLowMsg.color.g, m_TMPLowMsg.color.b, alpha);

                yield return null;
            }
        }

        // Fully faded in
        m_TMPLowMsg.color = new Color(m_TMPLowMsg.color.r, m_TMPLowMsg.color.g, m_TMPLowMsg.color.b, 1.0f);
        yield return new WaitForSeconds(active);

        {   // Fade out
            float alpha = 1.0f;
            while (alpha > 0.0f) {
                alpha -= Time.deltaTime / fadeout;
                m_TMPLowMsg.color = new Color(m_TMPLowMsg.color.r, m_TMPLowMsg.color.g, m_TMPLowMsg.color.b, alpha);

                yield return null;
            }
        }

        // Fully faded out
        m_TMPLowMsg.text = "";
        m_TMPLowMsg.color = new Color(m_TMPLowMsg.color.r, m_TMPLowMsg.color.g, m_TMPLowMsg.color.b, 0.0f);
        m_MessageCoroutineActive = false;
    }


    public void ShowSprite(Sprite sprite, float delay, float fadein, float active, float fadeout) {
        if (!m_SpriteCoroutineActive) {
            m_SpriteCoroutine = StartCoroutine(ShowSpriteEnumerator(sprite, delay, fadein, active, fadeout));
        } else {
            StopCoroutine(m_SpriteCoroutine);
            m_SpriteCoroutineActive = false;
            ShowSprite(sprite, delay, fadein, active, fadeout);
        }
    }


    public IEnumerator ShowSpriteEnumerator(Sprite sprite, float delay, float fadein, float active, float fadeout) {
        m_SpriteCoroutineActive = true;
        yield return new WaitForSeconds(delay);

        m_Sprite.color = new Color(m_Sprite.color.r, m_Sprite.color.g, m_Sprite.color.b, 0.0f);
        m_Sprite.sprite = sprite;

        {   // Fade in
            float alpha = 0.0f;
            while (alpha < 1.0f) {
                alpha += Time.deltaTime / fadein;
                m_Sprite.color = new Color(m_Sprite.color.r, m_Sprite.color.g, m_Sprite.color.b, alpha);

                yield return null;
            }
        }

        // Fully faded in
        m_Sprite.color = new Color(m_Sprite.color.r, m_Sprite.color.g, m_Sprite.color.b, 1.0f);
        yield return new WaitForSeconds(active);

        {   // Fade out
            float alpha = 1.0f;
            while (alpha > 0.0f) {
                alpha -= Time.deltaTime / fadeout;
                m_Sprite.color = new Color(m_Sprite.color.r, m_Sprite.color.g, m_Sprite.color.b, alpha);

                yield return null;
            }
        }

        // Fully faded out
        m_Sprite.color = new Color(m_Sprite.color.r, m_Sprite.color.g, m_Sprite.color.b, 0.0f);
        m_Sprite.sprite = null;
        m_SpriteCoroutineActive = false;
    }
}
