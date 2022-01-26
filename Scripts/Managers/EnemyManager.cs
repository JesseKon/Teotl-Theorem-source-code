using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] public bool WokeUpInPrisonCell {
        get { return m_WokeUpInPrisonCell; }
        set { m_WokeUpInPrisonCell = value; }
    }
    private bool m_WokeUpInPrisonCell = false;

}
