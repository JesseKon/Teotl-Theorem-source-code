using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugInfo : MonoBehaviour
{
    private bool debugInfoEnabled = false;
    private string m_DebugInfo = "";
    private Text m_TextComponent;
    private LayerMask m_LayerMask;

    private Color m_ColorEnabled;
    private Color m_ColorDisabled;

    private ShardManager m_ShardManager;
    private PlayerSanity m_PlayerSanity;


    private void Start() {
        m_TextComponent = GetComponent<Text>();
        m_LayerMask = LayerMask.GetMask("InteractableObject");
        m_ColorEnabled = new Color(1.0f, 0.0f, 1.0f, 1.0f);
        m_ColorDisabled = new Color(1.0f, 0.0f, 1.0f, 0.4f);
        m_ShardManager = GameObject.FindGameObjectWithTag("ShardManager").GetComponent<ShardManager>();
        m_PlayerSanity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSanity>();
    }


    private void Update() {
        m_DebugInfo = "Press F1 to toggle debug info\n";

        if (Input.GetKeyDown(KeyCode.F1)) 
            debugInfoEnabled = !debugInfoEnabled;

        if (debugInfoEnabled) {
            m_TextComponent.color = m_ColorEnabled;
            m_DebugInfo += "pos: " + Camera.main.transform.position + '\n';
            m_DebugInfo += "rot: " + Camera.main.transform.rotation.eulerAngles + '\n';

            m_DebugInfo += "interactableObject: ";
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, GameManager.pickDistance, m_LayerMask)) {
                m_DebugInfo += hit.collider.gameObject.name + '\n';
            } else {
                m_DebugInfo += "-\n";
            }

            m_DebugInfo += "Shards: " + GameObject.FindGameObjectWithTag("ShardManager").GetComponent<ShardManager>().NumOfShards() + '\n';

            m_DebugInfo += "fearLevel: " + m_PlayerSanity.FearLevel + '\n';
            m_DebugInfo += "pulse: " + m_PlayerSanity.Pulse + '\n';

        } else {
            m_TextComponent.color = m_ColorDisabled;
        }

        m_TextComponent.text = m_DebugInfo;
    }
}
