using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Linq;

public class LevelControllerBase : MonoBehaviour, IReadied
{
    //Atributos
    protected Data data;
    protected PlayerController playerController;
    protected ControlHUD controlHUD;
    protected LoadingScreenController loadingScreenController;

    protected float levelInitTime;

    [SerializeField]
    protected string nextLevelWin;

    [SerializeField]
    protected string nextLevelGameOver;

    [SerializeField]
    protected bool goToFirstLevel;

    private bool ready = false;

    //Metodos
    protected virtual IEnumerator Start()
    {
        ready = false;

        //Referencias a elementos principales de la escena
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        data = GameObject.FindGameObjectWithTag("GameData").GetComponent<Data>();
        controlHUD = GameObject.FindGameObjectWithTag("Control/HUD").GetComponent<ControlHUD>();
        loadingScreenController = GameObject.FindGameObjectWithTag("Control/Loading").GetComponent<LoadingScreenController>();

        //Pongo la ventana de carga y bloqueo al jugador
        loadingScreenController.ShowLoadingScreen();

        //Espermos a que todos los elementos de la escena esten listos
        IReadied[] readieds = FindObjectsOfType<MonoBehaviour>().OfType<IReadied>().ToArray();
        foreach (IReadied readied in readieds)
        {
            if (readied is LevelControllerBase) continue;
            yield return new WaitUntil(() => (readied.IsReady()));
        }

        {
        //Obtener referencias a componentes
        levelInitTime = Time.time;

        playerController.GetStats().RestartStats();
        controlHUD.SetHPUI(playerController.GetStats().HP.CurrentValue, playerController.GetStats().HP.maxValue);

        //Inicializacion cursor
        Cursor.lockState = CursorLockMode.Locked;
        }

        //Desactivo la pantalla de carga
        ready = true;
        loadingScreenController.HideLoadingScreen();
        yield return null;
    }
    public bool IsReady()
    {
        return ready;
    }

    protected virtual void Update() {
    }

    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
    }


    //Funciones de estadisticas
    protected virtual void LevelStatistical() {
        float secondsRequired = Time.time - levelInitTime;
        Debug.Log($"Tiempo requerido: {secondsRequired} segundos." );
    }

    //Funciones de gestión de escena
    public virtual IEnumerator LevelWon()
    {
        //data.SetCurrentLevelWin(true);

        yield return new WaitForSecondsRealtime(0.5f);
        //TODO Efecto de de pantalla de victoria

        //TODO Animación PJ victoria


        //Estadisticas del Nivel
        //LevelStatistical();

        //TODO Siguiente Nivel
        data.SetNextLevelByName(nextLevelWin);
        SceneManager.LoadScene(nextLevelWin);
    }

    public virtual IEnumerator LevelGameOver()
    {
        //TODO quitarcolisiones player o dejarlo quieto para evitar que cambie de escena mientras muere

        //data.SetCurrentLevelWin(false);

        //TODO hacer un efecto de camara en el que los bordes se oscurezcan
        yield return new WaitForSeconds(0.5f);
        yield return LevelRestart();
        //SceneManager.LoadScene(nextLevelGameOver);
        //yield return StartCoroutine(LevelRestart());
    }

    protected virtual IEnumerator LevelRestart()
    {
        //Recargo la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null;
    }

    public virtual IEnumerator LevelChange(string nextScene)
    {
        //TODO Animacion de algo

        yield return new WaitForSecondsRealtime(0.5f);
    
        //Recargo la escena actual
        data.SetNextLevelByName(nextScene);
        SceneManager.LoadScene(nextScene);
    }

    public virtual IEnumerator ToMainMenu(string nextScene= "MainMenu")
    {
        yield return new WaitForSecondsRealtime(0.0f);
        SceneManager.LoadScene(nextScene);
    }


}
