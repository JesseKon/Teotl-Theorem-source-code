using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualDoorHandler : MonoBehaviour
{
    // The door this handler will affect
    public GameObject affectedDoor;

    // Set this to the same for all handlers that are affecting the same door
    public uint doorHandlerId;

    private List<GameObject> m_DoorHandlersWithSameId = new List<GameObject>();

    public bool rotationReversed = false;


    public float minRotation = 0.0f;
    public float maxRotation = 720.0f;
    public float startRotation = 0.0f;

    private bool m_ActionKeyPressed = false;
    private bool m_ActionOngoing = false;
    private Transform m_DoorObjectTransform;

    private Vector2 m_CurrentPos;
    private Vector2 m_LastPos;
    private float m_CurrentRotation;

    private LayerMask m_LayerMask;

    private bool m_ActionKeyReleased = false;
    private bool m_DoorValveActive = false;
    [HideInInspector] public bool stealMouseControl = false;

    private const float m_MaxRotationSpeed = 2.5f;
    private Vector3 m_InitialRotation;


    private void Start() {
        if (!affectedDoor) {
            Debug.Log("No affected door for " + gameObject.name + ".");
            return;
        }

        m_DoorObjectTransform = gameObject.GetComponentInChildren<Transform>();

        m_CurrentPos = Vector2.zero;
        m_LastPos = Vector2.zero;
        m_CurrentRotation = 0.0f;

        GameObject[] doorHandlers = GameObject.FindGameObjectsWithTag("ManualDoorHandler");
        foreach(GameObject doorHandler in doorHandlers) {
            if (doorHandler.GetComponent<ManualDoorHandler>().doorHandlerId == doorHandlerId)
                m_DoorHandlersWithSameId.Add(doorHandler);
        }

        m_LayerMask = LayerMask.GetMask("InteractableObject", "WorldCollider");
        m_InitialRotation = transform.rotation.eulerAngles;

    }


    private void Update() {
        m_ActionKeyPressed = Input.GetMouseButton(0);

        if (Input.GetMouseButtonUp(0))
            m_ActionKeyReleased = true;

        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerInPauseMenu)
            m_ActionKeyReleased = true;
    }


    private void FixedUpdate() {
        if (!affectedDoor)
            return;

        // In operating distance to the valve, trying to open it
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, GameManager.pickDistance, m_LayerMask)) {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("WorldCollider"))
                DoorOpenerFunctionality(hit);
        } 
        
        // Ventured too far away from the valve, give up its control
        else {
            if (m_DoorValveActive) {
                m_DoorValveActive = false;
                stealMouseControl = false;
                CursorManager.ShowCursor();
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        // Not this nor any other door handler are affecting this door
        if (!affectedDoor.GetComponent<ManualDoor>().IsDoorBeignAffected() || !m_ActionOngoing) {
            m_CurrentRotation = affectedDoor.GetComponent<ManualDoor>().GetDoorOpenPercentage() * maxRotation;
            m_DoorObjectTransform.eulerAngles = new Vector3(m_InitialRotation.x, m_InitialRotation.y, -m_CurrentRotation);
        }
    }


    private void DoorOpenerFunctionality(RaycastHit hit) {

            // The door valve was just activated
            if (hit.collider.CompareTag("ManualDoorHandlerValve") && hit.collider.gameObject == gameObject.transform.Find("ManualDoorHandlerValve").gameObject) {
                if (m_ActionKeyPressed && !m_DoorValveActive) {
                    m_DoorValveActive = true;
                    stealMouseControl = true;

                    CursorManager.HideCursor();
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().LockMovementControls = true;

                    Cursor.lockState = CursorLockMode.None;
                }
            }


        // The door valve was just released
        if (m_DoorValveActive && m_ActionKeyReleased) {
            m_DoorValveActive = false;
            m_ActionKeyReleased = false;
            stealMouseControl = false;

            CursorManager.ShowCursor();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().LockMovementControls = false;

            Cursor.lockState = CursorLockMode.Locked;
        }

        // When the door valve is active and beign affected
        if (m_DoorValveActive) {

            // The first frame when the door valve is beign turned
            if (!m_ActionOngoing) {
                m_LastPos = Input.mousePosition - GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(transform.position);
                m_CurrentPos = m_LastPos;
                affectedDoor.GetComponent<ManualDoor>().SetDoorBeignAffected(true);
                m_ActionOngoing = true;
            }

            m_CurrentPos = Input.mousePosition - GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(transform.position);


            float targetAngle = (Mathf.Atan2(m_LastPos.y, m_LastPos.x) - Mathf.Atan2(m_CurrentPos.y, m_CurrentPos.x))
                                * Mathf.Rad2Deg * m_MaxRotationSpeed;

            if (rotationReversed)
                targetAngle = -targetAngle;

            if (targetAngle < 180.0f)
                targetAngle += 360.0f;
            if (targetAngle > 180.0f)
                targetAngle -= 360.0f;

            float targetRotation = Mathf.Min(Mathf.Max(-m_MaxRotationSpeed, targetAngle), m_MaxRotationSpeed);

            if (affectedDoor.GetComponent<ManualDoor>().IsDoorBeignHalted()) {
                if (targetRotation < 0)
                    targetRotation = 0;
                m_CurrentRotation += targetRotation;
            } else {
                m_CurrentRotation += targetRotation;
            }

            // Update sound volumes when handler is rotating
            const float margin = 10.0f;
            if (m_CurrentRotation > minRotation + margin && m_CurrentRotation < maxRotation - margin)
                affectedDoor.GetComponent<ManualDoor>().SetDoorAudioVolume(Mathf.Clamp(Mathf.Abs(targetRotation) / m_MaxRotationSpeed, 0.0f, 1.0f));
            else
                affectedDoor.GetComponent<ManualDoor>().SetDoorAudioVolume(0.0f);


            m_CurrentRotation = Mathf.Clamp(m_CurrentRotation, minRotation, maxRotation);
            m_DoorObjectTransform.eulerAngles = new Vector3(m_InitialRotation.x, m_InitialRotation.y, -m_CurrentRotation);
            affectedDoor.GetComponent<ManualDoor>().DoorOpenPercentage(m_CurrentRotation / maxRotation);

            m_LastPos = m_CurrentPos;
        } 

        // When the door valve is released
        else {
            m_ActionOngoing = false;

            // In case there are multiple door handlers connected to the same door
            int handlersAffectingDoor = 0;
            foreach (GameObject doorHandler in m_DoorHandlersWithSameId) {
                if (doorHandler.GetComponent<ManualDoorHandler>().m_ActionOngoing)
                    ++handlersAffectingDoor;
            }

            // Send info to the door if no handlers are affecting it
            if (handlersAffectingDoor == 0)
                affectedDoor.GetComponent<ManualDoor>().SetDoorBeignAffected(false);
        }

    }

}
