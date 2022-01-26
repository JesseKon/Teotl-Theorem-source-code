using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRedRoom_Encounter : MonoBehaviour
{
    public GameObject m_Enemy;
    public GameObject[] m_TriggerZoneChaseStart;
    public GameObject[] m_TriggerZoneChaseEnd;
    public GameObject m_FallDeath;

    //public GameObject m_ChasingEnemy;


    private void Update() {
        foreach (GameObject chaseStart in m_TriggerZoneChaseStart)
            if (chaseStart.GetComponent<TriggerZone_RedRoom>().OnTriggerZone)
                m_Enemy.GetComponent<EnemyAI>().EnemyState = EnemyAI.State.Chasing;

        foreach (GameObject chaseEnd in m_TriggerZoneChaseEnd)
            if (chaseEnd.GetComponent<TriggerZone_RedRoom>().OnTriggerZone)
                m_Enemy.GetComponent<EnemyAI>().EnemyState = EnemyAI.State.Patrolling;


        // Enemy got player and player shall die
        if (m_Enemy.GetComponent<EnemyAI>().EnemyDistanceToPlayer() < 1.3f) {
            GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealth>().ShutPlayerDown(true);
            m_Enemy.GetComponent<EnemyAI>().MakeEnemyDisappear();
            Destroy(gameObject);
        }

        // Player fell to death
        if (m_FallDeath.GetComponent<TriggerZone_RedRoom>().OnTriggerZone) {
            GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealth>().ShutPlayerDown(true);
            m_Enemy.GetComponent<EnemyAI>().MakeEnemyDisappear();
            Destroy(gameObject);
        }

    }

}
