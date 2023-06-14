using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeController : MonoBehaviour
{
    public Volume volume;
    Vignette vignetteFilter;
    void Start()
    {
        volume= gameObject.GetComponent<Volume>();
        
        Vignette tmp;
        if (volume.profile.TryGet<Vignette>(out tmp))
        {
            vignetteFilter = tmp;
        }
    }

    // Update is called once per frame
    public void ToogleVignette()
    {
        vignetteFilter.active = !vignetteFilter.active;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToogleVignette();
        }
    }


}
