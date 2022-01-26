using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideIfPlayerSees : MonoBehaviour
{

    // Update is called once per frame
    private void Update() {
        float angle = Vector3.Angle(Camera.main.transform.forward, transform.position - Camera.main.transform.position);

        if (angle < 90.0f) {
            transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
            transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop();
        }
        else {
            transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
        }
    }

}
