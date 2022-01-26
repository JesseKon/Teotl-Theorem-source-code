using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Player's movement speed
    public float MovementSpeed {
        get { return m_MovementSpeed; }
    }
    private float m_MovementSpeed = 0.0f;
    private const float m_MaxMovementSpeed = 2.75f;

    // Player's running movement speed multiplier
    public float RunMultiplier {
        get { return m_RunMultiplier; }
    }
    private float m_RunMultiplier = 1.0f;
    private const float m_MaxRunMultiplier = 1.62f;

    // Player's crouching movement speed multiplier
    public float CrouchingMultiplier {
        get { return m_CrouchingMultiplier; }
    }
    private float m_CrouchingMultiplier = 1.0f;
    private const float m_MaxCrouchingMultiplier = 0.4f;

    // Player's jumping height
    public float JumpHeight {
        get { return m_JumpHeight; }
    }
    private const float m_JumpHeight = 1.0f;

    // The global player's movement speed multiplier
    public float GlobalSpeedMultiplier {
        get { return m_GlobalSpeedMultiplier; }
        set { m_GlobalSpeedMultiplier = value; }
    }
    private float m_GlobalSpeedMultiplier = 1.0f;

    private const float m_Gravity = -9.81f;
    private Vector3 m_Velocity;

    /// <summary>
    /// Is player currently running?
    /// </summary>
    public bool Running {
        get { return m_Running; }
    }
    private bool m_Running = false;

    /// <summary>
    /// Is player currently crouching?
    /// </summary>
    public bool Crouching {
        get { return m_Crouching; }
    }
    private bool m_Crouching = false;

    /// <summary>
    /// Is player currently jumping and on air
    /// </summary>
    public bool Jumping {
        get { return m_Jumping; }
    }
    private bool m_Jumping = false;

    public bool LockMovementControls {
        get { return m_LockMovementControls; }
        set { m_LockMovementControls = value; }
    }
    private bool m_LockMovementControls = false;

    private bool m_CanStandUp = false;
    private bool m_Jump = false;


    private Vector3 m_MoveVector = Vector3.zero;

    private const float m_CcHeightWhenStanding = 1.80f;
    private const float m_CcHeightWhenCrouching = 0.80f;
    private const float m_CameraYWhenStanding = 0.74f;
    private const float m_CameraYWhenCrouching = -0.25f;

    private CharacterController m_CharacterController;
    private Camera m_MainCamera;

    private AudioSource[] m_FootStepSounds;
    private float m_FootStepTimer = 0.0f;

    private AudioSource[] m_JumpSounds;


    private void Start() {
        m_CharacterController = GetComponent<CharacterController>();
        m_MainCamera = Camera.main;

        m_CharacterController.height = m_CcHeightWhenStanding;

        m_MainCamera.transform.localPosition = new Vector3(
            m_MainCamera.transform.localPosition.x,
            m_CameraYWhenStanding,
            m_MainCamera.transform.localPosition.z
        );

        m_FootStepSounds = transform.GetChild(3).GetComponentsInChildren<AudioSource>();
        m_JumpSounds = transform.GetChild(4).GetComponentsInChildren<AudioSource>();
    }


    private void Update() {
        if (m_LockMovementControls)
            return;

        // Update jumping behaviour
        if (Input.GetKeyDown(KeyCode.Space) && !m_Crouching) {
            m_Jump = true;
            //m_JumpSounds[Random.Range(0, m_JumpSounds.Length)].Play();  // Play jump sounds
        }
    }


    private void FixedUpdate() {
        m_Jumping = !m_CharacterController.isGrounded;

        if (m_LockMovementControls)
            return;

        float x = 0.0f;
        float z = 0.0f;
        bool moving = false;

        if (Input.GetKey(KeyCode.A)) {
            x = -1.0f;
            moving = true;
        }

        if (Input.GetKey(KeyCode.D)) {
            x = 1.0f;
            moving = true;
        }

        if (Input.GetKey(KeyCode.W)) {
            z = 1.0f;
            moving = true;
        }

        if (Input.GetKey(KeyCode.S)) {
            z = -1.0f;
            moving = true;
        }

        m_Running = Input.GetKey(KeyCode.LeftShift) && !GameManager.IsTotemBeignDragged();
        m_Crouching = Input.GetKey(KeyCode.LeftControl) && m_CharacterController.isGrounded;

        // Running logic
        if (m_Running) {
            m_RunMultiplier = Mathf.Lerp(m_RunMultiplier, m_MaxRunMultiplier, 6.0f * Time.fixedDeltaTime);
        } else {
            m_RunMultiplier = Mathf.Lerp(m_RunMultiplier, 1.0f, 12.0f * Time.fixedDeltaTime);
        }

        // Crouching logic
        if (m_Crouching || !m_CanStandUp) {
            m_CrouchingMultiplier = Mathf.Lerp(m_CrouchingMultiplier, m_MaxCrouchingMultiplier, 12.0f * Time.fixedDeltaTime);
        } else {
            m_CrouchingMultiplier = Mathf.Lerp(m_CrouchingMultiplier, 1.0f, 2.5f * Time.fixedDeltaTime);
        }

        if (m_CharacterController.isGrounded) {
            m_Velocity.y = -0.1f;
        }

        // Execute jump action
        if (m_CharacterController.isGrounded && !m_Crouching && m_CanStandUp) {
            if (m_Jump) {
                m_Velocity.y = Mathf.Sqrt(m_JumpHeight * -2.0f * m_Gravity);
            }
        }
        else {
            m_Velocity.y += m_Gravity * Time.deltaTime;
            m_Jump = false;
        }

        // Crouching logic
        if (m_Crouching) {
            m_CharacterController.center = new Vector3(0.0f, (m_CcHeightWhenCrouching - m_CcHeightWhenStanding) / 2.0f, 0.0f);

            Vector3 newPos = new Vector3(
                m_MainCamera.transform.localPosition.x,
                m_CameraYWhenCrouching,
                m_MainCamera.transform.localPosition.z
            );

            // Crouch down slowly
            m_MainCamera.transform.localPosition = Vector3.Lerp(m_MainCamera.transform.localPosition, newPos, 6.0f * Time.fixedDeltaTime);

            m_CharacterController.height = m_CcHeightWhenCrouching;
            m_CanStandUp = false;
        }
        
        // Not crouching, perhaps trying to stand up
        else {

            // Make sure there is room to stand up
            bool somethingAbovePlayer = Physics.BoxCast(m_MainCamera.transform.position, Vector3.one * 0.2f, Vector3.up, out RaycastHit hit, Quaternion.identity, 0.2f);
            //ExtDebug.DrawBoxCastBox(m_MainCamera.transform.position, Vector3.one * 0.2f, Quaternion.identity, Vector3.up, 0.2f, Color.red);

            if (!somethingAbovePlayer) {
                m_CharacterController.center = new Vector3(0.0f, 0.0f, 0.0f);

                Vector3 newPos = new Vector3(
                    m_MainCamera.transform.localPosition.x,
                    m_CameraYWhenStanding,
                    m_MainCamera.transform.localPosition.z
                );

                // Stand up slowly
                m_MainCamera.transform.localPosition = Vector3.Lerp(m_MainCamera.transform.localPosition, newPos, 4.5f * Time.fixedDeltaTime);

                m_CharacterController.height = m_CcHeightWhenStanding;
                m_CanStandUp = true;

            }

        }

        // xz-axis
        float dampening = 0.0f;

        if (moving && m_CharacterController.isGrounded) {
            dampening = 12.0f;
        } else if (!moving && m_CharacterController.isGrounded) {
            dampening = 20.0f;
        } else if (moving && !m_CharacterController.isGrounded) {
            dampening = 2.0f;
        } else if (!moving && !m_CharacterController.isGrounded) {
            dampening = 1.0f;
        }

        // Player hits ceiling or something from above
        if ((m_CharacterController.collisionFlags & CollisionFlags.Above) != 0) {
            m_Velocity.y = 0.0f;
            m_MoveVector *= 0.1f;
        }

        m_MoveVector = Vector3.Lerp(m_MoveVector, (transform.right * x + transform.forward * z).normalized, dampening * Time.fixedDeltaTime);

        m_CharacterController.Move(
            m_MoveVector * m_MaxMovementSpeed * m_RunMultiplier * m_CrouchingMultiplier *
            (GameManager.IsTotemBeignDragged() ? GameManager.totemDraggingSpeedMultiplier : 1.0f) *
            m_GlobalSpeedMultiplier * Time.deltaTime
        );

        // y-axis
        m_CharacterController.Move(m_Velocity * m_GlobalSpeedMultiplier * Time.deltaTime);

        // Update foot step sounds
        if (moving) {
            float volume = 0.65f;

            if (m_Running)
                volume = 1.0f;
            else if (m_Crouching)
                volume = 0.3f;

            m_FootStepTimer += Time.fixedDeltaTime;
            if (m_FootStepTimer > (m_Crouching ? 0.75f : m_Running ? 0.35f : 0.55f) && m_CharacterController.isGrounded) {
                int index = Random.Range(0, m_FootStepSounds.Length);
                m_FootStepSounds[index].volume = volume;
                m_FootStepSounds[index].Play();
                m_FootStepTimer = 0.0f;
            }
        }
        else {
            m_FootStepTimer = 0.0f;
        }

    }



}
