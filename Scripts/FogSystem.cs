using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogSystem : MonoBehaviour
{
    public enum EnvironmentAttributesEnum
    {
        Normal,
        DarkRoom,
        RedRoom
    }


    [System.Serializable]
    public struct EnvironmentAttributes {
        public Color m_AmbientColor;
        public FogMode m_FogMode;
        public Color m_FogColor;
        public float m_FogDensity;
    }


    public EnvironmentAttributes m_NormalEnvironment;
    public EnvironmentAttributes m_DarkRoomEnvironment;
    public EnvironmentAttributes m_RedRoomEnvironment;

    [Space]

    public EnvironmentAttributes m_TrueEndingEnvironment;
    public EnvironmentAttributes m_FakeEndingEnvironment;


    private bool m_RenderSettingJustSet = false;
    private float m_Timer = 0.0f;

    private EnvironmentAttributes m_CurrentAttributes;
    private EnvironmentAttributes m_NewAttributes;

    private const float m_FadeTime = 1.5f; // In seconds

    private void Start() {
        RenderSettings.fog = true;
        m_CurrentAttributes = m_NewAttributes = m_NormalEnvironment;
        m_RenderSettingJustSet = true;
    }


    private void Update() {
        if (m_RenderSettingJustSet) {
            m_Timer = 0.0f;
            RenderSettings.fogMode = m_NewAttributes.m_FogMode;
            m_RenderSettingJustSet = false;
        }

        m_Timer += Time.deltaTime;
        m_Timer = Mathf.Clamp(m_Timer, 0.0f, m_FadeTime);

        SmoothTransit(m_CurrentAttributes, m_NewAttributes, m_Timer / m_FadeTime);
    }


    private void SmoothTransit(EnvironmentAttributes currentAttributes, EnvironmentAttributes newAttributes, float t) {
        RenderSettings.ambientEquatorColor = Color.Lerp(currentAttributes.m_AmbientColor, newAttributes.m_AmbientColor, t);
        RenderSettings.fogColor = Color.Lerp(currentAttributes.m_FogColor, newAttributes.m_FogColor, t);
        RenderSettings.fogDensity = Mathf.Lerp(currentAttributes.m_FogDensity, newAttributes.m_FogDensity, t);
    }


    public void SetToNormalEnvironment() {
        m_CurrentAttributes = m_NormalEnvironment;
        m_NewAttributes = m_NormalEnvironment;
        m_RenderSettingJustSet = true;
    }


    public IEnumerator EnterDarkRoom() {
        yield return new WaitForSeconds(m_FadeTime);
        m_CurrentAttributes = m_NormalEnvironment;
        m_NewAttributes = m_DarkRoomEnvironment;
        m_RenderSettingJustSet = true;
    }


    public IEnumerator ExitDarkRoom() {
        yield return new WaitForSeconds(m_FadeTime);
        m_CurrentAttributes = m_DarkRoomEnvironment;
        m_NewAttributes = m_NormalEnvironment;
        m_RenderSettingJustSet = true;
    }


    public IEnumerator EnterRedRoom() {
        yield return new WaitForSeconds(m_FadeTime);
        m_CurrentAttributes = m_NormalEnvironment;
        m_NewAttributes = m_RedRoomEnvironment;
        m_RenderSettingJustSet = true;
    }


    public IEnumerator ExitRedRoom() {
        yield return new WaitForSeconds(m_FadeTime);
        m_CurrentAttributes = m_RedRoomEnvironment;
        m_NewAttributes = m_NormalEnvironment;
        m_RenderSettingJustSet = true;
    }


    public IEnumerator SetEnvironmentToRed() {
        yield return new WaitForSeconds(0.1f);
        m_CurrentAttributes = m_NormalEnvironment;
        m_NewAttributes = m_FakeEndingEnvironment;
        m_RenderSettingJustSet = true;
    }

}
