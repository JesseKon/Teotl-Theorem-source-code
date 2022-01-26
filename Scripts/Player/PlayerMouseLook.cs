using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseLook : MonoBehaviour
{
    public Transform playerBody;

    private GameManager m_GameManager;

    private float m_XRot = 0;

    public bool LockMouseControls {
        get { return m_LockMouseControls; }
        set { m_LockMouseControls = value; }
    }
    private bool m_LockMouseControls = false;


    private void Start() {
        m_GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }


    private void Update() {
        if (m_LockMouseControls)
            return;

        if (!GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().IsMouseControlStolen()) {
            float mouseX = Input.GetAxis("Mouse X") * m_GameManager.ActualMouseSensitivity * (GameManager.IsTotemBeignDragged() ? Mathf.Clamp(1.0f - GameManager.GetTotemDragDistance(), 0.0f, 1.0f) : 1.0f) * Time.timeScale;
            float mouseY = Input.GetAxis("Mouse Y") * m_GameManager.ActualMouseSensitivity * (GameManager.IsTotemBeignDragged() ? Mathf.Clamp(1.0f - GameManager.GetTotemDragDistance(), 0.0f, 1.0f) : 1.0f) * Time.timeScale;

            m_XRot -= mouseY;
            m_XRot = Mathf.Clamp(m_XRot, -82.0f, 82.0f);

            transform.localRotation = Quaternion.Euler(m_XRot, 0.0f, 0.0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }

    }

}
