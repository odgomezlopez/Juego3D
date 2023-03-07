using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPointController : MonoBehaviour
{
    //Puntos del coleccionable
    public int points;

    //Gestion de sonido
    public AudioClip audioSFX;
    private AudioSource audioSource;
    
    //Comunicacion con el controlador de Nivel
    LevelControllerBase control;

    void Start()
    {
        control = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelControllerBase>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Si es un nivel de puntos, sumo puntos
            if(control is LevelControllerPoints)
                ((LevelControllerPoints)control)?.AddPoints(points);

            //TODO lauch SFX
            //audioSource.PlayOneShot(audioSFX);
            AudioSource.PlayClipAtPoint(audioSFX,transform.position,audioSource.volume);

            //Destruyo el colecionable
            Destroy(gameObject);
        }
    }
}
