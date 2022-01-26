using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This enemy will attack after both generators on lvl 2 are running. The enemy will spawn behind the electric door
/// that the generators will open.
/// </summary>

public class Enemy4_Encounter : MonoBehaviour
{
    public GameObject m_Enemy;
    public Transform m_SpawnPointIfStunned;
    public GameObject m_Generator1;
    public GameObject m_Generator2;

    private bool m_Activated = false;

    private const float m_EnemySpeedWhenChasing = 3.5f;

    private bool m_EnemyCanReachPlayer = true;

    private void Start() {
        m_Enemy.SetActive(false);
    }


    private void Update() {

        // Destroy this encounter if player has already woken up in prison
        if (GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>().WokeUpInPrisonCell) {
            m_Enemy.GetComponent<EnemyAI>().MakeEnemyDisappear();
            Destroy(gameObject);
        }


        // Spawn enemy after both generators are on
        if (!m_Activated) {
            if (m_Generator1.GetComponent<Generator>().IsGeneratorActivated() && m_Generator2.GetComponent<Generator>().IsGeneratorActivated()) {
                m_Activated = true;
                m_Enemy.SetActive(true);
                m_Enemy.GetComponent<EnemyAI>().EnemyState = EnemyAI.State.Chasing;
                m_Enemy.GetComponent<EnemyAI>().ChasingSpeed = m_EnemySpeedWhenChasing;
            }
        }

        // Start chasing
        if (m_Activated) {

            if (m_EnemyCanReachPlayer)
                m_Enemy.GetComponent<EnemyAI>().EnemyState = EnemyAI.State.Chasing;
            else
                m_Enemy.GetComponent<EnemyAI>().EnemyState = EnemyAI.State.Patrolling;

            // Destroy enemy when player is far enough
            if (m_Enemy.GetComponent<EnemyAI>().EnemyDistanceToPlayer() > 15.0f) {
                m_Enemy.GetComponent<EnemyAI>().MakeEnemyDisappear();
                Destroy(gameObject);
            }

        }

        // Enemy got player, wake up in prison
        if (m_Enemy.GetComponent<EnemyAI>().EnemyDistanceToPlayer() < 1.3f) {

            GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>().WokeUpInPrisonCell = true;
            Debug.Log("Player will wake up in the prison cell.");

            // Stun player and put fear levels back to normal
            GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealth>().Stunned(m_SpawnPointIfStunned.transform.position, Quaternion.identity, 5.0f, 5.0f);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSanity>().FearLevel = 0.0f;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSanity>().Pulse = 40.0f;

            m_Enemy.GetComponent<EnemyAI>().MakeEnemyDisappear();
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
            m_EnemyCanReachPlayer = false;
    }


    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player"))
            m_EnemyCanReachPlayer = true;
    }
}
