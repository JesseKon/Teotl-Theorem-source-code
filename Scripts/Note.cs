using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Note : MonoBehaviour
{
    private PlayerMovement m_PlayerMovement;

    private LayerMask m_LayerMask;
    private bool m_NoteCanBePickedUp;
    private bool m_NotePickedUp;
    private Vector3 m_InitialLocation;
    private Quaternion m_InitialRotation;

    private bool m_IsNoteSeen;

    private AudioSource m_PickUpSound;
    private AudioSource m_PutBackSound;

    private MeshRenderer m_MeshRenderer;

    private GameObject m_NoteShape;
    private GameObject m_NoteShapeBack;
    private GameObject m_NoteText;

    readonly Color m_Color1 = new Color(1.0f, 1.0f, 1.0f, 1.0f) * 0.5f;
    readonly Color m_Color2 = new Color(1.0f, 1.0f, 1.0f, 1.0f) * 1.0f;
    Color m_NoteColor;

    Quaternion m_TargetRotation = Quaternion.identity;


    private void Start() {
        m_PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        m_LayerMask = LayerMask.GetMask("InteractableObject", "WorldCollider");
        m_NoteCanBePickedUp = false;
        m_NotePickedUp = false;
        m_IsNoteSeen = false;
        m_InitialLocation = transform.position;
        m_InitialRotation = transform.rotation;

        m_PickUpSound = transform.GetChild(2).GetChild(0).GetComponent<AudioSource>();
        m_PutBackSound = transform.GetChild(2).GetChild(1).GetComponent<AudioSource>();

        m_MeshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        m_MeshRenderer.material.EnableKeyword("_EMISSION");

        m_NoteShape = transform.GetChild(0).gameObject;
        m_NoteShapeBack = transform.GetChild(1).gameObject;
        m_NoteText = transform.GetChild(2).gameObject;
    }


    private void LateUpdate() {
        if (!m_IsNoteSeen)
            m_NoteColor = Color.Lerp(m_Color1, m_Color2, Mathf.Sin(Time.realtimeSinceStartup * 3.0f));
        else
            m_NoteColor = Color.white * 0.5f;

        if (!GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerInPauseMenu) {

            // The note is picked up in this frame
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) && m_NoteCanBePickedUp && !m_NotePickedUp && !m_PlayerMovement.Jumping) {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().LockMovementControls = true;
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllowPauseMenu = false;
                m_NotePickedUp = true;

                // TODO: not working properly...
                m_TargetRotation =
                    Quaternion.LookRotation(transform.position - (Camera.main.transform.position - Camera.main.transform.forward)) *
                    Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x + 270.0f, transform.rotation.eulerAngles.y, 180.0f));

                m_NoteShape.layer = 10; // Put note to NoteCamera layer
                m_NoteShapeBack.layer = 10;
                m_NoteText.layer = 10;

                CursorManager.HideCursor();
                m_PickUpSound.Play();
            }


            // The note is released (put back) in this frame
            else if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)) && m_NotePickedUp) {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().LockMovementControls = false;
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllowPauseMenu = true;

                m_NotePickedUp = false;

                //transform.position = m_InitialLocation;
                transform.rotation = m_InitialRotation;

                m_NoteShape.layer = 0;  // Put note back to default layer
                m_NoteShapeBack.layer = 0;
                m_NoteText.layer = 0;

                CursorManager.ShowCursor();
                m_PutBackSound.Play();
            }
        }


        // When note is picked up
        if (m_NotePickedUp) {
            transform.LookAt(Camera.main.transform.position - Camera.main.transform.forward);
            transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x + 270.0f, transform.rotation.eulerAngles.y, 180.0f);
            //transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;

            transform.position = Vector3.Lerp(transform.position, Camera.main.transform.position + Camera.main.transform.forward * 0.5f, Time.deltaTime * 20.0f);
            //transform.rotation = Quaternion.Lerp(transform.rotation, m_TargetRotation, Time.deltaTime * 20.0f);

            m_NoteColor = Color.white * 0.5f;
            m_IsNoteSeen = true;
        }

        // When note is not picked up
        else {
            transform.position = m_InitialLocation;
            //transform.position = Vector3.Lerp(transform.position, m_InitialLocation, Time.deltaTime * 20.0f);
            //transform.rotation = Quaternion.Lerp(transform.rotation, m_InitialRotation, Time.deltaTime * 20.0f);
        }

        m_MeshRenderer.material.SetColor("_EmissionColor", m_NoteColor);
    }


    private void FixedUpdate() {

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, GameManager.pickDistance, m_LayerMask)) {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("WorldCollider")) {
                m_NoteCanBePickedUp = false;
                return;
            }

            if (hit.collider.gameObject.CompareTag("Note") && hit.collider.gameObject == gameObject) {
                m_NoteCanBePickedUp = true;
            } else {
                m_NoteCanBePickedUp = false;
            }
        } else {
            m_NoteCanBePickedUp = false;
        }
    }


    public bool IsNotePickedUp() {
        return m_NotePickedUp;
    }


    public bool IsNoteSeen() {
        return m_IsNoteSeen;
    }

}
