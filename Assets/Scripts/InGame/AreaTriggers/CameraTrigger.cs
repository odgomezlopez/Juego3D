using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraTrigger : MonoBehaviour
{
    CinemachineVirtualCamera cameraCine;
    public bool disableOnEnd = true;

    void Start()
    {
        cameraCine = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || (other.transform.parent && other.transform.parent.CompareTag("Player")))
        {
            cameraCine.Priority = 1000;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || (other.transform.parent && other.transform.parent.CompareTag("Player")))
        {
            cameraCine.Priority = 1;
            if (disableOnEnd) Destroy(this);
        }
    }
}
