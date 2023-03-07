using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSoundController : MonoBehaviour
{
    //Gestion de sonido
    private AudioSource audioSource;
    public Stats stats;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        stats = GetComponent<IGenericController>().GetStats();
    }

    public void OnAttackSound()
    {
        if(stats==null) stats = GetComponent<IGenericController>().GetStats();
        //if (audioSource.isPlaying) return;
        audioSource.PlayOneShot(stats.attackSFX);
    }

    public void OnDamageSound()
    {
        if (stats == null) stats = GetComponent<IGenericController>().GetStats();

        //if (audioSource.isPlaying) return;
        audioSource.PlayOneShot(stats.damageSFX);
    }
}
