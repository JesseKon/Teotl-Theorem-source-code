using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicHolder : MonoBehaviour
{
    public uint activateEnergyId;

    private bool m_ActionKeyPressed = false;
    private LayerMask m_LayerMask;

    private bool m_RelicHolderActivated = false;
    private GameObject m_RelicSprite;

    private Narrator m_Narrator;
    private Coroutine m_Coroutine = null;

    private AudioSource m_ErrorSound;
    private const float m_ErrorSoundCoolDown = 1.0f; // In seconds
    private float m_ErrorSoundTimer = 0.0f;

    private AudioSource m_InsertSound;

    private void Start() {
        m_LayerMask = LayerMask.GetMask("InteractableObject");
        m_RelicSprite = transform.GetChild(0).gameObject;
        m_RelicSprite.SetActive(false);

        m_ErrorSound = transform.GetChild(1).GetComponent<AudioSource>();
        m_InsertSound = transform.GetChild(2).GetComponent<AudioSource>();

        //m_Narrator = GameObject.FindGameObjectWithTag("Narrator").GetComponent<Narrator>();
    }


    void Update() {
        m_ActionKeyPressed = Input.GetKey(KeyCode.E) || Input.GetMouseButton(0);
    }


    private void FixedUpdate() {
        m_ErrorSoundTimer += Time.fixedDeltaTime;

        // In operating distance to the relic holder, try to activate it
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, GameManager.pickDistance, m_LayerMask)) {

            // Give relic holder a relic
            if (hit.collider.CompareTag("RelicHolder") && hit.collider.gameObject == gameObject) {

                if (m_ActionKeyPressed) {

                    // Activate relic
                    if (GameObject.FindGameObjectWithTag("RelicManager").GetComponent<RelicManager>().NumOfRelics() > 0 && !m_RelicHolderActivated) {
                        m_RelicHolderActivated = true;
                        m_RelicSprite.SetActive(true);
                        Debug.Log("Relic activated! activateEnergyId: " + activateEnergyId);
                        GameObject.FindGameObjectWithTag("RelicManager").GetComponent<RelicManager>().ConsumeRelic();
                        m_InsertSound.Play();
                        ActivateEnergyCordsWithThisId();
                        ActivateElectricDoorsWithThisId();
                        ActivateLightsWithThisId();
                    }
                    
                    else {

                        // No relics in inventory, inform player about that
                        if (!m_RelicHolderActivated) {
                            GameObject.Find("OnScreenInformation").GetComponent<OnScreenInformation>().ShowMessage(
                                GameObject.Find("MessagesAndNoteTexts").GetComponent<MessagesAndNoteTexts>().NoRelicsMessage,
                                0.0f, 1.0f, 2.0f, 1.0f
                            );

                            // Play error sound if it can't be activated
                            if (m_ErrorSoundTimer > m_ErrorSoundCoolDown) {
                                m_ErrorSound.Play();
                                m_ErrorSoundTimer = 0.0f;
                            }
                        }
                    }
                }
            }

        }
    }


    public bool IsActivated() {
        return m_RelicHolderActivated;
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
