using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3_TotemIntro_Maze : MonoBehaviour
{
    public GameObject m_Totem;
    public GameObject m_Note;

    private GameObject m_Obstacle0;
    private GameObject m_Obstacle1;
    private GameObject m_Obstacle2;

    private TriggerZoneLogics m_TriggerZone0;
    private TriggerZoneLogics m_TriggerZone1;
    private TriggerZoneLogics m_TriggerZone2;
    private bool m_Entered = false;
    private bool m_CloseTheArea = false;

    private float m_Timer = 0.0f;


    private void Start() {
        m_Totem.SetActive(false);

        m_Obstacle0 = transform.GetChild(0).gameObject;
        m_Obstacle1 = transform.GetChild(1).gameObject;
        m_Obstacle2 = transform.GetChild(2).gameObject;

        m_TriggerZone0 = transform.GetChild(3).GetComponent<TriggerZoneLogics>();
        m_TriggerZone1 = transform.GetChild(4).GetComponent<TriggerZoneLogics>();
        m_TriggerZone2 = transform.GetChild(5).GetComponent<TriggerZoneLogics>();

        m_Obstacle0.SetActive(true);
        m_Obstacle1.SetActive(false);
        m_Obstacle2.SetActive(false);
    }


    private void Update() {

        // Player enters the maze
        if (m_TriggerZone0.Enter) {
            m_Obstacle0.SetActive(false);
            m_Obstacle1.SetActive(true);
            m_Entered = true;
        }

        if (m_Entered) {
            m_Timer += Time.deltaTime;
        }

        // Player read the note
        if (m_Entered && m_Note.GetComponent<Note>().IsNoteSeen()) {
            //m_Timer += Time.deltaTime;
            if (m_Timer > 15.0f && m_TriggerZone0.Enter) {
                m_Totem.SetActive(true);
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().UpdateTotems();

                m_Obstacle0.SetActive(false);
                m_Obstacle1.SetActive(false);
            }
        }

        // Player did not read the note
        if (m_Entered && !m_Note.GetComponent<Note>().IsNoteSeen()) {
            //m_Timer += Time.deltaTime;
            if (m_Timer > 8.0f && m_TriggerZone0.Enter) {
                m_Totem.SetActive(true);
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().UpdateTotems();

                m_Obstacle0.SetActive(false);
                m_Obstacle1.SetActive(false);
            }
        }

        // After leaving the maze
        if (m_Entered && m_TriggerZone1.Enter && !m_CloseTheArea) {
            m_CloseTheArea = true;
        }


        if (m_CloseTheArea) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player.transform.localEulerAngles.y > 181.0f && player.transform.localEulerAngles.y < 359.0f || m_TriggerZone2.Enter) {
                m_Obstacle0.SetActive(false);
                m_Obstacle1.SetActive(false);
                m_Obstacle2.SetActive(true);
                m_Entered = false;
            }
        }

    }
}
