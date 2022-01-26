using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicLogic : MonoBehaviour
{
    private LayerMask m_LayerMask;
    private bool m_ActionButtonIsPressed = false;

    private MeshRenderer m_MeshRenderer;

    readonly Color m_Color1 = new Color(1.0f, 1.0f, 1.0f, 1.0f) * 0.5f;
    readonly Color m_Color2 = new Color(0.9f, 1.0f, 0.7f, 1.0f) * 2.5f;
    private Color m_CurrentColor;

    private bool m_Picked = false;


    private void Start() {
        m_LayerMask = LayerMask.GetMask("InteractableObject");

        m_MeshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        m_MeshRenderer.material.EnableKeyword("_EMISSION");
    }


    private void Update() {
        m_ActionButtonIsPressed = Input.GetMouseButton(0);

        m_CurrentColor = Color.Lerp(m_Color1, m_Color2, Mathf.Sin(Time.realtimeSinceStartup * 3.0f));
        m_MeshRenderer.material.SetColor("_EmissionColor", m_CurrentColor);
    }


    private void FixedUpdate() {

        // When relic is picked
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, GameManager.pickDistance, m_LayerMask)) {
            if (hit.collider.CompareTag("Relic") && hit.collider.gameObject == gameObject && m_ActionButtonIsPressed) {
                PickItem();
            }
        }
    }


    private void OnTriggerEnter(Collider other) {

        // When player walk over the relic
        if (other.gameObject.CompareTag("Player")) {
            PickItem();
        }
    }


    // Add relic to player's inventory
    private void PickItem() {
        if (m_Picked)
            return;

        m_Picked = true;
        GameObject.FindGameObjectWithTag("RelicManager").GetComponent<RelicManager>().AddRelic();
        transform.GetChild(0).transform.position = new Vector3(0.0f, 1000.0f, 0.0f);    // "hide" the item
        transform.GetChild(1).GetComponent<AudioSource>().Play();
        Destroy(gameObject, 1.0f);
    }
}
