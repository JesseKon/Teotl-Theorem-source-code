using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private SoundManager m_SoundManager;
    private AudioSource m_SmackSound;
    private AudioSource m_Beep2Sound;

    private OnScreenInformation m_OnScreenInformation;

    public bool OnEndingScreen {
        get { return m_OnEndingScreen; }
    }
    private bool m_OnEndingScreen = false;


    private void OnDrawGizmos() {
        GetComponent<SpriteRenderer>().enabled = false;
    }


    private void Start() {
        m_SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        m_Beep2Sound = GameObject.FindGameObjectWithTag("SoundManager").transform.GetChild(1).GetComponent<AudioSource>();

        m_OnScreenInformation = GameObject.Find("OnScreenInformation").GetComponent<OnScreenInformation>();
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }


    public void Stunned(Vector3 newSpawnPoint, Quaternion newSpawnRotation, float blackoutTime, float recoveryTime) {
        m_SmackSound = Instantiate(m_SoundManager.m_SmackSound);
        m_SmackSound.Play();
        Destroy(m_SmackSound.gameObject, m_SmackSound.clip.length);
        StartCoroutine(StunnedCoroutine(newSpawnPoint, newSpawnRotation, blackoutTime, recoveryTime));
    }


    private IEnumerator StunnedCoroutine(Vector3 newSpawnPoint, Quaternion newSpawnRotation, float blackoutTime, float recoveryTime) {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().LockMovementControls = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerMouseLook>().LockMouseControls = true;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllowPauseMenu = false;
        GameObject.FindGameObjectWithTag("Player").transform.position = newSpawnPoint;
        GameObject.FindGameObjectWithTag("Player").transform.rotation = newSpawnRotation;

        GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        yield return new WaitForSeconds(blackoutTime);

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().LockMovementControls = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerMouseLook>().LockMouseControls = false;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllowPauseMenu = true;

        float alpha = 1.0f;
        while (alpha > 0) {
            alpha -= Time.deltaTime / recoveryTime;
            GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, alpha);
            yield return null;
        }
    }


    // Kill the player
    public void ShutPlayerDown(bool playSound = true, bool blackout = true, bool trueEndingShutDown = false, string endingText = "", string quoteText = "") {
        GameObject.FindGameObjectWithTag("ObjectiveHud").GetComponent<ObjectiveHud>().AllowInventory = false;

        if (playSound) {
            m_SmackSound = Instantiate(m_SoundManager.m_SmackSound);
            m_SmackSound.Play();
            Destroy(m_SmackSound.gameObject, m_SmackSound.clip.length);
        }

        // This is the true ending, let player listen to the ending music
        if (trueEndingShutDown) {
            GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.Find("SpawnAtNowhere").transform.position;
            Camera.main.transform.rotation = GameObject.Find("SpawnAtNowhere").transform.rotation;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerMouseLook>().LockMouseControls = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().LockMovementControls = true;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().TrueEndingShutDown = true;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllowPauseMenu = false;
            CursorManager.HideCursor();

            m_OnEndingScreen = true;

            // Reset checkpoint
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CheckpointReached = false;

            StartCoroutine(TheEndTextFadeIn(endingText));
            StartCoroutine(TheEndQuoteFadeIn(quoteText));

            MusicManager musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
            StartCoroutine(ShowExitMessages(musicManager.GetMusicRemainingTime() + 6.0f));
            StartCoroutine(QuitGame(musicManager.GetMusicRemainingTime() + 6.0f + 4.8f));
        }

        // Not true ending, stop music and start blackout coroutine
        else {
            GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>().StopAllMusic();
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerShuttingDown = true;

            // Spawn player far from the game area
            GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.Find("SpawnAtNowhere").transform.position;
            Camera.main.transform.rotation = GameObject.Find("SpawnAtNowhere").transform.rotation;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerMouseLook>().LockMouseControls = true;

            if (blackout)
                StartCoroutine(BlackOut(8.0f));

            StartCoroutine(ShowExitMessages());
            StartCoroutine(ExitToMainMenu(8.0f));
            //StartCoroutine(QuitGame(4.8f));
        }

    }


    private IEnumerator TheEndTextFadeIn(string endingText) {
        yield return new WaitForSeconds(1.0f);
        float alpha = 0.0f;
        while (alpha < 1.0f) {
            alpha += Time.deltaTime / 2.0f;
            m_OnScreenInformation.ShowStaticMessage(endingText, alpha);
            yield return null;
        }
    }

    private IEnumerator TheEndQuoteFadeIn(string quoteText) {
        yield return new WaitForSeconds(1.0f);
        float alpha = 0.0f;
        while (alpha < 1.0f) {
            alpha += Time.deltaTime / 2.0f;
            m_OnScreenInformation.ShowStaticQuoteMessage(quoteText, alpha);
            yield return null;
        }
    }


    private IEnumerator BlackOut(float time) {
        yield return m_OnScreenInformation.BlackoutEnumerator(0.0f, 0.0f, time, 0.0f);
    }


    private IEnumerator ShowExitMessages(float initialDelay = 0.0f) {
        yield return new WaitForSeconds(initialDelay);
        yield return m_OnScreenInformation.ShowMessageEnumerator("Shutting down...", 0.8f, 0.0f, 0.5f, 0.0f);
        m_Beep2Sound.Play();
        yield return m_OnScreenInformation.ShowMessageEnumerator("Shutting down...3", 0.0f, 0.0f, 1.5f, 0.0f);
        m_Beep2Sound.Play();
        yield return m_OnScreenInformation.ShowMessageEnumerator("Shutting down...2", 0.0f, 0.0f, 1.5f, 0.0f);
        m_Beep2Sound.Play();
        yield return m_OnScreenInformation.ShowMessageEnumerator("Shutting down...1", 0.0f, 0.0f, 1.5f, 0.0f);
    }


    public IEnumerator QuitGame(float delay) {
        yield return new WaitForSeconds(delay);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    private IEnumerator ExitToMainMenu(float delay) {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

}
