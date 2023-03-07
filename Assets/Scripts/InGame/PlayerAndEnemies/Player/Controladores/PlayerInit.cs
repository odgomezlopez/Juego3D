using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{
    Data data;

    private bool ready = false;
    public bool isReady => ready;

    void Start()
    {
        ready = false;
        //Obtengo los datos que se mantienen entre escenas
        data = GameObject.FindGameObjectWithTag("GameData").GetComponent<Data>();

        //Obtengo el hijo que guarda el avatar actual
        GameObject avatar = gameObject.transform.Find("Avatar").gameObject;

        //Destruyo el avatar actual si existe dentro del prefab
        if (avatar != null) Destroy(avatar);

        //Creo el nuevo avatar y lo coloco en 0,0,0 respecto a su padre
        GameObject avatarNew=Instantiate(data.currentAvatar, gameObject.transform);
        avatarNew.transform.localPosition = Vector3.zero;


        //Movemos al jugador al punto de spawn seleccionado
        try
        {
            gameObject.transform.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        }
        catch(Exception e)
        {
            Debug.Log("No creado un spawn point en el nivel actual");
        }

        ready = true;
    }

}

