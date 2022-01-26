using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonBreakableWallLogic : MonoBehaviour
{
    public GameObject m_Cube;
    public GameObject m_BreakableWall;

    private bool m_WallBroken = false;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == m_Cube.gameObject && !m_WallBroken) {
            Debug.Log("Force break breakable wall at the prison");
            if (m_BreakableWall)
                m_BreakableWall.GetComponent<BreakableWall>().BreakWall();

            m_WallBroken = true;
        }
    }
}
