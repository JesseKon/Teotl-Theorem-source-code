using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrollingPoint : MonoBehaviour
{
    private const float m_SphereSize = 0.25f;


    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, m_SphereSize);
    }
}
