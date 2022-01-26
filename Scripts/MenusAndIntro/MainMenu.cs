using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private GameObject m_GameIntro;
    private GameObject m_Player;
    private SpriteRenderer m_BlackScreenFadeInSprite;

    private const KeyCode m_NewGame = KeyCode.N;
    private const KeyCode m_Continue = KeyCode.C;
    private const KeyCode m_Instructions = KeyCode.I;
    private const KeyCode m_Credits = KeyCode.R;
    private const KeyCode m_QuitGame = KeyCode.E;
    private const KeyCode m_Yes = KeyCode.Y;
    private const KeyCode m_No = KeyCode.N;
    private const KeyCode m_Back = KeyCode.B;

    private TextMeshPro m_TMPNewGame;
    private TextMeshPro m_TMPContinue;
    private TextMeshPro m_TMPInstructions;
    private TextMeshPro m_TMPCredits;
    private TextMeshPro m_TMPQuit;
    private TextMeshPro m_TMPNewGameYN;
    private TextMeshPro m_TMPContinueYN;
    private TextMeshPro m_TMPInstructionsText;
    private TextMeshPro m_TMPCreditsText;
    private TextMeshPro m_TMPQuitYN;
    private TextMeshPro m_TMPInfoNoSaving;
    private TextMeshPro m_TMPGameVersion;

    private AudioSource m_Beep1Sound;

    private enum State {
        MainMenu,
        StartingNewGame,
        ContinuingGame,
        ReadingInstructions,
        ReadingCredits,
        Quitting
    };
    State m_State = State.MainMenu;

    float m_MainMenuFadeInTimer = 0.0f;
    const float m_MainMenuFadeInTime = 1.75f;
    bool m_FullyFadedIn = false;

    private const float m_ColorHidden = 0.0f;
    private const float m_ColorPartial = 0.2f;
    private const float m_ColorVisible = 1.0f;

    private bool m_CanContinueGame;

    void Start()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerInMainMenu = true;
        GameObject.FindGameObjectWithTag("ObjectiveHud").GetComponent<ObjectiveHud>().AllowInventory = false;

        m_GameIntro = GameObject.Find("GameIntro");
        CursorManager.HideCursor();

        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Player.transform.position = GameObject.Find("SpawnForMainMenu").transform.position;
        m_Player.transform.rotation = GameObject.Find("SpawnForMainMenu").transform.rotation;

        m_BlackScreenFadeInSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();

        m_TMPNewGame            = transform.GetChild(1).GetChild(1).GetComponent<TextMeshPro>();
        m_TMPContinue           = transform.GetChild(1).GetChild(2).GetComponent<TextMeshPro>();
        m_TMPInstructions       = transform.GetChild(1).GetChild(3).GetComponent<TextMeshPro>();
        m_TMPCredits            = transform.GetChild(1).GetChild(4).GetComponent<TextMeshPro>();
        m_TMPQuit               = transform.GetChild(1).GetChild(5).GetComponent<TextMeshPro>();
        m_TMPNewGameYN          = transform.GetChild(1).GetChild(6).GetComponent<TextMeshPro>();
        m_TMPContinueYN         = transform.GetChild(1).GetChild(7).GetComponent<TextMeshPro>();
        m_TMPInstructionsText   = transform.GetChild(1).GetChild(8).GetComponent<TextMeshPro>();
        m_TMPCreditsText        = transform.GetChild(1).GetChild(9).GetComponent<TextMeshPro>();
        m_TMPQuitYN             = transform.GetChild(1).GetChild(10).GetComponent<TextMeshPro>();
        m_TMPInfoNoSaving       = transform.GetChild(1).GetChild(11).GetComponent<TextMeshPro>();

        m_TMPGameVersion        = transform.GetChild(1).GetChild(12).GetComponent<TextMeshPro>();
        m_TMPGameVersion.text = GameObject.FindGameObjectWithTag("GameVersion").GetComponent<GameVersion>().GetGameVersion();

        m_Beep1Sound = GameObject.FindGameObjectWithTag("SoundManager").transform.GetChild(0).GetComponent<AudioSource>();

        m_CanContinueGame = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CheckpointReached;

        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
        //GameObject.FindGameObjectWithTag("CursorManager").GetComponent<CursorManager>().ShowActualCursor();
        //GameObject.FindGameObjectWithTag("CursorManager").GetComponent<CursorManager>().HideActualCursor();
        BackToMainMenu();
    }



    void Update() {

        if (!m_FullyFadedIn) {
            m_MainMenuFadeInTimer += Time.deltaTime;
            m_BlackScreenFadeInSprite.color = new Color(0.0f, 0.0f, 0.0f, 1.0f - m_MainMenuFadeInTimer / m_MainMenuFadeInTime);
            if (m_MainMenuFadeInTimer > m_MainMenuFadeInTime) {
                m_BlackScreenFadeInSprite.enabled = false;
                m_FullyFadedIn = true;
            }
        }

        switch (m_State) {

            /* ****************** */
            case State.MainMenu:
                if (Input.GetKeyDown(m_NewGame)) {
                    NewGame(false);
                }

                if (Input.GetKeyDown(m_Continue) && m_CanContinueGame) {
                    ContinueGame(false);
                }

                if (Input.GetKeyDown(m_Instructions)) {
                    Instructions();
                }

                if (Input.GetKeyDown(m_Credits)) {
                    Credits();
                }

                if (Input.GetKeyDown(m_QuitGame) || Input.GetKeyDown(KeyCode.Escape)) {
                    QuitGame(false, true);
                }

                break;

            /* ****************** */
            case State.StartingNewGame:
                if (Input.GetKeyDown(m_Yes)) {
                    NewGame(true);
                }

                if (Input.GetKeyDown(m_No) || Input.GetKeyDown(KeyCode.Escape)) {
                    BackToMainMenu();
                }

                break;

            /* ****************** */
            case State.ContinuingGame:
                if (Input.GetKeyDown(m_Yes)) {
                    ContinueGame(true);
                }

                if (Input.GetKeyDown(m_No) || Input.GetKeyDown(KeyCode.Escape)) {
                    BackToMainMenu();
                }

                break;

            /* ****************** */
            case State.ReadingInstructions:
                if (Input.GetKeyDown(m_Back) || Input.GetKeyDown(m_Instructions) || Input.GetKeyDown(KeyCode.Escape)) {
                    BackToMainMenu();
                }

                break;

            /* ****************** */
            case State.ReadingCredits:
                if (Input.GetKeyDown(m_Back) || Input.GetKeyDown(m_Credits) || Input.GetKeyDown(KeyCode.Escape)) {
                    BackToMainMenu();
                }

                break;

            /* ****************** */
            case State.Quitting:
                if (Input.GetKeyDown(m_Yes)) {
                    QuitGame(true, false);
                }

                if (Input.GetKeyDown(m_No) || Input.GetKeyDown(KeyCode.Escape)) {
                    BackToMainMenu();
                }

                break;
        }
    }


    private void BackToMainMenu() {
        if (!m_Beep1Sound.isPlaying)
            m_Beep1Sound.Play();

        m_State = State.MainMenu;

        m_TMPNewGame.color = new Color(m_TMPNewGame.color.r, m_TMPNewGame.color.g, m_TMPNewGame.color.b, m_ColorVisible);
        m_TMPContinue.color = new Color(m_TMPContinue.color.r, m_TMPContinue.color.g, m_TMPContinue.color.b, m_CanContinueGame ? m_ColorVisible : m_ColorHidden);
        m_TMPInstructions.color = new Color(m_TMPInstructions.color.r, m_TMPInstructions.color.g, m_TMPInstructions.color.b, m_ColorVisible);
        m_TMPCredits.color = new Color(m_TMPCredits.color.r, m_TMPCredits.color.g, m_TMPCredits.color.b, m_ColorVisible);
        m_TMPQuit.color = new Color(m_TMPQuit.color.r, m_TMPQuit.color.g, m_TMPQuit.color.b, m_ColorVisible);
        m_TMPNewGameYN.color = new Color(m_TMPNewGameYN.color.r, m_TMPNewGameYN.color.g, m_TMPNewGameYN.color.b, m_ColorHidden);
        m_TMPContinueYN.color = new Color(m_TMPContinueYN.color.r, m_TMPContinueYN.color.g, m_TMPContinueYN.color.b, m_ColorHidden);
        m_TMPInstructionsText.color = new Color(m_TMPInstructionsText.color.r, m_TMPInstructionsText.color.g, m_TMPInstructionsText.color.b, m_ColorHidden);
        m_TMPCreditsText.color = new Color(m_TMPCreditsText.color.r, m_TMPCreditsText.color.g, m_TMPCreditsText.color.b, m_ColorHidden);
        m_TMPQuitYN.color = new Color(m_TMPQuitYN.color.r, m_TMPQuitYN.color.g, m_TMPQuitYN.color.b, m_ColorHidden);
        m_TMPInfoNoSaving.color = new Color(m_TMPInfoNoSaving.color.r, m_TMPInfoNoSaving.color.g, m_TMPInfoNoSaving.color.b, m_ColorVisible);
    }


    private void NewGame(bool confirm) {
        if (!m_Beep1Sound.isPlaying)
            m_Beep1Sound.Play();

        m_State = State.StartingNewGame;

        m_TMPNewGame.color = new Color(m_TMPNewGame.color.r, m_TMPNewGame.color.g, m_TMPNewGame.color.b, m_ColorVisible);
        m_TMPContinue.color = new Color(m_TMPContinue.color.r, m_TMPContinue.color.g, m_TMPContinue.color.b, m_CanContinueGame ? m_ColorPartial : m_ColorHidden);
        m_TMPInstructions.color = new Color(m_TMPInstructions.color.r, m_TMPInstructions.color.g, m_TMPInstructions.color.b, m_ColorPartial);
        m_TMPCredits.color = new Color(m_TMPCredits.color.r, m_TMPCredits.color.g, m_TMPCredits.color.b, m_ColorPartial);
        m_TMPQuit.color = new Color(m_TMPQuit.color.r, m_TMPQuit.color.g, m_TMPQuit.color.b, m_ColorPartial);
        m_TMPNewGameYN.color = new Color(m_TMPNewGameYN.color.r, m_TMPNewGameYN.color.g, m_TMPNewGameYN.color.b, m_ColorVisible);
        m_TMPContinueYN.color = new Color(m_TMPContinueYN.color.r, m_TMPContinueYN.color.g, m_TMPContinueYN.color.b, m_ColorHidden);
        m_TMPInstructionsText.color = new Color(m_TMPInstructionsText.color.r, m_TMPInstructionsText.color.g, m_TMPInstructionsText.color.b, m_ColorHidden);
        m_TMPCreditsText.color = new Color(m_TMPCreditsText.color.r, m_TMPCreditsText.color.g, m_TMPCreditsText.color.b, m_ColorHidden);
        m_TMPQuitYN.color = new Color(m_TMPQuitYN.color.r, m_TMPQuitYN.color.g, m_TMPQuitYN.color.b, m_ColorHidden);
        m_TMPInfoNoSaving.color = new Color(m_TMPInfoNoSaving.color.r, m_TMPInfoNoSaving.color.g, m_TMPInfoNoSaving.color.b, m_ColorVisible);

        if (confirm) {
            m_GameIntro.GetComponent<GameIntro>().StartGameIntro(false);
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CheckpointReached = false;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerInMainMenu = false;
            Destroy(gameObject);
        }
    }


    private void ContinueGame(bool confirm) {
        if (!m_Beep1Sound.isPlaying)
            m_Beep1Sound.Play();

        m_State = State.ContinuingGame;

        m_TMPNewGame.color = new Color(m_TMPNewGame.color.r, m_TMPNewGame.color.g, m_TMPNewGame.color.b, m_ColorPartial);
        m_TMPContinue.color = new Color(m_TMPContinue.color.r, m_TMPContinue.color.g, m_TMPContinue.color.b, m_ColorVisible);
        m_TMPInstructions.color = new Color(m_TMPInstructions.color.r, m_TMPInstructions.color.g, m_TMPInstructions.color.b, m_ColorPartial);
        m_TMPCredits.color = new Color(m_TMPCredits.color.r, m_TMPCredits.color.g, m_TMPCredits.color.b, m_ColorPartial);
        m_TMPQuit.color = new Color(m_TMPQuit.color.r, m_TMPQuit.color.g, m_TMPQuit.color.b, m_ColorPartial);
        m_TMPNewGameYN.color = new Color(m_TMPNewGameYN.color.r, m_TMPNewGameYN.color.g, m_TMPNewGameYN.color.b, m_ColorHidden);
        m_TMPContinueYN.color = new Color(m_TMPContinueYN.color.r, m_TMPContinueYN.color.g, m_TMPContinueYN.color.b, m_ColorVisible);
        m_TMPInstructionsText.color = new Color(m_TMPInstructionsText.color.r, m_TMPInstructionsText.color.g, m_TMPInstructionsText.color.b, m_ColorHidden);
        m_TMPCreditsText.color = new Color(m_TMPCreditsText.color.r, m_TMPCreditsText.color.g, m_TMPCreditsText.color.b, m_ColorHidden);
        m_TMPQuitYN.color = new Color(m_TMPQuitYN.color.r, m_TMPQuitYN.color.g, m_TMPQuitYN.color.b, m_ColorHidden);
        m_TMPInfoNoSaving.color = new Color(m_TMPInfoNoSaving.color.r, m_TMPInfoNoSaving.color.g, m_TMPInfoNoSaving.color.b, m_ColorVisible);

        if (confirm) {
            m_GameIntro.GetComponent<GameIntro>().StartGameIntro(true);
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerInMainMenu = false;
            Destroy(gameObject);
        }
    }


    private void Instructions() {
        if (!m_Beep1Sound.isPlaying)
            m_Beep1Sound.Play();

        m_State = State.ReadingInstructions;

        m_TMPNewGame.color = new Color(m_TMPNewGame.color.r, m_TMPNewGame.color.g, m_TMPNewGame.color.b, m_ColorHidden);
        m_TMPContinue.color = new Color(m_TMPContinue.color.r, m_TMPContinue.color.g, m_TMPContinue.color.b, m_CanContinueGame ? m_ColorHidden : m_ColorHidden);
        m_TMPInstructions.color = new Color(m_TMPInstructions.color.r, m_TMPInstructions.color.g, m_TMPInstructions.color.b, m_ColorHidden);
        m_TMPCredits.color = new Color(m_TMPCredits.color.r, m_TMPCredits.color.g, m_TMPCredits.color.b, m_ColorHidden);
        m_TMPQuit.color = new Color(m_TMPQuit.color.r, m_TMPQuit.color.g, m_TMPQuit.color.b, m_ColorHidden);
        m_TMPNewGameYN.color = new Color(m_TMPNewGameYN.color.r, m_TMPNewGameYN.color.g, m_TMPNewGameYN.color.b, m_ColorHidden);
        m_TMPContinueYN.color = new Color(m_TMPContinueYN.color.r, m_TMPContinueYN.color.g, m_TMPContinueYN.color.b, m_ColorHidden);
        m_TMPInstructionsText.color = new Color(m_TMPInstructionsText.color.r, m_TMPInstructionsText.color.g, m_TMPInstructionsText.color.b, m_ColorVisible);
        m_TMPCreditsText.color = new Color(m_TMPCreditsText.color.r, m_TMPCreditsText.color.g, m_TMPCreditsText.color.b, m_ColorHidden);
        m_TMPQuitYN.color = new Color(m_TMPQuitYN.color.r, m_TMPQuitYN.color.g, m_TMPQuitYN.color.b, m_ColorHidden);
        m_TMPInfoNoSaving.color = new Color(m_TMPInfoNoSaving.color.r, m_TMPInfoNoSaving.color.g, m_TMPInfoNoSaving.color.b, m_ColorHidden);
    }


    private void Credits() {
        if (!m_Beep1Sound.isPlaying)
            m_Beep1Sound.Play();

        m_State = State.ReadingCredits;

        m_TMPNewGame.color = new Color(m_TMPNewGame.color.r, m_TMPNewGame.color.g, m_TMPNewGame.color.b, m_ColorHidden);
        m_TMPContinue.color = new Color(m_TMPContinue.color.r, m_TMPContinue.color.g, m_TMPContinue.color.b, m_CanContinueGame ? m_ColorHidden : m_ColorHidden);
        m_TMPInstructions.color = new Color(m_TMPInstructions.color.r, m_TMPInstructions.color.g, m_TMPInstructions.color.b, m_ColorHidden);
        m_TMPCredits.color = new Color(m_TMPCredits.color.r, m_TMPCredits.color.g, m_TMPCredits.color.b, m_ColorHidden);
        m_TMPQuit.color = new Color(m_TMPQuit.color.r, m_TMPQuit.color.g, m_TMPQuit.color.b, m_ColorHidden);
        m_TMPNewGameYN.color = new Color(m_TMPNewGameYN.color.r, m_TMPNewGameYN.color.g, m_TMPNewGameYN.color.b, m_ColorHidden);
        m_TMPContinueYN.color = new Color(m_TMPContinueYN.color.r, m_TMPContinueYN.color.g, m_TMPContinueYN.color.b, m_ColorHidden);
        m_TMPInstructionsText.color = new Color(m_TMPInstructionsText.color.r, m_TMPInstructionsText.color.g, m_TMPInstructionsText.color.b, m_ColorHidden);
        m_TMPCreditsText.color = new Color(m_TMPCreditsText.color.r, m_TMPCreditsText.color.g, m_TMPCreditsText.color.b, m_ColorVisible);
        m_TMPQuitYN.color = new Color(m_TMPQuitYN.color.r, m_TMPQuitYN.color.g, m_TMPQuitYN.color.b, m_ColorHidden);
        m_TMPInfoNoSaving.color = new Color(m_TMPInfoNoSaving.color.r, m_TMPInfoNoSaving.color.g, m_TMPInfoNoSaving.color.b, m_ColorHidden);
    }


    private void QuitGame(bool confirm, bool beep) {
        if (!m_Beep1Sound.isPlaying && beep)
            m_Beep1Sound.Play();

        m_State = State.Quitting;

        m_TMPNewGame.color = new Color(m_TMPNewGame.color.r, m_TMPNewGame.color.g, m_TMPNewGame.color.b, m_ColorPartial);
        m_TMPContinue.color = new Color(m_TMPContinue.color.r, m_TMPContinue.color.g, m_TMPContinue.color.b, m_CanContinueGame ? m_ColorPartial : m_ColorHidden);
        m_TMPInstructions.color = new Color(m_TMPInstructions.color.r, m_TMPInstructions.color.g, m_TMPInstructions.color.b, m_ColorPartial);
        m_TMPCredits.color = new Color(m_TMPCredits.color.r, m_TMPCredits.color.g, m_TMPCredits.color.b, m_ColorPartial);
        m_TMPQuit.color = new Color(m_TMPQuit.color.r, m_TMPQuit.color.g, m_TMPQuit.color.b, m_ColorVisible);
        m_TMPNewGameYN.color = new Color(m_TMPNewGameYN.color.r, m_TMPNewGameYN.color.g, m_TMPNewGameYN.color.b, m_ColorHidden);
        m_TMPContinueYN.color = new Color(m_TMPContinueYN.color.r, m_TMPContinueYN.color.g, m_TMPContinueYN.color.b, m_ColorHidden);
        m_TMPInstructionsText.color = new Color(m_TMPInstructionsText.color.r, m_TMPInstructionsText.color.g, m_TMPInstructionsText.color.b, m_ColorHidden);
        m_TMPCreditsText.color = new Color(m_TMPCreditsText.color.r, m_TMPCreditsText.color.g, m_TMPCreditsText.color.b, m_ColorHidden);
        m_TMPQuitYN.color = new Color(m_TMPQuitYN.color.r, m_TMPQuitYN.color.g, m_TMPQuitYN.color.b, m_ColorVisible);
        m_TMPInfoNoSaving.color = new Color(m_TMPInfoNoSaving.color.r, m_TMPInfoNoSaving.color.g, m_TMPInfoNoSaving.color.b, m_ColorVisible);

        if (confirm) {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

    }
}
