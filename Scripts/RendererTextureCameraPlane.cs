using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererTextureCameraPlane : MonoBehaviour
{
    [SerializeField] public float DizzinessSeverity {
        get { return m_DizziessSeverity; }
        set {
            m_DizziessSeverity = Mathf.Clamp(value, 0.0f, 0.10f);
            GetComponent<MeshRenderer>().material.SetFloat("_DizzinessSeverity", m_DizziessSeverity);
        }
    }
    private float m_DizziessSeverity = 0.0f;

    [SerializeField]
    public float SwayingSpeed {
        get { return m_SwayingSpeed; }
        set {
            m_SwayingSpeed = Mathf.Clamp(value, 0.0005f, 0.8f);
            GetComponent<MeshRenderer>().material.SetFloat("_SwayingSpeed", m_SwayingSpeed);
        }
    }
    private float m_SwayingSpeed = 0.0f;

    [SerializeField]
    public float SwayingSeverity {
        get { return m_SwayingSeverity; }
        set {
            m_SwayingSeverity = Mathf.Clamp(value, 0.0025f, 0.05f);
            GetComponent<MeshRenderer>().material.SetFloat("_SwayingSeverity", m_SwayingSeverity);
        }
    }
    private float m_SwayingSeverity = 0.0f;


}
