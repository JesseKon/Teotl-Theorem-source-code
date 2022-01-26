using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Sprite defaultIcon;
    public Sprite handIcon;
    public Sprite handArrowIcon;
    public Sprite electricityIcon;
    public Sprite relicIcon;

    public Texture2D cursor;

    private static bool m_HideCursor;

    private SpriteRenderer m_SpriteRenderer;
    private LayerMask m_LayerMask;


    public static void HideCursor() {
        m_HideCursor = true;
    }


    public static void ShowCursor() {
        m_HideCursor = false;
    }


    public void HideActualCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void ShowActualCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }


    private void Start() {
        HideActualCursor();
        
        m_HideCursor = true;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_LayerMask = LayerMask.GetMask("InteractableObject", "WorldCollider");
    }


    private void FixedUpdate() {

        if (m_HideCursor) {
            m_SpriteRenderer.sprite = null;
        }

        m_SpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.15f);

        // Picked wall or door
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit worldColliderHit, GameManager.pickDistance, m_LayerMask) &&
            worldColliderHit.collider.gameObject.layer == LayerMask.NameToLayer("WorldCollider")) {
            m_SpriteRenderer.sprite = defaultIcon;
        }
        
        else if (!m_HideCursor && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, GameManager.pickDistance, m_LayerMask)) {
            m_SpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.7f);

            // Show generator icon if generator is picked and it's not activated
            if (hit.collider.gameObject.CompareTag("Generator")) {
                if (!hit.collider.gameObject.GetComponent<Generator>().IsGeneratorActivated()) {
                    m_SpriteRenderer.sprite = electricityIcon;
                }

            } else if (hit.collider.gameObject.CompareTag("RelicHolder")) {
                if (!hit.collider.gameObject.GetComponent<RelicHolder>().IsActivated()) {
                    m_SpriteRenderer.sprite = relicIcon;
                }

            } else if (hit.collider.gameObject.name == "rotator") { // Generator rotator, show default icon
                m_SpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.15f);
                m_SpriteRenderer.sprite = defaultIcon;

            } else if (hit.collider.gameObject.CompareTag("Shard")) {
                m_SpriteRenderer.sprite = handIcon;

            } else if (hit.collider.gameObject.CompareTag("ManualDoorHandlerValve")) {
                m_SpriteRenderer.sprite = handArrowIcon;

            } else {
                m_SpriteRenderer.sprite = handIcon;
            }

        } else if (!m_HideCursor) {
            m_SpriteRenderer.sprite = defaultIcon;
            m_SpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.15f);
        }
    }
}
