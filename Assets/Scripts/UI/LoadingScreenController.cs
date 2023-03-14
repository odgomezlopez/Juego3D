using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenController : MonoBehaviour
{
    [SerializeField]
    GameObject loadingImage;
    public void ShowLoadingScreen()
    {
        //GameObject loadingImage = gameObject.transform.GetChild(0).gameObject;
        loadingImage.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        //GameObject loadingImage = gameObject.transform.GetChild(0).gameObject;
        loadingImage.SetActive(false);
    }
}
