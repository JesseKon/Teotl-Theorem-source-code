using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource m_CurrentMusic = null;
    private bool m_FadeoutInProgress = false;

    private void Update() {
        
    }


    public void SetMusic(AudioSource music) {
        //if (m_CurrentMusic == music)
        //    return;

        m_CurrentMusic = music;
        m_CurrentMusic.volume = music.volume;
        m_CurrentMusic.Play();

        Debug.Log("Music set to: " + music);
    }


    public void MusicFadeOut(AudioSource music, float fadeoutTime) {
        if (m_FadeoutInProgress)
            return;

        StartCoroutine(FadeOut(music, fadeoutTime));
    }


    public void FadeOutCurrent(float fadeoutTime) {
        if (m_FadeoutInProgress)
            return;

        StartCoroutine(FadeOut(m_CurrentMusic, fadeoutTime));
    }


    private IEnumerator FadeOut(AudioSource music, float FadeoutTime) {
        if (!music) {
            Debug.Log("Music fadeout, but there was no music.");
            yield break;
        }

        Debug.Log("Music fading out...");

        m_FadeoutInProgress = true;
        float startVolume = music.volume;

        while (music.volume > 0.0f) {
            music.volume -= startVolume * Time.deltaTime / FadeoutTime;
            yield return null;
        }

        music.Stop();
        music.volume = startVolume;
        m_FadeoutInProgress = false;
    }


    public float GetMusicRemainingTime() {
        if (!m_CurrentMusic)
            return 0.0f;

        if (!m_CurrentMusic.isPlaying)
            return 0.0f;

        return Mathf.Max(m_CurrentMusic.clip.length - m_CurrentMusic.time, 0.0f);
    }


    public void StopAllMusic() {
        if (m_CurrentMusic) {
            m_CurrentMusic.Stop();
            Debug.Log("All music stopped.");
        }
    }
}
