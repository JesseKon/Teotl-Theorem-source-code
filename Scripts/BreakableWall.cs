using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public Texture[] m_WallTextures;

    private int m_WallStatus = 0;  // How broken the wall is
    private Material m_Material = null;

    private const float m_TimeUntilNextHit = 1.0f;
    private float m_Timer = 0.0f;

    private const float m_MinimalHitImpulse = 18.0f;

    private AudioSource m_CrackSound;
    private AudioSource m_BreakSound;


    private void Start() {
        m_Material = GetComponent<MeshRenderer>().material;
        m_Material.SetTexture("_MainTex", m_WallTextures[0]);
        GetComponent<MeshRenderer>().material = m_Material;

        m_CrackSound = transform.GetChild(0).GetComponent<AudioSource>();
        m_BreakSound = transform.GetChild(1).GetComponent<AudioSource>();
    }


    private void Update() {
        m_Timer += Time.deltaTime;
    }


    private void OnCollisionEnter(Collision collision) {
        Debug.Log(collision.collider.name + " hit breakable wall at magnitude " + Vector3.Magnitude(collision.impulse));

        if (collision.collider.CompareTag("PickableCube") && m_Timer > m_TimeUntilNextHit && Vector3.Magnitude(collision.impulse) > m_MinimalHitImpulse) {

            if (++m_WallStatus >= m_WallTextures.Length) {
                BreakWall();
                return;
            }

            m_CrackSound.Play();

            m_Material.SetTexture("_MainTex", m_WallTextures[m_WallStatus]);
            m_Timer = 0.0f;
        }
    }


    public void BreakWall() {
        m_BreakSound.Play();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        Destroy(gameObject, 2.0f);
    }
}
