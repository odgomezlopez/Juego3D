using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlPauseUI : MonoBehaviour
{
    protected LevelControllerBase control;
    private GameObject pauseMenu;
    private PlayerController playerController;

    private float fixedDeltaTime;

    GameObject HUD;
    private void Awake()
    {
        this.playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Start()
    {
        HUD = GameObject.FindGameObjectWithTag("Control/HUD");
        pauseMenu = gameObject.transform.GetChild(0).gameObject;
        control = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelControllerBase>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(pauseKey))
        {
            OnPauseButton();
        }

        if (Input.GetKeyDown(menuKey) && pauseMenu.activeSelf)
        {
            OnMainMenuButton();
        }*/
    }

    public void OnMainMenuButton()
    {
        ToMainMenu();
    }

    public void OnMainMenuButton(InputAction.CallbackContext context)
    {
        if(pauseMenu.activeSelf && context.performed && context.action.WasPerformedThisFrame() && context.action.IsPressed())
        {
            ToMainMenu();
        }
    }

    private void ToMainMenu()
    {
        //Se reinicia el time Scale
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;

        //Cambio de escenaf
        StartCoroutine(control.ToMainMenu("MainMenu"));
    }

    public void OnPauseButton() {
        TooglePauseMenu();
    }


    public void OnPauseButton(InputAction.CallbackContext context)
    {
        if(context.performed && context.action.WasPerformedThisFrame() && context.action.IsPressed())
        {
            TooglePauseMenu();
        }
    }

    private void TooglePauseMenu()
    {
        if (pauseMenu.activeSelf)//Si el menú de pausa esta activo
        {
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
            HUD.SetActive(true);
            playerController.EnableGamePlay();

            if (control is LevelControllerPoints)
                ((LevelControllerPoints)control).deacreaseTime = 1;

            //TODO desactivar movimiento jugador y gestionar errores si se cambia de nivel
        }
        else// Si el menú de pausa NO esta activo
        {
            Time.timeScale = 0.01f;
            pauseMenu.SetActive(true);
            HUD.SetActive(false);
            playerController.DisableGamePlay();
            //TODO activar movimiento jugador

            if (control is LevelControllerPoints)
                ((LevelControllerPoints)control).deacreaseTime = 0;
        }
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }
}
