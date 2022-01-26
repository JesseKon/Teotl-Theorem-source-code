using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTowardsPlayer : MonoBehaviour
{
    private Transform m_PlayerTransform;

    private void Start() {
        m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    private void Update()
    {
        transform.LookAt(m_PlayerTransform);
        transform.eulerAngles = new Vector3(90.0f, transform.eulerAngles.y, 0.0f);
    }
}
