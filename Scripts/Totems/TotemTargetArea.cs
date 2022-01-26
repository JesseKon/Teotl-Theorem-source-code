using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemTargetArea : MonoBehaviour
{
    public TotemType.Type acceptedTotemType;
    public uint activateEnergyId;

    [ColorUsage(true, true)]
    public Color m_HDRColor;

    private Totem m_ThisTotem = null;

    private Vector3 m_PositionWhenInactivated;
    private Vector3 m_PositionWhenActivated;

    public bool Activated {
        get { return m_Activated; }
    }
    private bool m_Activated = false;

    private AudioSource m_ClickSound;


    private void Start() {
        m_PositionWhenInactivated = transform.position;
        m_PositionWhenActivated = transform.position - Vector3.up * 0.053f;
        m_ClickSound = transform.GetChild(0).GetComponent<AudioSource>();
    }


    private void Update() {
        if (m_Activated) {
            transform.position = Vector3.Slerp(transform.position, m_PositionWhenActivated, 5.0f * Time.deltaTime);
        } else {
            transform.position = Vector3.Slerp(transform.position, m_PositionWhenInactivated, 5.0f * Time.deltaTime);
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Totem")) {
            m_Activated = true;

            // Play sound
            m_ClickSound.pitch = 1.0f;
            m_ClickSound.Play();

            // The totem is accepted
            if (other.gameObject.GetComponent<Totem>().totemType == acceptedTotemType) {

                // Start emission
                gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", m_HDRColor);

                // Activate the totem's energy emission
                m_ThisTotem = other.gameObject.GetComponent<Totem>();
                m_ThisTotem.ActivateEnergyEmission();

                GameManager.energyIdsActive.Add(activateEnergyId);

                ActivateEnergyCordsWithThisId();
                ActivateElectricDoorsWithThisId();
                ActivateLightsWithThisId();

                Debug.Log(
                    "Totem activated. [" + acceptedTotemType + "] \\\\ " + 
                    "activateEnergyId ( " + activateEnergyId + " )"
                );

            }
            
            // The totem was not accepted
            else {
                Debug.Log(
                    "This totem target area doesn't accept this totem. [given: " +
                    other.gameObject.GetComponent<Totem>().totemType + ", accepted: " + acceptedTotemType + "]"
                );
            }
        }
    }


    private void OnTriggerExit(Collider other) {

        if (other.gameObject.CompareTag("Totem")) {
            m_Activated = false;

            // Play sound
            m_ClickSound.pitch = 0.8f;
            m_ClickSound.Play();

            if (other.gameObject.GetComponent<Totem>().totemType == acceptedTotemType) {

                // End emission
                gameObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");

                // The totem was active and now will be disabled
                if (m_ThisTotem) {
                    m_ThisTotem.DisableEnergyEmission();
                    GameManager.energyIdsActive.Remove(activateEnergyId);

                    DisableEnergyCordsWithThisid();
                    DisableElectricDoorsWithThisId();
                    DisableLightsWithThisId();

                    m_ThisTotem = null;

                    Debug.Log(
                        "Totem deactivated. [" + acceptedTotemType + "] \\\\ " +
                        "activateEnergyId ( " + activateEnergyId + " )"
                    );
                }
            }
        }

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


    private void DisableEnergyCordsWithThisid() {
        GameObject[] electricCords = GameObject.FindGameObjectsWithTag("ElectricCord");
        foreach (GameObject electricCord in electricCords) {
            electricCord.GetComponent<ElectricCord>().DisableElectricityId(activateEnergyId);
        }
    }


    private void DisableElectricDoorsWithThisId() {
        GameObject[] electricDoors = GameObject.FindGameObjectsWithTag("ElectricDoor");
        foreach (GameObject electricDoor in electricDoors) {
            electricDoor.GetComponent<ElectricDoor>().DisableElectricityId(activateEnergyId);
        }
    }


    private void DisableLightsWithThisId() {
        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");
        foreach (GameObject light in lights) {
            light.GetComponent<LightSystem>().DisableElectricityId(activateEnergyId);
        }
    }

}
