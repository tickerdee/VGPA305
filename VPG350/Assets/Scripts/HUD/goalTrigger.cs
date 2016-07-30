using UnityEngine;
using System.Collections;

public class goalTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider triggerCube)
    {
        Time.timeScale = 0;
    }
}
