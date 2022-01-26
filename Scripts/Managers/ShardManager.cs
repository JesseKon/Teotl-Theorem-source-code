using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardManager : MonoBehaviour
{
    private uint m_NumOfShards;

    public bool AtLeastOneShardFound {
        get { return m_AtLeastOneShardFound; }
    }
    private bool m_AtLeastOneShardFound = false;


    private void Start() {
        //m_NumOfShards = 0;
    }


    public uint NumOfShards() {
        return m_NumOfShards;
    }


    public void SetNumOfShards(uint numOfShards) {
        m_NumOfShards = numOfShards;
    }


    public void AddShard() {
        ++m_NumOfShards;
        m_AtLeastOneShardFound = true;
        Debug.Log("Shard added. Shards owned: " + m_NumOfShards);
    }


    public void ConsumeShard() {
        if (m_NumOfShards == 0) {
            Debug.LogWarning("You have zero shards and tried to consume one!");
            return;
        }

        --m_NumOfShards;
        Debug.Log("Shard consumed. Shards remaining: " + m_NumOfShards);
    }
}
