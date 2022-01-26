using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraShader : MonoBehaviour
{
    public Shader m_ReplacementShader;

    private void Awake() {
        GetComponent<Camera>().SetReplacementShader(m_ReplacementShader, "");
    }

}
