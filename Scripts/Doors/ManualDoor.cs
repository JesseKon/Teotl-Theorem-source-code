using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManualDoor : MonoBehaviour
{
    public float m_DoorInitialOpenPercentage = 0.0f;
    public bool m_GoAutomaticallyDown = false;

    private const float m_cDoorMaxOpenY = 2.75f;
    private float m_InitialPosY;

    private float m_CurrentPosY;
    private float m_LastPosY;
    private bool m_HaltDownwardMovement;

    private bool m_DoorUnlocked;

    private bool m_DoorBeignAffected;
    private float m_ReleaseTimer = 0.0f;
    private float m_ReleaseTimerWhenNotAffected = 2.0f;

    private float m_DoorOpenPercentage;
    private float m_DoorOpenLastPercentage;

    private bool m_DoorMovingDown = false;

    private int m_NumOfColliders = 0;

    private float m_PlayerStuckTimer = 0.0f;
    private const float m_PlayerStuckTime = 5.5f;
    private bool m_PlayerStuck = false;
    private bool m_PlayerWasMoved = false;

    private AudioSource m_HingeSound;
    private float m_HingeSoundVolume = 0.0f;

    private AudioSource m_DoorDragSound;
    private float m_DoorDragSoundVolume = 0.0f;

    private AudioSource m_DistFx;

    private TextMeshPro m_TMPWarningMessage;


    private void Start() {
        m_InitialPosY = m_CurrentPosY = m_LastPosY = transform.position.y;
        m_HaltDownwardMovement = false;

        m_DoorUnlocked = false;
        m_DoorBeignAffected = false;

        m_DoorOpenPercentage = 0.0f;
        m_DoorOpenLastPercentage = 0.0f;

        m_HingeSound = transform.GetChild(2).GetComponent<AudioSource>();
        m_DoorDragSound = transform.GetChild(3).GetComponent<AudioSource>();
        m_DistFx = transform.GetChild(4).GetComponent<AudioSource>();

        m_TMPWarningMessage = GameObject.Find("DoorsWarningMessage").GetComponent<TextMeshPro>();
        m_TMPWarningMessage.color = new Color(m_TMPWarningMessage.color.r, m_TMPWarningMessage.color.g, m_TMPWarningMessage.color.b, 0.0f);
        m_TMPWarningMessage.text = GameObject.Find("MessagesAndNoteTexts").GetComponent<MessagesAndNoteTexts>().StuckAtDoor;

        DoorOpenPercentage(m_DoorInitialOpenPercentage);
    }


    private void Update() {
        m_HingeSound.volume = m_HingeSoundVolume;
        m_DoorDragSound.volume = m_DoorDragSoundVolume;
        UpdateSounds();
    }


    private void FixedUpdate() {
        if (!m_DoorBeignAffected) {
            m_ReleaseTimer += Time.deltaTime;
        } else {
            m_ReleaseTimer = 0.0f;
        }

        if (m_GoAutomaticallyDown && m_ReleaseTimer > m_ReleaseTimerWhenNotAffected) {
            DoorOpenPercentage(m_DoorOpenPercentage - 0.1f * Time.deltaTime);
        } else {
            m_DoorMovingDown = false;
        }

        m_DoorOpenLastPercentage = m_DoorOpenPercentage;
    }


    private void UpdateSounds() {

        // If player is affecting the door
        if (m_DoorBeignAffected /*&& !GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerInPauseMenu*/) {

            if (!m_HingeSound.isPlaying) {
                m_HingeSound.Play();
            }

            if (!m_DoorDragSound.isPlaying) {
                m_DoorDragSound.Play();
            }

        }
        
        // If door is coming down on its own
        else if (m_DoorMovingDown) {

            m_HingeSound.volume = 0.5f * Time.timeScale;
            m_DoorDragSound.volume = 0.5f * Time.timeScale;

            if (!m_HingeSound.isPlaying) {
                m_HingeSound.Play();
            }

            if (!m_DoorDragSound.isPlaying) {
                m_DoorDragSound.Play();
            }

        }
        
        // If door is halted or all the way down
        else {

            if (m_HingeSound.isPlaying) {
                m_HingeSound.Pause();
            }

            if (m_DoorDragSound.isPlaying) {
                m_DoorDragSound.Pause();
            }

        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="percent">Between 0.0f and 1.0f</param>
    public void DoorOpenPercentage(float percent) {
        m_CurrentPosY = transform.position.y;
        m_DoorOpenPercentage = Mathf.Clamp(percent, 0.0f, 1.0f);

        float posY = m_CurrentPosY;

        // Something preventing door to come all the way down
        if (m_HaltDownwardMovement && m_DoorOpenPercentage < m_DoorOpenLastPercentage) {
            m_DoorOpenPercentage = m_DoorOpenLastPercentage;
            m_DoorMovingDown = false;
        }
        
        // Door is coming down
        else {
            posY = m_InitialPosY + m_cDoorMaxOpenY * percent;
            m_DoorMovingDown = true;
        }

        // The door is fully shut down
        if (posY < m_InitialPosY) {
            posY = m_InitialPosY;
            m_DoorMovingDown = false;
        }

        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        m_LastPosY = m_CurrentPosY;
    }


    private void OnTriggerEnter(Collider other) {
        ++m_NumOfColliders;
        m_HaltDownwardMovement = true;
    }


    private void OnTriggerExit(Collider other) {
        --m_NumOfColliders;
        if (m_NumOfColliders == 0)
            m_HaltDownwardMovement = false;

        if (other.CompareTag("Player")) {
            m_PlayerStuckTimer = 0.0f;
            m_PlayerStuck = false;
            m_PlayerWasMoved = false;
        }

    }

    private void OnTriggerStay(Collider other) {

        // Player might be stuck at a door
        if (other.CompareTag("Player")) {
            if (!m_PlayerStuck) {
                Debug.Log("Player is stuck at door " + this);
                //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllowPauseMenu = false;
                m_PlayerStuck = true;
            }

            m_PlayerStuckTimer += Time.fixedDeltaTime;
        }

        // Show message and start playing dist fx after being stuck for certain time
        if (m_PlayerStuckTimer > 0.5f && !m_PlayerWasMoved) {
            GameObject.FindGameObjectWithTag("Player").transform.position -= transform.forward * 1.0f;
            Debug.Log("Player was moved from inside the door " + this);
            m_PlayerWasMoved = true;

        }

        // Kill player if stuck for too long
        if (m_PlayerStuckTimer > m_PlayerStuckTime) {
            GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealth>().ShutPlayerDown(false);
        }

    }


    public bool IsDoorUnlocked() {
        return m_DoorUnlocked;
    }


    public void SetDoorBeignAffected(bool affected) {
        m_DoorBeignAffected = affected;
    }


    public bool IsDoorBeignHalted() {
        return m_HaltDownwardMovement;
    }


    public bool IsDoorBeignAffected() {
        return m_DoorBeignAffected;
    }


    public float GetDoorOpenPercentage() {
        return m_DoorOpenPercentage;
    }


    public void SetDoorAudioVolume(float newVolume) {
        m_HingeSoundVolume = newVolume;
        m_DoorDragSoundVolume = newVolume;
    }

}
