using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelControllerBase : MonoBehaviour
{
    //Atributos
    protected Data data;
    protected PlayerStats stats;
    protected ControlHUD controlHUD;

    protected float levelInitTime;


    [SerializeField]
    protected string nextLevelWin;

    [SerializeField]
    protected string nextLevelGameOver;

    [SerializeField]
    protected bool goToFirstLevel;

    //Metodos
    protected virtual void Start()
    {
        data = GameObject.FindGameObjectWithTag("GameData").GetComponent<Data>();

        stats = GameObject.FindGameObjectWithTag("GameData").GetComponent<PlayerStats>();
        stats.RestartStats();

        controlHUD= GameObject.FindGameObjectWithTag("Control/HUD").GetComponent<ControlHUD>();
        levelInitTime = Time.time;

        controlHUD.SetHPUI(stats.HP.CurrentValue,stats.HP.maxValue);

        //Inicializacion cursor
        Cursor.lockState = CursorLockMode.Locked;
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
