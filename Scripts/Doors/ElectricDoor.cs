using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricDoor : MonoBehaviour
{
    public uint[] energyIdsNeeded;

    private Dictionary<uint, bool> energyIdsSatisfied = new Dictionary<uint, bool>();
    private bool m_AllEnergySatisfied = false;
    private float m_PosY = 0.0f;
    private bool m_HaltDownwardMovement = false;

    private const float m_cDoorMinOpenY = 0.0f;
    private const float m_cDoorMaxOpenY = 2.75f;

    private bool m_DoorComingDown = false;
    private bool m_ForceCloseDoor = false;


    private AudioSource m_HingeSound;
    private AudioSource m_DoorDragSound;

    private int m_NumOfColliders = 0;

    private float m_PlayerStuckTimer = 0.0f;
    private const float m_PlayerStuckTime = 5.5f;
    private bool m_PlayerStuck = false;
    private bool m_PlayerWasMoved = false;


    private void Start() {
        foreach (uint energyId in energyIdsNeeded) {
            energyIdsSatisfied.Add(energyId, false);
        }

        m_HingeSound = transform.GetChild(2).GetComponent<AudioSource>();
        m_DoorDragSound = transform.GetChild(3).GetComponent<AudioSource>();

        ActivateElectricityId(0);
    }


    private void FixedUpdate() {

        // Electrics on, door is opening
        if (m_AllEnergySatisfied && !m_ForceCloseDoor) {
            m_DoorComingDown = false;

            m_PosY += 1.0f * Time.fixedDeltaTime;
            m_PosY = Mathf.Clamp(m_PosY, m_cDoorMinOpenY, m_cDoorMaxOpenY);

            // Update audio
            if (m_PosY == m_cDoorMaxOpenY) {
                if (m_HingeSound.isPlaying) {
                    m_HingeSound.Pause();
                }

                if (m_DoorDragSound.isPlaying) {
                    m_DoorDragSound.Pause();
                }
            } else {
                if (!m_HingeSound.isPlaying) {
                    m_HingeSound.Play();
                }

                if (!m_DoorDragSound.isPlaying) {
                    m_DoorDragSound.Play();
                }
            }

        }

        // Electrics off, door is closing
        else {
            if (!m_HaltDownwardMovement) {
                m_DoorComingDown = true;
                m_PosY += -2.0f * Time.fixedDeltaTime;
                m_PosY = Mathf.Clamp(m_PosY, m_cDoorMinOpenY, m_cDoorMaxOpenY);

                // Update audio
                if (m_PosY < m_cDoorMinOpenY + 0.1f) {
                    if (m_HingeSound.isPlaying) {
                        m_HingeSound.Pause();
                    }

                    if (m_DoorDragSound.isPlaying) {
                        m_DoorDragSound.Pause();
                    }
                }
                
                else {
                    if (!m_HingeSound.isPlaying) {
                        m_HingeSound.Play();
                    }

                    if (!m_DoorDragSound.isPlaying) {
                        m_DoorDragSound.Play();
                    }
                }
            } else {
                m_DoorComingDown = false;

                if (m_HingeSound.isPlaying) {
                    m_HingeSound.Pause();
                }

                if (m_DoorDragSound.isPlaying) {
                    m_DoorDragSound.Pause();
                }
            }
        }

        transform.position = new Vector3(transform.position.x, m_PosY, transform.position.z);
    }



    public void ActivateElectricityId(uint energyId) {
        foreach (uint id in energyIdsNeeded) {
            if (id == energyId)
                energyIdsSatisfied[id] = true;
        }

        Dictionary<uint, bool>.ValueCollection values = energyIdsSatisfied.Values;
        int numOfEnergyIdsSatisfied = 0;
        foreach (bool value in values) {
            if (value)
                ++numOfEnergyIdsSatisfied;
        }

        m_AllEnergySatisfied = numOfEnergyIdsSatisfied == energyIdsNeeded.Length;
    }


    public void DisableElectricityId(uint energyId) {
        foreach (uint id in energyIdsNeeded) {
            if (id == energyId)
                energyIdsSatisfied[id] = false;
        }

        Dictionary<uint, bool>.ValueCollection values = energyIdsSatisfied.Values;
        int numOfEnergyIdsSatisfied = 0;
        foreach (bool value in values) {
            if (value)
                ++numOfEnergyIdsSatisfied;
        }

        m_AllEnergySatisfied = numOfEnergyIdsSatisfied == energyIdsNeeded.Length;
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
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllowPauseMenu = false;
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


    public void ForceCloseDoor() {
        m_ForceCloseDoor = true;
        GetComponent<BoxCollider>().enabled = false;
    }


    public bool IsOpen() {
        return m_AllEnergySatisfied;
    }
}
