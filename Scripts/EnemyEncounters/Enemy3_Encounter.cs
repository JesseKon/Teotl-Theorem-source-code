using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Encounter 3 happens when player picks up the note in level 2.
/// </summary>

public class Enemy3_Encounter : MonoBehaviour
{
    public GameObject m_Note;
    public GameObject m_Enemy;
    public Transform m_SpawnPointIfStunned;

    private bool m_Activated = false;
    float m_Timer = 0.0f;

    private bool m_SpawnEnemy = false;
    private bool m_EnemyCanChasePlayer = false;
    private bool m_EnemyBeginChase = false;

    private void Start() {
        m_Enemy.SetActive(false);
        m_Enemy.GetComponent<EnemyAI>().EnemyState = EnemyAI.State.Idle;
        m_Enemy.GetComponent<EnemyAI>().EnemyPatrollingBehaviour = EnemyAI.PatrollingBehaviour.Stop;
    }


    private void Update() {

        // TODO: probably because the slowdown didn't wear off and object is beign deleted

        // Destroy this encounter if player has already woken up in prison
        if (GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>().WokeUpInPrisonCell) {
            m_Enemy.GetComponent<EnemyAI>().MakeEnemyDisappear();
            Destroy(gameObject);
        }

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



        if (m_Activated) {

            m_Timer += Time.deltaTime;
            const float enemyTimer = 20.2f; // After this many seconds the enemy will disappear
            if (m_Timer > enemyTimer && m_Enemy.GetComponent<EnemyAI>().EnemyDistanceToPlayer() > 15.0f && m_Enemy.GetComponent<EnemyAI>().EnemyState != EnemyAI.State.Chasing) {
                m_Enemy.GetComponent<EnemyAI>().MakeEnemyDisappear();
                PlayerMovement playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
                playerMovement.GlobalSpeedMultiplier = 1.0f;
                Destroy(gameObject);
            }

            if (m_Enemy.GetComponent<EnemyAI>().GetNextPatrollingPoint() >= 5 && m_Enemy.GetComponent<EnemyAI>().EnemySeeingPlayer()) {
                //m_EnemyCanChasePlayer = true;
                m_EnemyBeginChase = true;
            }

            // Chase player until player is stunned, if player had rushed towards it
            if (m_EnemyBeginChase) {
                m_Enemy.GetComponent<EnemyAI>().EnemyState = EnemyAI.State.Chasing;

                // Enemy got player
                if (m_Enemy.GetComponent<EnemyAI>().EnemyDistanceToPlayer() < 1.3f) {

                    GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>().WokeUpInPrisonCell = true;
                    Debug.Log("Player will wake up in the prison cell.");

                    // Stun player and put fear levels back to normal and wake up in the "prison cell"
                    GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealth>().Stunned(m_SpawnPointIfStunned.transform.position, Quaternion.identity, 5.0f, 5.0f);
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSanity>().FearLevel = 0.0f;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSanity>().Pulse = 40.0f;

                    m_Enemy.GetComponent<EnemyAI>().MakeEnemyDisappear();
                    Destroy(gameObject);
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other) {
        //if (!m_Activated && m_Note.GetComponent<Note>().IsNoteSeen()) {
        //    m_Activated = true;
        //    m_Enemy.SetActive(true);
        //    m_Enemy.GetComponent<EnemyAI>().EnemyState = EnemyAI.State.Patrolling;
        //    StartCoroutine(SlowDownPlayer());
        //}
    }


    private IEnumerator SlowDownPlayer() {
        PlayerMovement playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        yield return new WaitForSeconds(0.5f);

        playerMovement.GlobalSpeedMultiplier = 0.53f;
        yield return new WaitForSeconds(9.0f);

        playerMovement.GlobalSpeedMultiplier = 1.0f;
    }
}
