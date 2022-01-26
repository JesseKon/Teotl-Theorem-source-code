using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public enum State
    {
        Idle,
        Patrolling,
        Chasing
    }

    public enum PatrollingBehaviour
    {
        /// <summary>
        /// Puts enemy in idle state after completing the route.
        /// </summary>
        Stop,

        /// <summary>
        /// Kills enemy after it has completed its route.
        /// </summary>
        OneShot,

        /// <summary>
        /// Loop the given route.
        /// </summary>
        Loop,

        /// <summary>
        /// Picks a random new point from the route after the previous is reached.
        /// </summary>
        Random
    }

    /* ******************************************** */

    [SerializeField] public bool WillDisappear {
        get { return m_WillDisappear; }
        set { m_WillDisappear = value; }
    }
    [Tooltip("Does the enemy disappear if it catches the player?")]
    public bool m_WillDisappear = false;

    [SerializeField] public float DisappearingProximity {
        get { return m_DisappearingProximity; }
        set { m_DisappearingProximity = value; }
    }
    [Tooltip("How close the enemy has to come in order to disappear.")]
    public float m_DisappearingProximity = 2.0f;


    [SerializeField] public float PatrollingSpeed {
        get { return m_PatrollingSpeed; }
        set { m_PatrollingSpeed = value; }
    }
    [Space][Tooltip("The enemy's patrolling speed.")]
    public float m_PatrollingSpeed = 1.0f;

    [SerializeField] public float ChasingSpeed {
        get { return m_ChasingSpeed; }
        set { m_ChasingSpeed = value; }
    }
    [Tooltip("The enemy's chasing speed.")]
    public float m_ChasingSpeed = 3.0f;

    [SerializeField] public float SeeingDistance {
        get { return m_SeeingDistance; }
        set { m_SeeingDistance = value; }
    }
    [Tooltip("From how far the enemy sees player.")]
    public float m_SeeingDistance = 10.0f;

    [SerializeField] public State EnemyState {
        get { return m_EnemyState; }
        set {
            if (m_EnemyState == value)
                return;

            m_EnemyState = value;
            m_StateChanged = true; 
        }
    }
    [Space][Tooltip("The enemy's initial state.")]
    public State m_EnemyState = State.Idle;

    [SerializeField] public PatrollingBehaviour EnemyPatrollingBehaviour {
        get { return m_EnemyPatrollingBehaviour; }
        set {
            if (m_EnemyPatrollingBehaviour == value)
                return;

            m_EnemyPatrollingBehaviour = value;
            m_StateChanged = true;
        }
    }
    [Tooltip("The enemy's patrolling behaviour.")]
    public PatrollingBehaviour m_EnemyPatrollingBehaviour = PatrollingBehaviour.OneShot;

    [SerializeField] public Transform[] EnemyPatrollingPoints {
        get { return m_EnemyPatrollingPoints; }
        set { m_EnemyPatrollingPoints = value; }
    }
    [Tooltip("List of the enemy's patrolling points")]
    public Transform[] m_EnemyPatrollingPoints;

    public bool PlayEnemySounds {
        get { return m_PlayEnemySounds; }
        set { m_PlayEnemySounds = value; }
    }
    public bool m_PlayEnemySounds = true;

    /* ******************************************** */

    private Transform m_Player;
    private NavMeshAgent m_Agent;
    private uint m_CurrentPatrollingPoint = 0;
    private const float m_UpdateInterval = 0.2f;

    private bool m_StateChanged;
    private bool m_ReachedDestination;

    /* ******************************************** */
    // Audio

    private float m_PatrollingTimer = 0.0f;
    private float m_PatrollingTimeUntilNextSound = 0.0f;
    private float m_ChasingTimer = 0.0f;
    private float m_ChasingTimeUntilNextSound = 0.0f;

    /* ******************************************** */

    private void Start() {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_StateChanged = true;
        m_ReachedDestination = false;

        // Put enemy to the first patrolling point or a random position
        if (m_EnemyPatrollingPoints.Length > 0) {
            if (m_EnemyPatrollingBehaviour == PatrollingBehaviour.Stop) {
                m_Agent.Warp(m_EnemyPatrollingPoints[0].transform.position);
            }
            else if (m_EnemyPatrollingBehaviour == PatrollingBehaviour.OneShot) {
                m_Agent.Warp(m_EnemyPatrollingPoints[0].transform.position);
            }
            else if (m_EnemyPatrollingBehaviour == PatrollingBehaviour.Loop) {
                m_Agent.Warp(m_EnemyPatrollingPoints[0].transform.position);
            }
                
            else if (m_EnemyPatrollingBehaviour == PatrollingBehaviour.Random) {
                m_Agent.Warp(m_EnemyPatrollingPoints[Random.Range(0, m_EnemyPatrollingPoints.Length)].transform.position);
            }

            // Put the enemy on the ground level
            //transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        }
    }


    private void Update() {
        if (m_PlayEnemySounds)
            UpdateSounds();
    }


    private void FixedUpdate() {

        if (m_StateChanged) {
            m_Agent.isStopped = false;

            // Enemy is idling
            if (m_EnemyState == State.Idle) {
                m_Agent.isStopped = true;
            }

            // Enemy is patrolling using waypoints
            else if (m_EnemyState == State.Patrolling) {
                m_Agent.speed = m_PatrollingSpeed;

                // Stop: put the enemy in idle state after it has completed its route
                if (m_EnemyPatrollingBehaviour == PatrollingBehaviour.Stop) {
                    if (m_ReachedDestination && m_CurrentPatrollingPoint < m_EnemyPatrollingPoints.Length)
                        ++m_CurrentPatrollingPoint;

                    if (m_CurrentPatrollingPoint >= m_EnemyPatrollingPoints.Length) {
                        m_Agent.isStopped = true;
                        return;
                    }

                    m_Agent.destination = m_EnemyPatrollingPoints[m_CurrentPatrollingPoint].transform.position;
                }

                // OneShot: destroy the enemy after it has completed its route
                else if (m_EnemyPatrollingBehaviour == PatrollingBehaviour.OneShot) {
                    if (m_ReachedDestination && m_CurrentPatrollingPoint < m_EnemyPatrollingPoints.Length)
                        ++m_CurrentPatrollingPoint;

                    if (m_CurrentPatrollingPoint >= m_EnemyPatrollingPoints.Length) {
                        MakeEnemyDisappear(false);
                        return;
                    }

                    m_Agent.destination = m_EnemyPatrollingPoints[m_CurrentPatrollingPoint].transform.position;
                }

                // Loop: go to the first waypoint after loop is completed
                else if (m_EnemyPatrollingBehaviour == PatrollingBehaviour.Loop) {
                    if (m_ReachedDestination)
                        ++m_CurrentPatrollingPoint;

                    if (m_CurrentPatrollingPoint >= m_EnemyPatrollingPoints.Length)
                        m_CurrentPatrollingPoint = 0;

                    m_Agent.destination = m_EnemyPatrollingPoints[m_CurrentPatrollingPoint].transform.position;
                }

                // Random: always pick a random waypoint
                else if (m_EnemyPatrollingBehaviour == PatrollingBehaviour.Random) {
                    m_Agent.destination = m_EnemyPatrollingPoints[Random.Range(0, m_EnemyPatrollingPoints.Length)].transform.position;
                }

            }

            // Enemy is chasing the player
            else if (m_EnemyState == State.Chasing) {
                StartCoroutine(Chasing());
            }

            m_StateChanged = false;
            m_ReachedDestination = false;
        }

        // Get next waypoint when current is reached and the enemy is in patrolling mode
        // TODO: remainingDistance to const
        if (!m_Agent.pathPending && m_EnemyState == State.Patrolling && m_Agent.remainingDistance < 0.25f) {
            m_StateChanged = true;
            m_ReachedDestination = true;
        }

        // Destroy the enemy if it's meant to disappear when too close to player
        if (m_WillDisappear && Vector3.Distance(m_Player.position, transform.position) < m_DisappearingProximity) {
            MakeEnemyDisappear(false);
        }
    }


    private void UpdateSounds() {

        // Patrolling and alerted sounds
        if (GetComponent<EnemyAI>().EnemyState == State.Patrolling) {
            m_PatrollingTimer += Time.deltaTime;
            m_ChasingTimer = 0.0f;

            if (m_PatrollingTimer > m_PatrollingTimeUntilNextSound) {
                float audioLength;

                // Change to hiss to player if player is close
                if (GetComponent<EnemyAI>().EnemyDistanceToPlayer() < 7.0f) {
                    int dice = Random.Range(0, 2);

                    if (dice == 0) {
                        GetComponent<EnemySounds>().PlayRandomSound(EnemySounds.SoundType.Hissing, out audioLength);
                    } else {
                        GetComponent<EnemySounds>().PlayRandomSound(EnemySounds.SoundType.Patrolling, out audioLength);
                    }

                } else {
                    GetComponent<EnemySounds>().PlayRandomSound(EnemySounds.SoundType.Patrolling, out audioLength);
                }


                m_PatrollingTimer = 0.0f;
                m_PatrollingTimeUntilNextSound = Random.Range(audioLength + 1.2f, audioLength + 2.0f);
            }
        }

        // Chasing sounds
        if (GetComponent<EnemyAI>().EnemyState == State.Chasing) {
            m_ChasingTimer += Time.deltaTime;

            if (m_ChasingTimer > m_ChasingTimeUntilNextSound) {
                GetComponent<EnemySounds>().PlayRandomSound(EnemySounds.SoundType.Chasing, out float audioLength);
                m_ChasingTimer = 0.0f;
                m_ChasingTimeUntilNextSound = Random.Range(audioLength + 0.5f, audioLength + 0.8f);
            }
        }

    }


    /// <summary>
    /// Enemy follows player while in chase mode.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Chasing() {
        m_Agent.destination = m_Player.position;
        m_Agent.speed = m_ChasingSpeed;
        
        yield return new WaitForSeconds(m_UpdateInterval);
        m_StateChanged = true;
    }


    /// <summary>
    /// Warps enemy to the new location.
    /// </summary>
    /// <param name="newPos">The new location for the enemy.</param>
    public void SetPosition(Vector3 newPos) {
        if (!m_Agent.Warp(newPos))
            Debug.LogWarning("NavMeshAgent warping wasn't success: " + gameObject.name);
    }


    /// <summary>
    /// Overrides enemy's current patrolling point.
    /// </summary>
    /// <param name="newPoint">The new patrolling point.</param>
    /// <param name="speed">Enemy's speed until the it reaches the new patrolling point.</param>
    public void GotoPatrollingPoint(Vector3 newPoint, float speed) {
        m_Agent.destination = newPoint;
        m_Agent.speed = speed;
    }


    /// <summary>
    /// Check if enemy is seeing the player.
    /// </summary>
    /// <returns>True if enemy is seeing the player and false otherwise.</returns>
    public bool EnemySeeingPlayer() {

        // TODO: very error prone stuff! "Vector3.up * 0.7f"
        Vector3 enemyEyesPosition = transform.position + Vector3.up * 1.5f;
        Vector3 playerFeetPosition = GameObject.FindGameObjectWithTag("Player").transform.position - Vector3.up * 0.7f;

        // Head trigger zone
        if (Physics.Raycast(enemyEyesPosition, Camera.main.transform.position - enemyEyesPosition, out RaycastHit hitTop, m_SeeingDistance)) {
            if (hitTop.collider.CompareTag("MainCamera"))
                return true;
        }

        // Feets trigger zone
        if (Physics.Raycast(enemyEyesPosition, playerFeetPosition - enemyEyesPosition, out RaycastHit hitBottom, m_SeeingDistance)) {
            if (hitBottom.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }


    /// <summary>
    /// Check if player is seeing the enemy.
    /// </summary>
    /// <param name="seeingDistance">From for far seeing is registered.</param>
    /// <returns>True if player is seeing the enemy and false otherwise.</returns>
    public bool PlayerSeeingEnemy(float seeingDistance = 10.0f) {
        Vector3 enemyViewportPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool enemyInViewport =
            enemyViewportPoint.z > 0.0f &&
            enemyViewportPoint.x > 0.0f &&
            enemyViewportPoint.x < 1.0f &&
            enemyViewportPoint.y > 0.0f &&
            enemyViewportPoint.y < 1.0f;

        Vector3 playerEyesPosition = Camera.main.transform.position + Camera.main.transform.forward * 0.15f;
        Vector3 enemyEyesPosition = transform.position + Vector3.up * 1.5f;
        Vector3 enemyFeetPosition = transform.position;

        // Head trigger zone
        if (Physics.Raycast(playerEyesPosition, enemyEyesPosition - playerEyesPosition, out RaycastHit hitTop, seeingDistance)) {
            if (hitTop.collider.CompareTag("Enemy"))
                return enemyInViewport;
        }

        // Feets trigger zone
        if (Physics.Raycast(playerEyesPosition, enemyFeetPosition - playerEyesPosition, out RaycastHit hitBottom, seeingDistance)) {
            if (hitBottom.collider.CompareTag("Enemy"))
                return enemyInViewport;
        }

        return false;
    }


    /// <summary>
    /// Returns the distance between enemy and player.
    /// </summary>
    /// <returns>The distance between enemy and player.</returns>
    public float EnemyDistanceToPlayer() {
        return Vector3.Distance(transform.position + Vector3.up * 1.5f, Camera.main.transform.position);
    }


    /// <summary>
    /// Returns the next patrolling point or the last if enemy has reached the destination.
    /// </summary>
    /// <returns>The next patrolling point.</returns>
    public uint GetNextPatrollingPoint() {
        return m_CurrentPatrollingPoint;
    }


    /// <summary>
    /// Makes the enemy disappear by destroying it.
    /// </summary>
    public void MakeEnemyDisappear(bool useEffect = false) {
        if (useEffect)
            Debug.LogWarning("No effect implemented when enemy disappears!");

        // TODO: some nice effect when it disappears
        Destroy(gameObject);
    }
}
