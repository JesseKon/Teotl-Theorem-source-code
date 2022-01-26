using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level7_EndingLogic : MonoBehaviour
{
    public GameObject m_Door;

    private TriggerZoneLogics m_Trigger;


    private void Start() {
        m_Trigger = GetComponent<TriggerZoneLogics>();
    }


    private void FixedUpdate() {
        if (m_Trigger.Enter) {
            m_Door.GetComponent<ElectricDoor>().ForceCloseDoor();
        }
    }
}
