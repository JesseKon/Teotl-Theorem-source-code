using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private EnemyAI m_EnemyAI;
    private GameObject m_Propeller;


    private void Start() {
        m_EnemyAI = GetComponentInParent<EnemyAI>();
        m_Propeller = transform.GetChild(0).gameObject;
    }


    private void Update() {
        //m_Head.transform.Rotate(-Vector3.forward * 80.0f * Time.deltaTime);

        if (m_EnemyAI.EnemyState == EnemyAI.State.Chasing) {
            m_Propeller.transform.localEulerAngles += new Vector3(0.0f, 0.0f, 1.0f) * 800.0f * Time.deltaTime;
        } else {
            m_Propeller.transform.localEulerAngles += new Vector3(0.0f, 0.0f, 1.0f) * 240.0f * Time.deltaTime;
        }

    }
}
