using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    public Transform m_SpawnTransform;
    public bool m_SetEnvironmentToRed = false;

    public enum PortalType
    {
        FromLevel4ToLiminal,
        FromLiminalToLiminal_Portal1,
        FromLiminalToLiminal_Portal2,
        FromLiminalToLiminal_Portal3,
        FromLiminalToLevel4,
        ToTrueEnding,
        ShutDown,
        ShutDownDontStopMusicFakeEnding,
        ShutDownDontStopMusicTrueEnding,
    }
    public PortalType m_PortalType;

    public Sprite m_SpriteAlpha;
    public Sprite m_SpriteOmega;
    public Sprite m_SpritePi;
    public Sprite m_SpriteTheta;

    private const float m_BlackoutDelayTime = 0.0f;
    private const float m_BlackoutFadeinTime = 0.0f;
    private const float m_BlackoutActiveTime = 3.0f;
    private const float m_BlackoutFadeoutTime = 3.0f;

    private string m_Message = "";
    private string m_Quote = "";
    private const float m_MessageDelayTime = 0.5f;
    private const float m_MessageFadeinTime = 1.5f;
    private const float m_MessageActiveTime = 3.0f;
    private const float m_MessageFadeoutTime = 1.5f;

    private Sprite m_Sprite;
    private const float m_SpriteDelayTime = 1.2f;
    private const float m_SpriteFadeinTime = 0.3f;
    private const float m_SpriteActiveTime = 3.0f;
    private const float m_SpriteFadeoutTime = 1.5f;

    private bool m_EnteredPortal = false;
    private OnScreenInformation m_OnScreenInformation;

    private bool m_ShutDownPlayer = false;
    private bool m_TrueEndingShutDown = false;


    private void Start() {
        m_OnScreenInformation = GameObject.Find("OnScreenInformation").GetComponent<OnScreenInformation>();

        MessagesAndNoteTexts texts = GameObject.Find("MessagesAndNoteTexts").GetComponent<MessagesAndNoteTexts>();
        switch (m_PortalType) {
            case PortalType.FromLevel4ToLiminal:
                m_Message = texts.Portal_FromLevel4ToLiminal_Message;
                m_Sprite = m_SpriteAlpha;
                break;
            case PortalType.FromLiminalToLiminal_Portal1:
                m_Message = texts.Portal1_FromLiminalToLiminal_Message;
                m_Sprite = m_SpriteOmega;
                break;
            case PortalType.FromLiminalToLiminal_Portal2:
                m_Message = texts.Portal2_FromLiminalToLiminal_Message;
                m_Sprite = m_SpriteOmega;
                break;
            case PortalType.FromLiminalToLiminal_Portal3:
                m_Message = texts.Portal3_FromLiminalToLiminal_Message;
                m_Sprite = m_SpriteOmega;
                break;
            case PortalType.FromLiminalToLevel4:
                m_Message = texts.Portal_FromLiminalToLevel4_Message;
                m_Sprite = m_SpriteOmega;
                break;
            case PortalType.ToTrueEnding:
                m_Message = texts.Portal_ToTrueExit_Message;
                break;
            case PortalType.ShutDown:
                m_Message = "";
                m_ShutDownPlayer = true;
                m_TrueEndingShutDown = false;
                break;
            case PortalType.ShutDownDontStopMusicFakeEnding:
                m_Message = texts.FakeExitMessage;
                m_Quote = texts.FakeExitQuote;
                m_ShutDownPlayer = true;
                m_TrueEndingShutDown = true;
                break;
            case PortalType.ShutDownDontStopMusicTrueEnding:
                m_Message = texts.TrueExitMessage;
                m_Quote = texts.TrueExitQuote;
                m_ShutDownPlayer = true;
                m_TrueEndingShutDown = true;
                break;
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            m_EnteredPortal = true;

            if (m_ShutDownPlayer) {
                GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealth>().ShutPlayerDown(false, true, m_TrueEndingShutDown, m_Message, m_Quote);
                return;
            } else {
                m_OnScreenInformation.Blackout(m_BlackoutDelayTime, m_BlackoutFadeinTime, m_BlackoutActiveTime, m_BlackoutFadeoutTime);
                m_OnScreenInformation.ShowMessage(m_Message, m_MessageDelayTime, m_MessageFadeinTime, m_MessageActiveTime, m_MessageFadeoutTime);
                m_OnScreenInformation.ShowSprite(m_Sprite, m_SpriteDelayTime, m_SpriteFadeinTime, m_SpriteActiveTime, m_SpriteFadeoutTime);
            }

            if (m_SetEnvironmentToRed)
                StartCoroutine(GameObject.FindGameObjectWithTag("FogSystem").GetComponent<FogSystem>().SetEnvironmentToRed());
            else
                GameObject.FindGameObjectWithTag("FogSystem").GetComponent<FogSystem>().SetToNormalEnvironment();

            StartCoroutine(SetPlayerNewPosition(0.1f));

        }
    }

    private IEnumerator SetPlayerNewPosition(float delay) {
        yield return new WaitForSeconds(delay);
        GameObject.FindGameObjectWithTag("Player").transform.position = m_SpawnTransform.transform.position;
        GameObject.FindGameObjectWithTag("Player").transform.rotation = m_SpawnTransform.transform.rotation;
    }


    public bool EnteredPortal() {
        return m_EnteredPortal;
    }

}
