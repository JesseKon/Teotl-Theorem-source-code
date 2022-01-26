[System.Serializable]
public class SaveData
{
    public int MouseSensitivity;
    public int AudioMasterVolume;
    public uint ShardsOwned;
    public bool CheckpointReached;

    public SaveData(int mouseSensitivity, int audioMasterVolume, uint shardsOwned, bool checkpointReached) {
        MouseSensitivity = mouseSensitivity;
        AudioMasterVolume = audioMasterVolume;
        ShardsOwned = shardsOwned;
        CheckpointReached = checkpointReached;
    }
}