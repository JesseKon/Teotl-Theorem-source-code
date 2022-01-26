using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableCube : MonoBehaviour
{
    private const float m_Drag = 1.5f;
    private const float m_DragWhenGrabbed = 20.0f;
    private const float m_AngularDrag = 1.5f;
    private const float m_AngularDragWhenGrabbed = 20.0f;

    private bool m_Grabbable = false;
    private bool m_Grabbed = false;
    private bool m_ActionButtonPressed = false;
    private bool m_ActionButtonPressedLastFrame = false;

    private bool m_ObjectWasReleased = false;
    private bool m_ObjectWasThrown = false;
    private bool m_ObjectBeignThrown = false;
    private bool m_ObjectBeingRotated = false;

    private LayerMask m_LayerMask;

    private Vector3 m_CubeTargetPos;
    private Vector3 m_CubeLastTargetPos;
    //private Vector3 m_CubeTargetRot;

    private float m_Distance;
    private const float m_DistanceChangeSpeed = 0.2f;
    private const float m_MinDistance = 1.5f;
    private const float m_MaxDistance = 3.2f;

    private int m_CollidingObjects = 0;

    [HideInInspector]
    public bool stealMouseControl = false;

    private PIDControllerVector3 m_MovePIDController;
    //private PIDControllerVector3Rotation m_RotationPIDController;

    private AudioSource[] m_BoxHittingSounds;



    private void Start() {
        m_LayerMask = LayerMask.GetMask("InteractableObject", "WorldCollider");
        //m_LastPos = m_NewPos = transform.position;
        m_Distance = 0.0f;
        GetComponent<Rigidbody>().drag = m_Drag;
        GetComponent<Rigidbody>().angularDrag = m_AngularDrag;

        m_MovePIDController = new PIDControllerVector3(-Vector3.one * 20.0f, Vector3.one * 20.0f, 12.0f, 0.0f, 0.2f);
        //m_RotationPIDController = new PIDControllerVector3Rotation(-Vector3.one * 1.0f, Vector3.one * 1.0f, 20.0f, 0.1f, 0.001f);

        //m_InitialQuaternion = Quaternion.identity;

        m_BoxHittingSounds = GetComponentsInChildren<AudioSource>();
    }


    private void Update() {

        m_ActionButtonPressed = Input.GetMouseButton(0);
        if (Input.GetMouseButtonDown(0))
            m_ActionButtonPressedLastFrame = true;

        if (Input.GetMouseButtonUp(0)) {
            m_ObjectBeignThrown = false;
            if (m_Grabbed)
                m_ObjectWasReleased = true;
        }

        if (Input.GetMouseButton(1) && m_Grabbed) {
            m_ObjectWasThrown = true;
        }

        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerInPauseMenu)
            m_ObjectWasReleased = true;

        m_ObjectBeingRotated = m_Grabbed && Input.GetKey(KeyCode.R);
        stealMouseControl = m_ObjectBeingRotated;

        // Cube's distance from player, changed using mouse scroll TODO: use PID controller
        m_Distance = Mathf.Clamp(m_Distance + Input.mouseScrollDelta.y * m_DistanceChangeSpeed, m_MinDistance, m_MaxDistance);
    }


    private void FixedUpdate() {

        // Get target position for the cube
        m_CubeTargetPos = Camera.main.transform.position + Camera.main.transform.forward * m_Distance;

        // Can object be grabbed?
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, GameManager.pickDistance, m_LayerMask) && m_ActionButtonPressedLastFrame) {
            if (hit.collider.CompareTag("PickableCube") && hit.collider.gameObject == gameObject && !m_ObjectBeignThrown && GameManager.HowManyObjectsBeignAffected() == 0) {
                if (hit.collider.gameObject.layer != LayerMask.NameToLayer("WorldCollider"))
                    m_Grabbable = true;
                else
                    m_Grabbable = false;
            } else {
                m_Grabbable = false;
            }
        } else {
            m_Grabbable = false;
            m_ActionButtonPressedLastFrame = false;
        }


        // Is player standing on the cube?
        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit _hit, 1.0f)) {
            if (_hit.collider.gameObject.name == "PlayerFeet") {
                m_Grabbable = false;
                if (m_Grabbed)
                    m_ObjectWasReleased = true;
            }
        }


        // Called when the object was just grabbed
        if (!m_Grabbed && m_Grabbable && m_ActionButtonPressed && !m_ObjectBeignThrown) {
            GetComponent<Rigidbody>().drag = m_DragWhenGrabbed;
            GetComponent<Rigidbody>().angularDrag = m_AngularDragWhenGrabbed;

            m_Grabbed = true;
            CursorManager.HideCursor();
            m_Distance = Vector3.Distance(Camera.main.transform.position, transform.position);

            //m_InitialQuaternion = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
            //Debug.Log(m_InitialQuaternion.eulerAngles);

            GameManager.BeginObjectBeignAffected();
        }


        // When object is in hand and beign rotated
        if (m_ObjectBeingRotated) {
            GetComponent<Rigidbody>().AddTorque(
               -Camera.main.transform.up * Input.GetAxis("Mouse X") * 13.0f +
                Camera.main.transform.right * Input.GetAxis("Mouse Y") * 13.0f,
                ForceMode.VelocityChange
            );
        }

        // When object is grabbed
        if (m_Grabbed) {

            // Drop cube if it's too far away from player
            if (Vector3.Distance(Camera.main.transform.position, transform.position) > 3.5f)
                m_ObjectWasReleased = true;

            float dampening = 2f;

            Vector3 cubeTargetPosForce = m_MovePIDController.Calculate(m_CubeTargetPos, transform.position, dampening, Time.fixedDeltaTime);
            GetComponent<Rigidbody>().AddForce(cubeTargetPosForce, ForceMode.VelocityChange);


            // TODO: get this stuff working!
            // Rotate object to its target position
            //Quaternion targetRot = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5.0f * Time.fixedDeltaTime);

            // Works because: https://stackoverflow.com/questions/24216507/how-to-calculate-euler-angles-from-forward-up-and-right-vectors/24225689
            //Quaternion q1 = transform.rotation;
            //Quaternion q2 = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
            //Quaternion q3 = q2 * Quaternion.Inverse(q1);
            //GetComponent<Rigidbody>().AddTorque(q3.x * 40.0f, q3.y * 40.0f, q3.z * 40.0f, ForceMode.VelocityChange);




            // Object is just released
            if (m_ObjectWasReleased) {
                GetComponent<Rigidbody>().drag = m_Drag;
                GetComponent<Rigidbody>().angularDrag = m_AngularDrag;

                m_Grabbed = false;
                m_ObjectWasReleased = false;
                m_ObjectWasThrown = false;

                // Nullify momentum and apply force to the direction where the object was released
                float forceFromFrames = 1.0f / Time.fixedDeltaTime;
                float forceFromDistance = Mathf.Clamp(Vector3.Distance(m_CubeTargetPos, m_CubeLastTargetPos), -0.5f, 0.5f);
                forceFromDistance = Mathf.Clamp(forceFromDistance, -0.01f, 0.01f);

                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                GetComponent<Rigidbody>().AddForce((m_CubeTargetPos - m_CubeLastTargetPos).normalized * forceFromDistance * forceFromFrames, ForceMode.Impulse);

                CursorManager.ShowCursor();
                GameManager.EndObjectBeignAffected();
            }

            // Object is just thrown
            if (m_ObjectWasThrown) {
                GetComponent<Rigidbody>().drag = m_Drag;
                GetComponent<Rigidbody>().angularDrag = m_AngularDrag;

                m_Grabbed = false;
                m_ObjectWasReleased = false;
                m_ObjectWasThrown = false;
                m_ObjectBeignThrown = true;

                // Nullify momentum and apply throw force mostly forward
                float force = 1.0f / Time.fixedDeltaTime;
                float forwardForce = 28.0f;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                GetComponent<Rigidbody>().AddForce(
                    ((m_CubeTargetPos - m_CubeLastTargetPos) * force * 0.05f + Camera.main.transform.forward) * forwardForce, ForceMode.Impulse);

                CursorManager.ShowCursor();
                GameManager.EndObjectBeignAffected();
            }

        }

        m_CubeLastTargetPos = m_CubeTargetPos;
    }


    private void OnCollisionEnter(Collision collision) {
        ++m_CollidingObjects;

        int index = Random.Range(0, m_BoxHittingSounds.Length);
        float volume = Mathf.Clamp(Vector3.Magnitude(collision.impulse) / 20.0f, 0.0f, 1.0f);
        
        m_BoxHittingSounds[index].volume = volume;
        m_BoxHittingSounds[index].Play();
    }


    private void OnCollisionExit(Collision collision) {
        --m_CollidingObjects;
    }

}
