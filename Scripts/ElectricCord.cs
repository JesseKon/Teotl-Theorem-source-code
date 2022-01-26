using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricCord : MonoBehaviour
{
    public uint[] electricityIdsNeeded;

    [ColorUsage(true, true)]
    public Color m_HDRColor;

    private Dictionary<uint, bool> electricityIdsSatisfied = new Dictionary<uint, bool>();


    private void Start() {
        gameObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");

        foreach (uint energyId in electricityIdsNeeded) {
            electricityIdsSatisfied.Add(energyId, false);
        }

        ActivateElectricityId(0);
    }


    public void ActivateElectricityId(uint energyId) {
        foreach (uint id in electricityIdsNeeded) {
            if (id == energyId)
                electricityIdsSatisfied[id] = true;
        }

        Dictionary<uint, bool>.ValueCollection values = electricityIdsSatisfied.Values;
        int numOfElectricityIdsSatisfied = 0;
        foreach(bool value in values) {
            if (value)
                ++numOfElectricityIdsSatisfied;
        }

        // Fully energized and full emission
        if (numOfElectricityIdsSatisfied == electricityIdsNeeded.Length) {
            gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", m_HDRColor);
        }

        // Partially energized and partial emission
        else if (numOfElectricityIdsSatisfied > 0) {
            gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", m_HDRColor * 0.45f);
        }
    }


    public void DisableElectricityId(uint electricityId) {
        foreach (uint id in electricityIdsNeeded) {
            if (id == electricityId)
                electricityIdsSatisfied[id] = false;
        }

        Dictionary<uint, bool>.ValueCollection values = electricityIdsSatisfied.Values;
        int numOfElectricityIdsSatisfied = 0;
        foreach (bool value in values) {
            if (value)
                ++numOfElectricityIdsSatisfied;
        }

        // Fully energized and full emission
        if (numOfElectricityIdsSatisfied == electricityIdsNeeded.Length) {
            gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", m_HDRColor);
        }

        // Partially energized and partial emission
        else if (numOfElectricityIdsSatisfied > 0) {
            gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", m_HDRColor * 0.45f);
        }

        // Not energized anymore
        else if (numOfElectricityIdsSatisfied == 0) {
            gameObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        }
    }
}
