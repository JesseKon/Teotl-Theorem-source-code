using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSystem : MonoBehaviour
{

    public uint[] electricityIdsNeeded;

    private Dictionary<uint, bool> m_ElectricityIdsSatisfied = new Dictionary<uint, bool>();

    const float m_LightIntensity = 10.0f;


    private void Start() {
        foreach (uint energyId in electricityIdsNeeded) {
            m_ElectricityIdsSatisfied.Add(energyId, false);
        }

        ActivateElectricityId(0);
    }


    public void ActivateElectricityId(uint energyId) {
        foreach (uint id in electricityIdsNeeded) {
            if (id == energyId)
                m_ElectricityIdsSatisfied[id] = true;
        }

        Dictionary<uint, bool>.ValueCollection values = m_ElectricityIdsSatisfied.Values;
        int numOfElectricityIdsSatisfied = 0;
        foreach (bool value in values) {
            if (value)
                ++numOfElectricityIdsSatisfied;
        }

        // Fully energized and full emission
        if (numOfElectricityIdsSatisfied == electricityIdsNeeded.Length) {
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.green * m_LightIntensity);
        }

        // Partially energized and partial emission
        else if (numOfElectricityIdsSatisfied > 0) {
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.yellow * m_LightIntensity);
        }
    }


    public void DisableElectricityId(uint electricityId) {
        foreach (uint id in electricityIdsNeeded) {
            if (id == electricityId)
                m_ElectricityIdsSatisfied[id] = false;
        }

        Dictionary<uint, bool>.ValueCollection values = m_ElectricityIdsSatisfied.Values;
        int numOfElectricityIdsSatisfied = 0;
        foreach (bool value in values) {
            if (value)
                ++numOfElectricityIdsSatisfied;
        }

        // Fully energized and full emission
        if (numOfElectricityIdsSatisfied == electricityIdsNeeded.Length) {
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.green * m_LightIntensity);
        }

        // Partially energized and partial emission
        else if (numOfElectricityIdsSatisfied > 0) {
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.yellow * m_LightIntensity);
        }

        // Not energized anymore
        else if (numOfElectricityIdsSatisfied == 0) {
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red * m_LightIntensity);
        }
    }
}
