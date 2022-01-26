using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerSanity : MonoBehaviour
{

    [SerializeField] public float FearLevel {
        get { return m_FearLevel; }
        set { m_FearLevel = value; }
    }
    private float m_FearLevel;

    private const float m_FearLevelMin = 0.0f;
    private const float m_FearLevelMax = 1.0f;

    [SerializeField] public float Pulse {
        get { return m_Pulse; }
        set { m_Pulse = value; }
    }
    private float m_Pulse;

    private const float m_PulseMin = 40.0f;
    private const float m_PulseMax = 230.0f;

    private float m_PulseTimer;
    private float m_ParalyzeTimer;

    [HideInInspector]
    public bool m_ControlsParalyzed;

    private float m_FixedDeltaTime;

    private RendererTextureCameraPlane m_RendererTextureCameraPlane;

    private Camera m_MainCamera;
    private float m_CameraNormalFOV;

    private PostProcessVolume m_PPVolume;
    private ChromaticAberration m_ChromaticAberration;
    float m_ChromaticAberrationNormalIntensity;


    private void Start() {
        m_FearLevel = 0.0f;
        m_Pulse = 40.0f;

        m_PulseTimer = 0.0f;
        m_ParalyzeTimer = 0.0f;
        m_ControlsParalyzed = false;
        m_FixedDeltaTime = Time.fixedDeltaTime;

        m_RendererTextureCameraPlane = GameObject.Find("RenderTextureCameraPlane").GetComponent<RendererTextureCameraPlane>();

        m_MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        m_CameraNormalFOV = m_MainCamera.fieldOfView;

        m_PPVolume = GameObject.FindGameObjectWithTag("PostProcessing").GetComponent<PostProcessVolume>();

        m_PPVolume.profile.TryGetSettings(out m_ChromaticAberration);
        m_ChromaticAberrationNormalIntensity = m_ChromaticAberration.intensity;
    }


    private void FixedUpdate() {
        UpdateFearSystem();
    }


    private void UpdateFearSystem() {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        bool beignChased = false;
        bool enemyVeryClose = false;
        float enemyDistanceToPlayer = 0.0f;
        bool enemySeeingPlayer = false;

        foreach (GameObject enemy in enemies) {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

            if (!beignChased && enemyAI.EnemyState == EnemyAI.State.Chasing)
                beignChased = true;

            if (!enemyVeryClose && enemyAI.EnemyDistanceToPlayer() < 3.5f) {
                enemyVeryClose = true;
                enemyDistanceToPlayer = enemyAI.EnemyDistanceToPlayer();
            }

            else if (!enemySeeingPlayer && enemyAI.PlayerSeeingEnemy()) {
                enemySeeingPlayer = true;
                enemyDistanceToPlayer = enemyAI.EnemyDistanceToPlayer();
            }

        }

        // Enemy is chasing player
        if (beignChased) {
            float dummy = 0.0f;
            float newFov = Mathf.SmoothDamp(m_MainCamera.fieldOfView, m_CameraNormalFOV + 10.0f, ref dummy, 0.3f);
            m_MainCamera.fieldOfView = newFov;

            float newCAValue = Mathf.Min(m_ChromaticAberration.intensity.value + Time.fixedDeltaTime * 0.2f, m_ChromaticAberrationNormalIntensity + 0.7f);
            m_ChromaticAberration.intensity.value = newCAValue;
        }
        
        // Enemy isn't chasing player
        else {
            float dummy = 0.0f;
            float newFov = Mathf.SmoothDamp(m_MainCamera.fieldOfView, m_CameraNormalFOV, ref dummy, 0.8f);
            m_MainCamera.fieldOfView = newFov;

            float newCAValue = Mathf.Max(m_ChromaticAberration.intensity.value - Time.fixedDeltaTime * 0.04f, m_ChromaticAberrationNormalIntensity);
            m_ChromaticAberration.intensity.value = newCAValue;
        }

        // Enemy really close to player
        if (enemyVeryClose) {
            m_Pulse += (GameManager.seeingDistance - enemyDistanceToPlayer) * 12.0f * Time.deltaTime;
            m_FearLevel += (GameManager.seeingDistance - enemyDistanceToPlayer) * 0.3f * Time.deltaTime;
            m_RendererTextureCameraPlane.DizzinessSeverity += 0.0046f * Time.fixedDeltaTime;
            m_RendererTextureCameraPlane.SwayingSeverity += 0.045f * Time.fixedDeltaTime;
            m_RendererTextureCameraPlane.SwayingSpeed += 0.045f * Time.fixedDeltaTime;
        }

        // Enemy sees player
        else if (enemySeeingPlayer) {
            m_Pulse += (GameManager.seeingDistance - enemyDistanceToPlayer) * 6.0f * Time.deltaTime;
            m_FearLevel += (GameManager.seeingDistance - enemyDistanceToPlayer) * 0.15f * Time.deltaTime;
            m_RendererTextureCameraPlane.DizzinessSeverity += 0.0032f * Time.fixedDeltaTime;
            m_RendererTextureCameraPlane.SwayingSeverity += 0.02f * Time.fixedDeltaTime;
            m_RendererTextureCameraPlane.SwayingSpeed += 0.023f * Time.fixedDeltaTime;
        }

        // No enemies nearby, relax sanity
        else {
            m_Pulse -= 3.0f * Time.deltaTime;
            m_FearLevel -= 0.08f * Time.deltaTime;
            m_RendererTextureCameraPlane.DizzinessSeverity -= 0.005f * Time.fixedDeltaTime;
            m_RendererTextureCameraPlane.SwayingSeverity -= 0.0065f * Time.fixedDeltaTime;
            m_RendererTextureCameraPlane.SwayingSpeed -= 0.005f * Time.fixedDeltaTime;
        }


        // Settle pulse and fear level also when there is no enemies left
        if (enemies.Length == 0) {
            m_Pulse -= 10.0f * Time.deltaTime;
            m_FearLevel -= 0.2f * Time.deltaTime;
        }

        m_FearLevel = Mathf.Clamp(m_FearLevel, m_FearLevelMin, m_FearLevelMax);
        m_Pulse = Mathf.Clamp(m_Pulse, m_PulseMin, m_PulseMax);

        // Shake camera effect when player sees enemy
        m_PulseTimer += Time.deltaTime;
        if (m_PulseTimer > 60.0f / m_Pulse) {
            //StartCoroutine(CameraShakeEffect(Random.Range(0.04f, 0.1f), m_FearLevel));
            m_PulseTimer = 0.0f;
        }

        // Glitch camera effect when enemy is close to player
        if (m_FearLevel > 0.7f) {
            m_ParalyzeTimer += Time.deltaTime;
            float fearLevelNormalized = m_FearLevelMax / m_FearLevel;

            if (m_ParalyzeTimer > fearLevelNormalized * 0.25f) {
                //StartCoroutine(ParalyzeControls(Random.Range(0.02f, 0.14f)));
                m_ParalyzeTimer = 0.0f;
            }
        }
        else {
            m_ParalyzeTimer = 0.0f;
        }
    }


    // TODO:
    public void AddShivers(float strength, float length) {

    }


    /// <summary>
    /// Shakes player camera.
    /// </summary>
    /// <param name="duration">The duration of the shake.</param>
    /// <param name="severity">The severity of the shake.</param>
    private IEnumerator CameraShakeEffect(float duration, float severity) {
        Vector3 originalPos = Camera.main.transform.localPosition;
        float timeElapsed = 0.0f;

        while (timeElapsed < duration) {
            if (!m_ControlsParalyzed) {
                float x = originalPos.x + Random.Range(-1.0f, 1.0f) * severity * 0.9f;
                float y = originalPos.y + Random.Range(-1.0f, 1.0f) * severity * 0.1f;
                Camera.main.transform.localPosition = new Vector3(x, y, Camera.main.transform.localPosition.z);
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
    }


    /// <summary>
    /// Paralyzes all controls by setting timeScale to zero for the duration.
    /// </summary>
    /// <param name="duration">The duration of the freeze.</param>
    private IEnumerator ParalyzeControls(float duration) {
        float timeElapsed = 0.0f;
        float lastTime = Time.realtimeSinceStartup;

        while (timeElapsed < duration) {
            m_ControlsParalyzed = true;
            Time.timeScale = 0.0f;
            Time.fixedDeltaTime = m_FixedDeltaTime * Time.timeScale;

            timeElapsed += (Time.realtimeSinceStartup - lastTime);
            lastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        m_ControlsParalyzed = false;
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = m_FixedDeltaTime * Time.timeScale;
    }
}
