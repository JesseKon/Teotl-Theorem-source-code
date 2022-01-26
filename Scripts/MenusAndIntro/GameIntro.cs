using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameIntro : MonoBehaviour
{
    private OnScreenInformation m_OnScreenInformation;
    private PlayerMovement m_PlayerMovement;
    private PlayerMouseLook m_PlayerMouseLook;
    private AudioSource m_Beep2Sound;
    private MessagesAndNoteTexts m_Texts;

    private SpriteRenderer m_BlackScreen;
    private TextMeshPro m_TMPLowMsg;
    private TextMeshPro m_TMPHighMsg;

    private bool m_GameIntroStarted = false;
    private bool m_SpawnAtMainLobby = false;

    private float m_Timer = 0.0f;

    private float m_EscapeIntroTimer = 0.0f;
    private const float m_EscapeIntroTime = 2.0f;

    private bool m_AllowIntroSkip = false;
    private bool m_IntroEnding = false;

    private enum GameIntroState
    {
        Booting1,
        Booting2,
        Booting3,

        Message1,
        Message2,
        Message3,
        Message4,

        Ending
    }
    private GameIntroState m_GameIntroState = GameIntroState.Booting1;


    private void Start() {
        m_OnScreenInformation = GameObject.Find("OnScreenInformation").GetComponent<OnScreenInformation>();
        m_PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        m_PlayerMouseLook = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerMouseLook>();
        m_Beep2Sound = GameObject.FindGameObjectWithTag("SoundManager").transform.GetChild(1).GetComponent<AudioSource>();
        m_Texts = GameObject.Find("MessagesAndNoteTexts").GetComponent<MessagesAndNoteTexts>();

        m_BlackScreen = transform.GetChild(0).GetComponent<SpriteRenderer>();
        m_BlackScreen.color = new Color(0.5f, 0.5f, 0.5f, 0.0f);
        m_TMPLowMsg = transform.GetChild(1).GetComponent<TextMeshPro>();
        m_TMPHighMsg = transform.GetChild(2).GetComponent<TextMeshPro>();


        m_PlayerMouseLook.LockMouseControls = true;
        m_PlayerMovement.LockMovementControls = true;
    }


    private void Update() {
        if (!m_GameIntroStarted)
            return;

        // Hold escape to skip intro
        if (Input.GetKey(KeyCode.Escape) && !m_IntroEnding && m_AllowIntroSkip) {
            m_TMPHighMsg.text = m_Texts.HoldEscToSkip;
            m_EscapeIntroTimer += Time.deltaTime;
        }

        else {
            m_TMPHighMsg.text = "";
            m_EscapeIntroTimer = 0.0f;
        }

        if (m_EscapeIntroTimer >= m_EscapeIntroTime)
            SkipGameIntro();

        // Intro states and messages
        m_Timer += Time.deltaTime;
        switch (m_GameIntroState) {
            case GameIntroState.Booting1:
                m_TMPLowMsg.text = m_Texts.IntroBooting1;
                if (m_Timer > 1.5f) {
                    m_Timer = 0.0f;
                    m_GameIntroState = GameIntroState.Booting2;
                }
                break;

            case GameIntroState.Booting2:
                m_TMPLowMsg.text = m_Texts.IntroBooting2;
                if (m_Timer > 1.5f) {
                    m_Timer = 0.0f;
                    m_GameIntroState = GameIntroState.Booting3;
                }
                break;

            case GameIntroState.Booting3:
                m_TMPLowMsg.text = m_Texts.IntroBooting3;
                if (m_Timer > 1.5f) {
                    m_Timer = 0.0f;
                    if (m_SpawnAtMainLobby)
                        m_GameIntroState = GameIntroState.Ending;
                    else
                        m_GameIntroState = GameIntroState.Message1;

                    m_AllowIntroSkip = true;
                    m_Beep2Sound.Play();
                }
                break;

            case GameIntroState.Message1:
                m_TMPLowMsg.text = m_Texts.IntroMessage1;
                m_TMPLowMsg.color = new Color(m_TMPLowMsg.color.r, m_TMPLowMsg.color.g, m_TMPLowMsg.color.b, GetFadeOutAlpha(m_Timer, 4.0f, 1.5f));
                if (m_Timer > 5.5f) {
                    m_Timer = 0.0f;
                    m_GameIntroState = GameIntroState.Message2;
                    m_Beep2Sound.Play();
                }
                break;

            case GameIntroState.Message2:
                m_TMPLowMsg.text = m_Texts.IntroMessage2;
                m_TMPLowMsg.color = new Color(m_TMPLowMsg.color.r, m_TMPLowMsg.color.g, m_TMPLowMsg.color.b, GetFadeOutAlpha(m_Timer, 4.0f, 1.5f));
                if (m_Timer > 5.5f) {
                    m_Timer = 0.0f;
                    m_GameIntroState = GameIntroState.Message3;
                    m_Beep2Sound.Play();
                }
                break;

            case GameIntroState.Message3:
                m_TMPLowMsg.text = m_Texts.IntroMessage3;
                m_TMPLowMsg.color = new Color(m_TMPLowMsg.color.r, m_TMPLowMsg.color.g, m_TMPLowMsg.color.b, GetFadeOutAlpha(m_Timer, 4.0f, 1.5f));
                if (m_Timer > 5.5f) {
                    m_Timer = 0.0f;
                    m_GameIntroState = GameIntroState.Message4;
                    m_Beep2Sound.Play();
                }
                break;

            case GameIntroState.Message4:
                m_TMPLowMsg.text = m_Texts.IntroMessage4;
                m_TMPLowMsg.color = new Color(m_TMPLowMsg.color.r, m_TMPLowMsg.color.g, m_TMPLowMsg.color.b, GetFadeOutAlpha(m_Timer, 4.0f, 1.5f));
                if (m_Timer > 5.5f) {
                    m_Timer = 0.0f;
                    m_GameIntroState = GameIntroState.Ending;
                    m_Beep2Sound.Play();
                }
                break;

            case GameIntroState.Ending:
                if (m_SpawnAtMainLobby) {
                    m_TMPLowMsg.text = "";
                }
                else {
                    m_TMPLowMsg.text = m_Texts.IntroEnding;
                    m_TMPLowMsg.color = new Color(m_TMPLowMsg.color.r, m_TMPLowMsg.color.g, m_TMPLowMsg.color.b, GetFadeOutAlpha(m_Timer, 2.0f, 2.0f));
                }

                if (!m_IntroEnding) {
                    if (m_SpawnAtMainLobby)
                        SpawnPlayerToMainLobby();
                    else
                        SpawnPlayerToLevel1();

                    EndBlackScreen();

                    m_PlayerMouseLook.LockMouseControls = false;
                    m_PlayerMovement.LockMovementControls = false;

                    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllowPauseMenu = true;
                    GameObject.FindGameObjectWithTag("ObjectiveHud").GetComponent<ObjectiveHud>().AllowInventory = true;
                    CursorManager.ShowCursor();

                    m_IntroEnding = true;
                    Destroy(gameObject, 5.0f);
                }

                break;
        }

    }

    private float GetFadeOutAlpha(float currentTime, float from, float fadeoutTime) {
        if (currentTime >= from)
            return 1.0f - (currentTime - from) / fadeoutTime;

        return 1.0f;
    }


    public void StartGameIntro(bool spawnAtMainLobby) {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllowPauseMenu = false;
        m_Timer = 0.0f;
        StartBlackScreen();
        m_GameIntroStarted = true;
        m_SpawnAtMainLobby = spawnAtMainLobby;
    }


    private void SkipGameIntro() {
        if (m_IntroEnding)
            return;

        m_Timer = 0.0f;
        m_GameIntroState = GameIntroState.Ending;
    }


    private void StartBlackScreen() {
        m_BlackScreen.color = new Color(0.1f, 0.1f, 0.1f, 1.0f);
    }


    private void EndBlackScreen() {
        StartCoroutine(FadeOutBlackScreen(2.0f));
    }


    private IEnumerator FadeOutBlackScreen(float time) {
        float alpha = 1.0f;
        while (alpha >= 0.0f) {
            alpha -= Time.deltaTime / time;
            m_BlackScreen.color = new Color(0.1f, 0.1f, 0.1f, alpha);
            yield return null;
        }
    }


    // Player start position and rotation
    private void SpawnPlayerToLevel1() {
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0.0f, 1.0f, 0.0f);
        GameObject.FindGameObjectWithTag("Player").transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    }


    // Player start position and rotation when continuing previous game
    private void SpawnPlayerToMainLobby() {
        if (GameObject.Find("Portal (from level4 to liminal3)"))
            GameObject.Find("Portal (from level4 to liminal3)").SetActive(false);

        if (GameObject.Find("ManualDoor_400_on_floor"))
            GameObject.Find("ManualDoor_400_on_floor").SetActive(false);

        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0.0f, 1.0f, 100.0f);
        GameObject.FindGameObjectWithTag("Player").transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    }
}
