using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDarkroom_Encounter : MonoBehaviour
{
    public GameObject m_Enemy;
    public GameObject m_Generator1;
    public GameObject m_Generator2;
    public Transform m_ChasePointSpawn;
    public Transform m_ChasePoint;

    //public GameObject playerTriggerZone;
    //public GameObject enemyTriggerZone;

    private bool m_EnemyCanReachPlayer = true;
    //private bool m_PlayerAtTotemRoom = false;

    private const float m_GiveUpAlertedStatusTime = 15.0f;
    private float m_AlertedTimer = 0.0f;

    private const float m_EnemyGiveUpChasingTime = 3.0f;    // Give up chasing after this amount of time after enemy can't see player
    private float m_EnemyGiveUpChasingTimer = 100.0f;       // The enemy will chase player if this is lower than the constant
    private bool m_ContinueChasing = false;

    private bool m_EnemyAlerted = false;

    private void Update() {
        //UpdateSounds();
        UpdateEnemyAI();
    }


    /// <summary>
    /// Updates enemy's AI.
    /// </summary>
    private void UpdateEnemyAI() {

        // When player looks directly to the enemy, its chasing speed increases
        if (m_Enemy.GetComponent<EnemyAI>().PlayerSeeingEnemy())
            m_Enemy.GetComponent<EnemyAI>().ChasingSpeed += Time.deltaTime * 0.2f;
        else
            m_Enemy.GetComponent<EnemyAI>().ChasingSpeed -= Time.deltaTime * 0.2f;

        m_Enemy.GetComponent<EnemyAI>().ChasingSpeed = Mathf.Clamp(m_Enemy.GetComponent<EnemyAI>().ChasingSpeed, 5.5f, 10.0f);


        // Alert enemy if player makes too much noise near it
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().Running && m_Enemy.GetComponent<EnemyAI>().EnemyDistanceToPlayer() < 12.0f) {
            m_Enemy.GetComponent<EnemyAI>().GotoPatrollingPoint(GameObject.FindGameObjectWithTag("Player").transform.position, 5.0f);
            m_EnemyAlerted = true;
            m_AlertedTimer = 0.0f;
        }

        if (m_EnemyAlerted) {
            m_AlertedTimer += Time.deltaTime;
            if (m_AlertedTimer > m_GiveUpAlertedStatusTime)
                m_EnemyAlerted = false;
        }


        // Chase player for short period of time after loosing line of sight
        if (!m_Enemy.GetComponent<EnemyAI>().EnemySeeingPlayer() || !m_EnemyCanReachPlayer) {
            m_EnemyGiveUpChasingTimer += Time.deltaTime;

            if (m_EnemyGiveUpChasingTimer < m_EnemyGiveUpChasingTime)
                m_ContinueChasing = true;
            else
                m_ContinueChasing = false;
        }

        // Chase player only if player is reachable
        if (m_Enemy.GetComponent<EnemyAI>().EnemySeeingPlayer() || m_ContinueChasing) {
            if (!m_ContinueChasing)
                m_EnemyGiveUpChasingTimer = 0.0f;

            m_Enemy.GetComponent<EnemyAI>().EnemyState = EnemyAI.State.Chasing;
            m_EnemyAlerted = false;
        }
        else {
            m_Enemy.GetComponent<EnemyAI>().EnemyState = EnemyAI.State.Patrolling;
        }

        // Enemy got player and player shall die
        if (m_Enemy.GetComponent<EnemyAI>().EnemyDistanceToPlayer() < 1.3f) {
            GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealth>().ShutPlayerDown(true);
            m_Enemy.GetComponent<EnemyAI>().MakeEnemyDisappear();
            Destroy(gameObject);
        }
    }


    // Trigger zones are player's safe zones
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
            m_EnemyCanReachPlayer = false;
    }


    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player"))
            m_EnemyCanReachPlayer = true;
    }

}
