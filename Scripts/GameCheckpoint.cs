using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player reached the checkpoint");
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CheckpointReached = true;
            gameObject.SetActive(false);
        }
    }
}
