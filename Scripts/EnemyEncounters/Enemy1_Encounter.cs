using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_Encounter : MonoBehaviour
{
    public GameObject m_Enemy;

    private EnemyAI m_EnemyAI;
    private bool m_Activated = false;


    private void Start() {
        m_EnemyAI = m_Enemy.GetComponent<EnemyAI>();
        m_EnemyAI.EnemyState = EnemyAI.State.Idle;
        m_EnemyAI.EnemyPatrollingBehaviour = EnemyAI.PatrollingBehaviour.OneShot;
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (!m_Activated)
                m_Activated = true;

            m_EnemyAI.EnemyState = EnemyAI.State.Patrolling;
        }
    }
}
