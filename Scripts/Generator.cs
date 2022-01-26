using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public uint activateEnergyId;

    private GameObject m_Base;
    private GameObject m_Rotator;

    private LayerMask m_LayerMask;

    private bool m_ActionKeyPressed = false;
    private bool m_ActionKeyPressedLastFrame = false;
    private bool m_ActionJustStarted = false;

    private float m_ActivationTimer = 0.0f;
    private const float m_cActivationTime = 1.5f;

    private bool m_SlowGeneratorDown = false;
    private float m_RotatingSpeed = 0.0f;
    private float m_StartingRotatingSpeed = 0.0f;
    private const float m_cMaxRotationSpeed = 500.0f;

    private AudioSource m_ErrorSound;
    private const float m_ErrorSoundCoolDown = 1.0f; // In seconds
    private float m_ErrorSoundTimer = 0.0f;

    private bool m_GeneratorActivated = false;

    static private bool m_AtLeastOneGeneratorActivated = false;


    private void Start() {
        m_Base = transform.GetChild(0).gameObject;
        m_Rotator = transform.GetChild(1).gameObject;
        m_LayerMask = LayerMask.GetMask("InteractableObject", "WorldCollider");

        transform.GetChild(0).GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        transform.GetChild(1).GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
        transform.GetChild(3).GetComponent<AudioSource>().Play();

        m_ErrorSound = transform.GetChild(4).GetComponent<AudioSource>();
    }


    private void Update() {
        m_ActionKeyPressed = Input.GetKey(KeyCode.E) || Input.GetMouseButton(0);
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            m_ActionKeyPressedLastFrame = true;

        // Generator was just activated
        if (m_ActivationTimer >= m_cActivationTime && !m_GeneratorActivated) {
            ActivateGenerator();
        }

        // Slow down generator rotating speed if it is not affected
        if (!m_GeneratorActivated && (!m_ActionKeyPressed || m_SlowGeneratorDown)) {
            m_RotatingSpeed = Mathf.Lerp(m_RotatingSpeed, 0.0f, Time.deltaTime * 10.0f);
        }

        // Update rotator and sounds
        m_Rotator.transform.Rotate(Vector3.up * Time.deltaTime * m_RotatingSpeed);
        gameObject.transform.GetChild(3).GetComponent<AudioSource>().volume = Mathf.Lerp(0.0f, 1.0f, m_RotatingSpeed / m_cMaxRotationSpeed);
        gameObject.transform.GetChild(3).GetComponent<AudioSource>().pitch = Mathf.Lerp(0.25f, 1.0f, m_RotatingSpeed / m_cMaxRotationSpeed);
    }


    private void FixedUpdate() {
        m_ErrorSoundTimer += Time.fixedDeltaTime;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, GameManager.pickDistance, m_LayerMask) && m_ActionKeyPressedLastFrame) {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("WorldCollider"))
                return;

            // In operating distance to the generator
            if (hit.collider.CompareTag("Generator") && hit.collider.gameObject == gameObject) {
                if (m_ActionKeyPressed) {

                    // Make sure player has shards
                    if (GameObject.FindGameObjectWithTag("ShardManager").GetComponent<ShardManager>().NumOfShards() > 0) {
                        m_SlowGeneratorDown = false;

                        if (m_ActionJustStarted) {
                            m_StartingRotatingSpeed = m_RotatingSpeed;
                            m_ActionJustStarted = false;
                        }

                        m_ActivationTimer += Time.fixedDeltaTime;
                        m_RotatingSpeed = Mathf.Lerp(m_StartingRotatingSpeed, m_cMaxRotationSpeed, m_ActivationTimer / m_cActivationTime);
                    }

                    // If player has no shards
                    else {

                        // Show message if the generator isn't active
                        if (!m_GeneratorActivated) {

                            // Play error sound if it can't be activated
                            if (m_ErrorSoundTimer > m_ErrorSoundCoolDown) {
                                m_ErrorSound.Play();
                                m_ErrorSoundTimer = 0.0f;
                            }

                            if (!m_AtLeastOneGeneratorActivated)
                                GameObject.Find("OnScreenInformation").GetComponent<OnScreenInformation>().ShowMessage(
                                    GameObject.Find("MessagesAndNoteTexts").GetComponent<MessagesAndNoteTexts>().NoShardsFirstMessage,
                                    0.0f, 1.0f, 2.0f, 1.0f
                                );
                            else
                                GameObject.Find("OnScreenInformation").GetComponent<OnScreenInformation>().ShowMessage(
                                    GameObject.Find("MessagesAndNoteTexts").GetComponent<MessagesAndNoteTexts>().NoShardsSecondMessage,
                                    0.0f, 1.0f, 2.0f, 1.0f
                                );
                        }

                    }
                }

                else {
                    m_ActivationTimer = 0.0f;
                    m_ActionJustStarted = true;
                    
                }
            }
            
            else {
                m_ActivationTimer = 0.0f;
                m_ActionJustStarted = true;
                
            }
        }

        else {
            m_ActivationTimer = 0.0f;
            m_ActionJustStarted = true;
            m_ActionKeyPressedLastFrame = false;
            m_SlowGeneratorDown = true;
        }

    }


    private void ActivateGenerator() {
        m_GeneratorActivated = true;
        m_AtLeastOneGeneratorActivated = true;

        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        gameObject.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
        gameObject.transform.GetChild(3).GetComponent<AudioSource>().Play();
        GameObject.FindGameObjectWithTag("ShardManager").GetComponent<ShardManager>().ConsumeShard();

        ActivateEnergyCordsWithThisId();
        ActivateElectricDoorsWithThisId();
        ActivateLightsWithThisId();

        Debug.Log("Generator activated! activateEnergyId: " + activateEnergyId);
    }


    public bool IsGeneratorActivated() {
        return m_GeneratorActivated;
    }


    private void ActivateEnergyCordsWithThisId() {
        GameObject[] electricCords = GameObject.FindGameObjectsWithTag("ElectricCord");
        foreach (GameObject electricCord in electricCords) {
            electricCord.GetComponent<ElectricCord>().ActivateElectricityId(activateEnergyId);
        }
    }


    private void ActivateElectricDoorsWithThisId() {
        GameObject[] electricDoors = GameObject.FindGameObjectsWithTag("ElectricDoor");
        foreach (GameObject electricDoor in electricDoors) {
            electricDoor.GetComponent<ElectricDoor>().ActivateElectricityId(activateEnergyId);
        }
    }


    private void ActivateLightsWithThisId() {
        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");
        foreach (GameObject light in lights) {
            light.GetComponent<LightSystem>().ActivateElectricityId(activateEnergyId);
        }
    }

}
