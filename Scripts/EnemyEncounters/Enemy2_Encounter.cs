using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2_Encounter : MonoBehaviour
{
    public GameObject m_Note;
    public GameObject m_Enemy;

    private bool m_Activated = false;
    private bool m_SpawnEnemy = false;

    private void Start() {
        m_Enemy.SetActive(false);
        m_Enemy.GetComponent<EnemyAI>().EnemyState = EnemyAI.State.Idle;
        m_Enemy.GetComponent<EnemyAI>().EnemyPatrollingBehaviour = EnemyAI.PatrollingBehaviour.OneShot;
    }


    private void Update() {

        // Activate the enemy after the note is opened
        if (!m_Activated && m_Note.GetComponent<Note>().IsNoteSeen()) {
            m_Activated = true;
            m_SpawnEnemy = true;
            m_Enemy.SetActive(true);
        }

        // Start moving the enemy when player looks at it
        if (m_SpawnEnemy && m_Enemy.GetComponent<EnemyAI>().PlayerSeeingEnemy()) {
            m_Enemy.GetComponent<EnemyAI>().EnemyState = EnemyAI.State.Patrolling;
            m_SpawnEnemy = false;
            StartCoroutine(SlowDownPlayer());
        }
    }


    private IEnumerator SlowDownPlayer() {
        PlayerMovement playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        playerMovement.GlobalSpeedMultiplier = 0.45f;
        yield return new WaitForSeconds(1.5f);

        playerMovement.GlobalSpeedMultiplier = 1.0f;
        Destroy(gameObject);
    }
}
