using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Audio;


public class GameManager : MonoBehaviour
{
    public AudioMixer m_AudioMixer;

    public float globalPickDistance = 2.0f;
    public static float pickDistance;

    public float globalSeeingDistance = 15.0f;
    public static float seeingDistance;

    private static bool m_TotemBeignDragged;
    public static readonly float totemDraggingSpeedMultiplier = 0.4f;
    public static readonly float totemDraggingMouseMultiplier = 0.4f;

    public static List<uint> energyIdsActive = new List<uint>();

    // How many game objects are beign affected. This should only be max one at any time.
    private static int m_ObjectsBeignAffected = 0;
    
    private GameObject[] m_Cubes;
    private GameObject[] m_Totems;
    private GameObject[] m_Doors;
    private GameObject[] m_Notes;

    private static GameObject m_CurrentTotemBeignDragged = null;

    private PlayerMovement m_PlayerMovement;

    // Is player shutting down (dying)?
    public bool PlayerShuttingDown {
        get { return m_PlayerShuttingDown; }
        set {
            m_PlayerShuttingDown = value;
            m_PauseMenu.SetActive(false);
        }
    }
    private bool m_PlayerShuttingDown = false;

    // Is player in main menu?
    public bool PlayerInMainMenu {
        get { return m_PlayerInMainMenu; }
        set { m_PlayerInMainMenu = value; }
    }
    private bool m_PlayerInMainMenu = false;

    // Is player in pause menu?
    public bool PlayerInPauseMenu {
        get { return m_PlayerInPauseMenu; }
    }
    private bool m_PlayerInPauseMenu = false;

    private readonly Color m_PauseMenuBackgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.8f);

    private SpriteRenderer m_BlackScreen;

    public int MouseSensitivity {
        get { return m_MouseSensitivity; }
        set {
            m_MouseSensitivity = Mathf.Clamp(value, 0, m_ActualMouseSensitivities.Length - 1);
        }
    }
    private int m_MouseSensitivity;

    public float ActualMouseSensitivity {
        get { return m_ActualMouseSensitivities[m_MouseSensitivity]; }
    }
    private float[] m_ActualMouseSensitivities = new float[10] { 1.0f, 1.33f, 1.66f, 2.0f, 2.33f, 2.66f, 3.0f, 3.33f, 3.66f, 4.0f };
    private const int m_MouseSensitivityDefault = 4;

    public int AudioMasterVolume {
        get { return m_AudioMasterVolume; }
        set {
            m_AudioMasterVolume = Mathf.Clamp(value, 0, m_ActualAudioMasterVolumes.Length - 1);
            m_AudioMixer.SetFloat("MasterVolume", m_ActualAudioMasterVolumes[m_AudioMasterVolume]);
        }
    }
    private int m_AudioMasterVolume;
    private float[] m_ActualAudioMasterVolumes = new float[10] { -48.0f, -36.0f, -24.0f, -18.0f, -12.0f, -9.0f, -6.0f, -3.0f, -1.5f, 0.0f };
    private const int m_AudioMasterVolumeDefault = 8;

    private uint NumOfShards;
    
    public bool AllowPauseMenu {
        get { return m_AllowPauseMenu; }
        set { m_AllowPauseMenu = value; }
    }
    private bool m_AllowPauseMenu = false;

    private GameObject m_PauseMenu;

    public bool TrueEndingShutDown {
        set { m_TrueEndingShutDown = value; }
    }
    private bool m_TrueEndingShutDown = false;

    // Has player reached the checkpoint
    public bool CheckpointReached {
        get { return m_CheckpointReached; }
        set { 
            m_CheckpointReached = value;
            if (!m_CheckpointReached)
                GameObject.FindGameObjectWithTag("ShardManager").GetComponent<ShardManager>().SetNumOfShards(0);

            SaveData(); 
        }
    }
    private bool m_CheckpointReached = false;


    private void Awake() {
        DontDestroyOnLoad(gameObject);

        pickDistance = globalPickDistance;
        seeingDistance = globalSeeingDistance;
        m_TotemBeignDragged = false;
        LoadSavedData();
    }


    private void Start() {
        m_Cubes = GameObject.FindGameObjectsWithTag("PickableCube");
        m_Totems = GameObject.FindGameObjectsWithTag("Totem");
        m_Doors = GameObject.FindGameObjectsWithTag("ManualDoorHandler");
        m_Notes = GameObject.FindGameObjectsWithTag("Note");

        m_PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        m_BlackScreen = GameObject.Find("Sprite (Blackscreen)").GetComponent<SpriteRenderer>(); // TODO: spooky to try to find with name. Maybe create own black screen for this?
        m_AllowPauseMenu = false;
        m_PlayerInPauseMenu = false;

        m_PauseMenu = GameObject.Find("PauseMenu");
        m_PauseMenu.SetActive(false);

    }


    private void Update() {
        foreach (GameObject totem in m_Totems) {
            if (totem.GetComponent<Totem>().IsBeignDragged()) {
                m_TotemBeignDragged = true;
                m_CurrentTotemBeignDragged = totem;
                break;
            } else {
                m_TotemBeignDragged = false;
                m_CurrentTotemBeignDragged = null;
            }
        }

        PauseMenu();
    }


    public void UpdateTotems() {
        m_Totems = GameObject.FindGameObjectsWithTag("Totem");
    }


    /// <summary>
    /// Is mouse control stolen at the moment.
    /// </summary>
    /// <returns>True if mouse control is stolen at the moment, false otherwise.</returns>
    public bool IsMouseControlStolen() {
        foreach (GameObject cube in m_Cubes) {
            if (cube.GetComponent<PickableCube>().stealMouseControl)
                return true;
        }

        foreach (GameObject door in m_Doors) {
            if (door.GetComponent<ManualDoorHandler>().stealMouseControl)
                return true;
        }

        foreach (GameObject note in m_Notes) {
            if (note.GetComponent<Note>().IsNotePickedUp())
                return true;
        }

        return false;
    }


    /// <summary>
    /// Is a totem currently beign dragged.
    /// </summary>
    /// <returns>True if a totem is beign dragged, false otherwise.</returns>
    public static bool IsTotemBeignDragged() {
        return m_TotemBeignDragged;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static float GetTotemDragDistance() {
        return m_CurrentTotemBeignDragged.GetComponent<Totem>().DragDistance();
    }


    /// <summary>
    /// Add one object to the list that are beign affected.
    /// </summary>
    public static void BeginObjectBeignAffected() {
        if (m_ObjectsBeignAffected >= 1)
            Debug.LogWarning("Trying to add object to being affected, but more than one object is beign affected already!");

        ++m_ObjectsBeignAffected;
    }


    /// <summary>
    /// Subtract one object from the list that are beign affected.
    /// </summary>
    public static void EndObjectBeignAffected() {
        if (m_ObjectsBeignAffected <= 0)
            Debug.LogWarning("Trying to subtract object beign affected, but there is no objects beign affected at the moment!");

        --m_ObjectsBeignAffected;
    }


    /// <summary>
    /// How many objects are beign affected at the moment.
    /// </summary>
    /// <returns>The number of objects beign affected at the moment.</returns>
    public static int HowManyObjectsBeignAffected() {
        return m_ObjectsBeignAffected;
    }


    private void PauseMenu() {

        // Start pause menu
        if (Input.GetKeyDown(KeyCode.Escape) && !m_PlayerInPauseMenu && m_AllowPauseMenu && !m_PlayerShuttingDown && !m_PlayerMovement.Jumping) {

            m_PlayerInPauseMenu = true;
            m_BlackScreen.color = m_PauseMenuBackgroundColor;
            m_PauseMenu.SetActive(true);
            m_PauseMenu.GetComponent<PauseMenu>().JustActivated();

            Time.timeScale = 0.0f;
            m_AudioMixer.SetFloat("SubMaster", -80.0f);

            CursorManager.HideCursor();
            //GameObject.FindGameObjectWithTag("CursorManager").GetComponent<CursorManager>().ShowActualCursor();

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerMouseLook>().LockMouseControls = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().LockMovementControls = true;
            GameObject.FindGameObjectWithTag("ObjectiveHud").GetComponent<ObjectiveHud>().AllowInventory = false;
            Debug.Log("Player opened pause menu");
            return;
        }

        // Exit pause menu
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.C)) && m_PlayerInPauseMenu) {
            m_PlayerInPauseMenu = false;
            m_BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            m_PauseMenu.SetActive(false);

            Time.timeScale = 1.0f;
            m_AudioMixer.SetFloat("SubMaster", 0.0f);

            if (!m_TrueEndingShutDown) {
                CursorManager.ShowCursor();
                //GameObject.FindGameObjectWithTag("CursorManager").GetComponent<CursorManager>().HideActualCursor();

                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerMouseLook>().LockMouseControls = false;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().LockMovementControls = false;
                GameObject.FindGameObjectWithTag("ObjectiveHud").GetComponent<ObjectiveHud>().AllowInventory = true;
            }

            Debug.Log("Player closed pause menu.");
            SaveData();
            return;
        }

    }


    private void LoadSavedData() {
        string filePath = Application.persistentDataPath + "/data.dat";

        if (!File.Exists(filePath))
            CreateSaveData();

        FileStream file;
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        try {
            file = File.OpenRead(filePath);
            SaveData saveData = (SaveData)binaryFormatter.Deserialize(file);
            MouseSensitivity = saveData.MouseSensitivity;
            AudioMasterVolume = saveData.AudioMasterVolume;
            NumOfShards = saveData.ShardsOwned;
            CheckpointReached = saveData.CheckpointReached;

            // Give player one shard if player has at least one
            if (NumOfShards >= 1)
                GameObject.FindGameObjectWithTag("ShardManager").GetComponent<ShardManager>().SetNumOfShards(1);

            file.Close();
            Debug.Log("Loaded save file at " + filePath);
            Debug.Log("[sens: " + m_MouseSensitivity + ", vol: " + m_AudioMasterVolume + ", shards: " + NumOfShards + ", checkpoint: " + m_CheckpointReached + "]");
        }
        catch {
            Debug.LogWarning("Save file corrupted at " + filePath);
            CreateSaveData();
        }
    }


    private void CreateSaveData() {
        string filePath = Application.persistentDataPath + "/data.dat";
        FileStream file;
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        if (!File.Exists(filePath)) {
            file = File.Create(filePath);
            Debug.Log("Created new save file at " + filePath);
        }
        else {
            file = File.OpenWrite(filePath);
        }

        SaveData saveData = new SaveData(m_MouseSensitivityDefault, m_AudioMasterVolumeDefault, 0, false);
        binaryFormatter.Serialize(file, saveData);
        file.Close();

        MouseSensitivity = m_MouseSensitivityDefault;
        AudioMasterVolume = m_AudioMasterVolumeDefault;
    }


    /// <summary>
    /// Saves the current state of mouse sensitivity and audio master volume
    /// </summary>
    public void SaveData() {
        string filePath = Application.persistentDataPath + "/data.dat";
        FileStream file;
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        try {
            file = File.OpenWrite(filePath);
            uint shards = GameObject.FindGameObjectWithTag("ShardManager").GetComponent<ShardManager>().NumOfShards();
            SaveData saveData = new SaveData(m_MouseSensitivity, m_AudioMasterVolume, shards, m_CheckpointReached);
            binaryFormatter.Serialize(file, saveData);
            file.Close();
            Debug.Log("Game data saved. [sens: " + m_MouseSensitivity + ", vol: " + m_AudioMasterVolume + ", shards: " + shards + ", checkpoint: " + m_CheckpointReached + "]");
        }
        catch {
            Debug.LogWarning("Data couldn't be saved!");
        }
    }
}
