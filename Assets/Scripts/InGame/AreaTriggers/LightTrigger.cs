using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    Light luz;
    public float targetValue=500;
    public float changePerFrame = 20f;

    Coroutine corutina;
    public bool disableOnEnd = true;
    void Start()
    {
        luz = GetComponentInChildren<Light>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (corutina!=null) StopCoroutine(corutina);
            corutina=StartCoroutine(increaseLight(targetValue));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (corutina != null) StopCoroutine(corutina);
            corutina = StartCoroutine(decreaseLight(0));
        }
    }

    private IEnumerator increaseLight(float targetValue) {
        luz.enabled = true;
        while (luz.intensity < targetValue)
        {
            luz.intensity+= changePerFrame;
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

    private IEnumerator decreaseLight(float targetValue)
    {
        luz.enabled = true;
        while (luz.intensity > targetValue)
        {
            luz.intensity -= changePerFrame;
            yield return new WaitForFixedUpdate();
        }

        if (luz.intensity == 0)
        {
            luz.enabled = false;
            if (disableOnEnd) Destroy(this);
        }

        yield return null;
    }
}
