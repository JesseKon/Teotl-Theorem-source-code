using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    private static uint m_NumOfRelics;
    
    public bool AtLeastOneRelicFound {
        get { return m_AtLeastOneRelicFound; }
    }
    private bool m_AtLeastOneRelicFound = false;


    private void Start() {
        m_NumOfRelics = 0;
    }


    public uint NumOfRelics() {
        return m_NumOfRelics;
    }


    public void AddRelic() {
        ++m_NumOfRelics;
        m_AtLeastOneRelicFound = true;
        Debug.Log("Relic added. Relics owned: " + m_NumOfRelics);
    }


    public void ConsumeRelic() {
        if (m_NumOfRelics == 0) {
            Debug.LogWarning("You have zero relics and tried to consume one!");
            return;
        }

        --m_NumOfRelics;
        Debug.Log("Relic consumed. Relics remaining: " + m_NumOfRelics);
    }
}
