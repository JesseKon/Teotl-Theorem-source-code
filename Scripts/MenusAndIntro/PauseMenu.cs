using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public AudioMixer m_AudioMixer;

    private TextMeshPro m_TMPPauseMenu;
    private TextMeshPro m_TMPContinue;
    private TextMeshPro m_TMPExit;
    private TextMeshPro m_TMPExitYN;
    private TextMeshPro m_TMPMouseSensitivity;
    private TextMeshPro m_TMPMouseSensitivitySlider;
    private TextMeshPro m_TMPAudioMaster;
    private TextMeshPro m_TMPAudioMasterSlider;

    private GameManager m_GameManager;
    private AudioSource m_Beep2Sound;

    private const KeyCode m_LessMouseSensitivity = KeyCode.A;
    private const KeyCode m_MoreMouseSensitivity = KeyCode.S;
    private const KeyCode m_LessAudioVolume = KeyCode.Z;
    private const KeyCode m_MoreAudioVolume = KeyCode.X;

    private const float m_ColorHidden = 0.0f;
    private const float m_ColorPartial = 0.2f;
    private const float m_ColorVisible = 1.0f;

    private bool m_Quitting = false;

    private void Awake() {
        m_TMPPauseMenu = transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>();
        m_TMPContinue = transform.GetChild(0).GetChild(1).GetComponent<TextMeshPro>();
        m_TMPExit = transform.GetChild(0).GetChild(2).GetComponent<TextMeshPro>();
        m_TMPExitYN = transform.GetChild(0).GetChild(3).GetComponent<TextMeshPro>();
        m_TMPMouseSensitivity = transform.GetChild(0).GetChild(4).GetComponent<TextMeshPro>();
        m_TMPMouseSensitivitySlider = transform.GetChild(0).GetChild(5).GetComponent<TextMeshPro>();
        m_TMPAudioMaster = transform.GetChild(0).GetChild(6).GetComponent<TextMeshPro>();
        m_TMPAudioMasterSlider = transform.GetChild(0).GetChild(7).GetComponent<TextMeshPro>();
    }

    private void Start() {


        m_GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        m_Beep2Sound = GameObject.FindGameObjectWithTag("SoundManager").transform.GetChild(1).GetComponent<AudioSource>();
    }

    public void JustActivated() {
        m_Quitting = false;

        m_TMPPauseMenu.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorVisible);
        m_TMPContinue.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorVisible);
        m_TMPExit.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorVisible);
        m_TMPExitYN.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorHidden);
        m_TMPMouseSensitivity.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorVisible);
        m_TMPMouseSensitivitySlider.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorVisible);
        m_TMPAudioMaster.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorVisible);
        m_TMPAudioMasterSlider.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorVisible);
    }


    private void Update() {

        // Player is quitting
        if (m_Quitting) {
            m_TMPPauseMenu.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorPartial);
            m_TMPContinue.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorPartial);
            m_TMPExit.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorVisible);
            m_TMPExitYN.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorVisible);
            m_TMPMouseSensitivity.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorPartial);
            m_TMPMouseSensitivitySlider.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorPartial);
            m_TMPAudioMaster.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorPartial);
            m_TMPAudioMasterSlider.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorPartial);

            if (Input.GetKeyDown(KeyCode.Y)) {
                StartCoroutine(GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealth>().QuitGame(0.0f));
            }

            if (Input.GetKeyDown(KeyCode.N)) {
                m_Quitting = false;
                m_Beep2Sound.Play();
            }

        }

        // Not quitting
        else {
            m_TMPPauseMenu.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorVisible);
            m_TMPContinue.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorVisible);
            m_TMPExit.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorVisible);
            m_TMPExitYN.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorHidden);
            m_TMPMouseSensitivity.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorVisible);
            m_TMPMouseSensitivitySlider.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorVisible);
            m_TMPAudioMaster.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorVisible);
            m_TMPAudioMasterSlider.color = new Color(m_TMPPauseMenu.color.r, m_TMPPauseMenu.color.g, m_TMPPauseMenu.color.b, m_ColorVisible);

            m_TMPMouseSensitivitySlider.text = "<color=#9c9cf0>[A]</color>[" + RepeatCharacter('X', m_GameManager.MouseSensitivity) + RepeatCharacter('_', 9 - m_GameManager.MouseSensitivity) + "]<color=#9c9cf0>[S]</color>";
            m_TMPAudioMasterSlider.text = "<color=#9c9cf0>[Z]</color>[" + RepeatCharacter('X', m_GameManager.AudioMasterVolume) + RepeatCharacter('_', 9 - m_GameManager.AudioMasterVolume) + "]<color=#9c9cf0>[X]</color>";

            if (Input.GetKeyDown(KeyCode.E)) {
                m_Quitting = true;
                m_Beep2Sound.Play();
            }

            if (Input.GetKeyDown(m_LessMouseSensitivity)) {
                --m_GameManager.MouseSensitivity;
                m_Beep2Sound.Play();
            }

            if (Input.GetKeyDown(m_MoreMouseSensitivity)) {
                ++m_GameManager.MouseSensitivity;
                m_Beep2Sound.Play();
            }

            if (Input.GetKeyDown(m_LessAudioVolume)) {
                --m_GameManager.AudioMasterVolume;
                m_Beep2Sound.Play();
            }

            if (Input.GetKeyDown(m_MoreAudioVolume)) {
                ++m_GameManager.AudioMasterVolume;
                m_Beep2Sound.Play();
            }

        }
    }


    private string RepeatCharacter(char character, int times) {
        string str = "";
        for (int i = 0; i < times; ++i) {
            str += character;
        }

        return str;
    }

}
