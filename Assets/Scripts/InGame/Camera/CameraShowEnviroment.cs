using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShowEnviroment : MonoBehaviour
{
    CinemachineVirtualCamera cineCamera;
    Animator animator;
    PlayerController playerController;
    LevelControllerBase levelController;

    IEnumerator Start()
    {
        cineCamera = GetComponent<CinemachineVirtualCamera>();
        animator = GetComponent<Animator>();
        levelController = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelControllerBase>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();


        //Desactivo el GamePlay
        yield return new WaitUntil(() => (playerController.IsReady()));
        yield return new WaitUntil(() => (levelController.IsReady()));

        playerController.DisableGamePlay();
        animator.enabled = true;
        //Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        //Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);//

        cineCamera.Priority = 0;

        //yield return new WaitForSecondsRealtime(0.5f);
        playerController.EnableGamePlay();
        yield return null;
    }
}
