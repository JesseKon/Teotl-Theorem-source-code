using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5ShardRandom : MonoBehaviour
{
    public Transform m_Shard;
    public Transform[] m_ShardLocations;

    // Start is called before the first frame update
    private void Start() {
        int pos1 = Random.Range(0, m_ShardLocations.Length);
        int pos2 = Random.Range(0, m_ShardLocations.Length);

        while (pos1 == pos2)
            pos2 = Random.Range(0, m_ShardLocations.Length);

        Instantiate(m_Shard, m_ShardLocations[pos1].position, Quaternion.identity);
        Instantiate(m_Shard, m_ShardLocations[pos2].position, Quaternion.identity);
    }


}
