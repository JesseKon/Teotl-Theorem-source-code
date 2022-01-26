using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    public TotemType.Type totemType;

    [ColorUsage(true, true)]
    public Color m_HDRColor;

    private bool m_Grabbable = false;
    private bool m_Grabbed = false;
    private bool m_ActionButtonIsPressed = false;
    private bool m_ObjectWasReleased = false;
    private bool m_ObjectWasThrown = false;
    private bool m_ObjectBeignThrown = false;

    private LayerMask m_LayerMask;
    private Vector3 m_LastPos;
    private Vector3 m_NewPos;
    private Vector3 m_CubeLastPos;
    private Vector3 m_CubeNewPos;
    private Vector3 m_CubeTargetPos;
    private Vector3 m_CubeTargetForce;

    private float m_Distance;
    private bool m_BeignDragged;

    private Vector3 m_Offset = Vector3.zero;

    private AudioSource m_DragBeginSound;
    private AudioSource m_DragLoopingSound;


    private void OnEnable() {
        transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
    }


    private void Start() {
        m_LayerMask = LayerMask.GetMask("InteractableObject", "WorldCollider");
        m_Distance = 0.0f;

        m_DragBeginSound = transform.GetChild(1).GetChild(0).GetComponent<AudioSource>();
        m_DragLoopingSound = transform.GetChild(1).GetChild(1).GetComponent<AudioSource>();
        m_DragLoopingSound.volume = 0.0f;
    }


    private void Update() {
        m_ActionButtonIsPressed = Input.GetMouseButton(0);
        if (Input.GetMouseButtonUp(0)) {
            m_ObjectBeignThrown = false;
            if (m_Grabbed)
                m_ObjectWasReleased = true;
        }

        if (Input.GetMouseButtonDown(1) && m_Grabbed)
            m_ObjectWasThrown = true;

        m_BeignDragged = m_Grabbed;
    }


    private void FixedUpdate() {
        m_NewPos = transform.position;
        m_CubeNewPos = Camera.main.transform.position + Camera.main.transform.forward * m_Distance;

        // Can object be grabbed?
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, GameManager.pickDistance, m_LayerMask)) {
            if (hit.collider.CompareTag("Totem") && hit.collider.gameObject == gameObject && !m_ObjectBeignThrown && GameManager.HowManyObjectsBeignAffected() == 0) {
                if (hit.collider.gameObject.layer != LayerMask.NameToLayer("WorldCollider"))
                    m_Grabbable = true;
                else
                    m_Grabbable = false;
            } else {
                m_Grabbable = false;
            }
        } else {
            m_Grabbable = false;
        }

        // Called when the object was just grabbed
        if (!m_Grabbed && m_Grabbable && m_ActionButtonIsPressed && !m_ObjectBeignThrown) {
            CursorManager.HideCursor();
            m_Grabbed = true;
            m_Distance = Vector3.Distance(Camera.main.transform.position, transform.position);
            m_Offset = (Camera.main.transform.position + Camera.main.transform.forward * m_Distance) - transform.position;
            m_Offset.y = 0.0f;

            GameManager.BeginObjectBeignAffected();

            m_DragLoopingSound.Play();
        }

        if (m_Grabbed) {
            m_CubeTargetPos = Camera.main.transform.position + Camera.main.transform.forward * m_Distance - m_Offset;
            m_CubeTargetPos.y = transform.localScale.y * 0.5f;

            m_CubeTargetForce = (m_CubeTargetPos - transform.position) * 12.0f;
            m_CubeTargetForce = Vector3.ClampMagnitude(m_CubeTargetForce, 1.0f);
            GetComponent<Rigidbody>().AddForce(m_CubeTargetForce, ForceMode.VelocityChange);

            float volume = Mathf.Clamp(Vector3.Magnitude(m_NewPos - m_LastPos) * 25.0f, 0.0f, 1.0f);
            m_DragLoopingSound.volume = volume * 0.5f;


            // Object is just released
            if (m_ObjectWasReleased) {
                m_Grabbed = false;
                m_ObjectWasReleased = false;
                m_ObjectWasThrown = false;

                // Apply force to the direction where the object was released
                float force = 1.0f / Time.fixedDeltaTime;
                GetComponent<Rigidbody>().AddForce((m_NewPos - m_LastPos) * force, ForceMode.Impulse);

                m_DragLoopingSound.Stop();

                CursorManager.ShowCursor();
                GameManager.EndObjectBeignAffected();
            }


            //// Object is thrown
            //if (m_ObjectWasThrown) {
            //    m_Grabbed = false;
            //    m_ObjectWasReleased = false;
            //    m_ObjectWasThrown = false;
            //    m_ObjectBeignThrown = true;

            //    // Apply throw force mostly forward
            //    float force = 1.0f / Time.fixedDeltaTime;
            //    float forwardForce = 20.0f;
            //    GetComponent<Rigidbody>().AddForce(
            //        ((m_NewPos - m_LastPos) * force * 0.05f + Camera.main.transform.forward) * forwardForce, ForceMode.Impulse);

            //    CursorManager.ShowCursor();
            //    GameManager.SubObjectBeignAffected();
            //}

        }

        m_LastPos = m_NewPos;
        m_CubeLastPos = m_CubeNewPos;
    }


    public void ActivateEnergyEmission() {
        gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", m_HDRColor);
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();
    }


    public void DisableEnergyEmission() {
        gameObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
    }


    public bool IsBeignDragged() {
        return m_BeignDragged;
    }


    public float DragDistance() {
        return Vector3.Distance(m_CubeTargetPos - transform.position, Vector3.zero);
    }

}
