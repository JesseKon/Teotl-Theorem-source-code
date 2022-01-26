using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemySounds : MonoBehaviour
{
    private SoundManager m_SoundManager;
    private AudioSource m_AudioSourceToPlay;

    private EnemyAI m_EnemyAI;

    private AudioSource m_Propeller;
    private AudioSource m_PropellerDistorted;

    private AudioSource[] m_PatrollingSounds;
    private AudioSource[] m_GruntingSounds;
    private AudioSource[] m_HissingSounds;
    private AudioSource[] m_ChasingSounds;

    private int m_PreviousSoundIndex = 0;

    private float m_PropellerPitch = 0.0f;
    private float m_PropellerVolume = 0.0f;
    private float m_PropellerDistortedPitch = 0.0f;
    private float m_PropellerDistortedVolume = 0.0f;

    public enum SoundType
    {
        Patrolling,
        Grunting,
        Hissing,
        Chasing
    }


    private void Start() {
        //m_SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();

        //if (m_SoundManager.m_EnemyPatrollingSounds.Length < 2)
        //    Debug.LogError("Must have more than 2 enemy patrolling sounds!");

        //if (m_SoundManager.m_EnemyGruntingSounds.Length < 2)
        //    Debug.LogError("Must have more than 2 enemy grunting sounds!");

        //if (m_SoundManager.m_EnemyHissingSounds.Length < 2)
        //    Debug.LogError("Must have more than 2 enemy hissing sounds!");

        //if (m_SoundManager.m_EnemyChasingSounds.Length < 2)
        //    Debug.LogError("Must have more than 2 enemy chasing sounds!");

        m_EnemyAI = GetComponent<EnemyAI>();

        m_PatrollingSounds = transform.GetChild(3).GetComponentsInChildren<AudioSource>();
        m_GruntingSounds = transform.GetChild(4).GetComponentsInChildren<AudioSource>();
        m_HissingSounds = transform.GetChild(5).GetComponentsInChildren<AudioSource>();
        m_ChasingSounds = transform.GetChild(6).GetComponentsInChildren<AudioSource>();

        m_Propeller = transform.GetChild(2).GetChild(0).GetComponent<AudioSource>();
        m_PropellerDistorted = transform.GetChild(2).GetChild(1).GetComponent<AudioSource>();
    }


    private void Update() {

        if (m_EnemyAI.EnemyState == EnemyAI.State.Idle) {
            m_PropellerPitch = Mathf.Lerp(m_PropellerPitch, 0.1f, 10.0f * Time.deltaTime);
            m_PropellerVolume = Mathf.Lerp(m_PropellerVolume, 0.3f, 10.0f * Time.deltaTime);
            m_PropellerDistortedPitch = Mathf.Lerp(m_PropellerDistortedPitch, 0.1f, 10.0f * Time.deltaTime);
            m_PropellerDistortedVolume = Mathf.Lerp(m_PropellerDistortedVolume, 0.3f, 10.0f * Time.deltaTime);
        }

        else if (m_EnemyAI.EnemyState == EnemyAI.State.Patrolling) {
            m_PropellerPitch = Mathf.Lerp(m_PropellerPitch, 0.5f, 5.0f * Time.deltaTime);
            m_PropellerVolume = Mathf.Lerp(m_PropellerVolume, 0.8f, 5.0f * Time.deltaTime);
            m_PropellerDistortedPitch = Mathf.Lerp(m_PropellerDistortedPitch, 0.3f, 5.0f * Time.deltaTime);
            m_PropellerDistortedVolume = Mathf.Lerp(m_PropellerDistortedVolume, 0.3f, 5.0f * Time.deltaTime);
        }
        
        else if (m_EnemyAI.EnemyState == EnemyAI.State.Chasing) {
            m_PropellerPitch = Mathf.Lerp(m_PropellerPitch, 1.0f, 10.0f * Time.deltaTime);
            m_PropellerVolume = Mathf.Lerp(m_PropellerVolume, 1.0f, 10.0f * Time.deltaTime);
            m_PropellerDistortedPitch = Mathf.Lerp(m_PropellerDistortedPitch, 1.0f, 10.0f * Time.deltaTime);
            m_PropellerDistortedVolume = Mathf.Lerp(m_PropellerDistortedVolume, 1.0f, 10.0f * Time.deltaTime);
        }


        m_Propeller.pitch = m_PropellerPitch;
        m_Propeller.volume = m_PropellerVolume;
        m_PropellerDistorted.pitch = m_PropellerDistortedPitch;
        m_PropellerDistorted.volume = m_PropellerDistortedVolume;
    }


    public void PlayRandomSound(SoundType soundType, out float audioLength) {
        audioLength = 0.0f;

        // Patrolling sound
        if (soundType == SoundType.Patrolling) {
            //int newIndex = Random.Range(0, m_SoundManager.m_EnemyPatrollingSounds.Length);
            //while (newIndex == m_PreviousSoundIndex)
            //    newIndex = Random.Range(0, m_SoundManager.m_EnemyPatrollingSounds.Length);

            //m_PreviousSoundIndex = newIndex;

            //m_AudioSourceToPlay = Instantiate(m_SoundManager.m_EnemyPatrollingSounds[newIndex], transform);
            //m_AudioSourceToPlay.Play();

            //audioLength = m_AudioSourceToPlay.clip.length;
            //Destroy(m_AudioSourceToPlay.gameObject, m_AudioSourceToPlay.clip.length);

            int newIndex = Random.Range(0, m_PatrollingSounds.Length);
            while (newIndex == m_PreviousSoundIndex)
                newIndex = Random.Range(0, m_PatrollingSounds.Length);

            m_PreviousSoundIndex = newIndex;

            m_PatrollingSounds[newIndex].Play();

            audioLength = m_PatrollingSounds[newIndex].clip.length;

            return;
        }

        // Grunting sound
        else if (soundType == SoundType.Grunting) {
            //int newIndex = Random.Range(0, m_SoundManager.m_EnemyGruntingSounds.Length);
            //while (newIndex == m_PreviousSoundIndex)
            //    newIndex = Random.Range(0, m_SoundManager.m_EnemyGruntingSounds.Length);

            //m_PreviousSoundIndex = newIndex;

            //m_AudioSourceToPlay = Instantiate(m_SoundManager.m_EnemyGruntingSounds[newIndex], transform);
            //m_AudioSourceToPlay.Play();

            //audioLength = m_AudioSourceToPlay.clip.length;
            //Destroy(m_AudioSourceToPlay.gameObject, m_AudioSourceToPlay.clip.length);

            //return;

            int newIndex = Random.Range(0, m_GruntingSounds.Length);
            while (newIndex == m_PreviousSoundIndex)
                newIndex = Random.Range(0, m_GruntingSounds.Length);

            m_PreviousSoundIndex = newIndex;

            m_GruntingSounds[newIndex].Play();

            audioLength = m_GruntingSounds[newIndex].clip.length;

            return;
        }

        // Hissing sound
        else if (soundType == SoundType.Hissing) {
            //int newIndex = Random.Range(0, m_SoundManager.m_EnemyHissingSounds.Length);
            //while (newIndex == m_PreviousSoundIndex)
            //    newIndex = Random.Range(0, m_SoundManager.m_EnemyHissingSounds.Length);

            //m_PreviousSoundIndex = newIndex;

            //m_AudioSourceToPlay = Instantiate(m_SoundManager.m_EnemyHissingSounds[newIndex], transform);
            //m_AudioSourceToPlay.Play();

            //audioLength = m_AudioSourceToPlay.clip.length;
            //Destroy(m_AudioSourceToPlay.gameObject, m_AudioSourceToPlay.clip.length);

            //return;

            int newIndex = Random.Range(0, m_HissingSounds.Length);
            while (newIndex == m_PreviousSoundIndex)
                newIndex = Random.Range(0, m_HissingSounds.Length);

            m_PreviousSoundIndex = newIndex;

            m_HissingSounds[newIndex].Play();

            audioLength = m_HissingSounds[newIndex].clip.length;

            return;
        }

        // Chasing sound
        else if (soundType == SoundType.Chasing) {
            //int newIndex = Random.Range(0, m_SoundManager.m_EnemyChasingSounds.Length);
            //while (newIndex == m_PreviousSoundIndex)
            //    newIndex = Random.Range(0, m_SoundManager.m_EnemyChasingSounds.Length);

            //m_PreviousSoundIndex = newIndex;

            //m_AudioSourceToPlay = Instantiate(m_SoundManager.m_EnemyChasingSounds[newIndex], transform);
            //m_AudioSourceToPlay.Play();

            //audioLength = m_AudioSourceToPlay.clip.length;
            //Destroy(m_AudioSourceToPlay.gameObject, m_AudioSourceToPlay.clip.length);

            //return;

            int newIndex = Random.Range(0, m_ChasingSounds.Length);
            while (newIndex == m_PreviousSoundIndex)
                newIndex = Random.Range(0, m_ChasingSounds.Length);

            m_PreviousSoundIndex = newIndex;

            m_ChasingSounds[newIndex].Play();

            audioLength = m_ChasingSounds[newIndex].clip.length;

            return;
        }

    }

}
