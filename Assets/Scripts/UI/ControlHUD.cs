using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class ControlHUD : MonoBehaviour
{
    //Stats
    protected PlayerStats stats;

    //Gestión de la vida dle jugador
    public GameObject HPBar;

    //Gestion de otros elementos de la UI
    public TextMeshProUGUI timeValue;
    public TextMeshProUGUI pointValue;

    //Gestion HUD
    protected Coroutine lastRoutine = null;
    protected LevelControllerBase control;
    private InputAction m_ShowHUDAction;


    public bool alwayShow = true;

    private void Start()
    {
        control = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelControllerBase>();
        m_ShowHUDAction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>().actions["ShowHUD"];

        //Obtengo los stats y me suscribo a los cambios de HP
        stats = GameObject.FindGameObjectWithTag("GameData").GetComponent<PlayerStats>();
        stats.HP.OnIndicatorChange += OnHPUIUpdate;

        if (control is LevelControllerPoints)
        {
            ((LevelControllerPoints)control).points.OnIndicatorChange += OnPointUIUpdate;
            ((LevelControllerPoints)control).timeSeconds.OnIndicatorChange += OnTimeUIUpdate;

        }
        //Activo la UI
        if (alwayShow) ShowHUD();
        else lastRoutine = StartCoroutine(ShowHideUI());
    }

    //Gestion de eventos de cambio de HP


    protected virtual void OnHPUIUpdate(float val)
    {
        //Actualizo la UI
        SetHPUI(stats.HP.CurrentValue, stats.HP.maxValue);
        if (!m_ShowHUDAction.IsPressed() && !alwayShow)
        {
            StopCoroutine(lastRoutine);
            lastRoutine = StartCoroutine(ShowHideUI());
        }
    }

    protected virtual void OnTimeUIUpdate(float val)
    {
        //Actualizo la UI
        SetTimeUI((int) val /60, (int) val % 60);
        /*if (!m_ShowHUDAction.IsPressed() && !alwayShow)
        {
            StopCoroutine(lastRoutine);
            lastRoutine = StartCoroutine(ShowHideUI());
        }*/
    }

    protected virtual void OnPointUIUpdate(float val)
    {
        //Actualizo la UI
        if (control is LevelControllerPoints)
        { 
            SetPointsUI(((int)((LevelControllerPoints)control).points.CurrentValue));

            if (!m_ShowHUDAction.IsPressed() && !alwayShow)
            {
                StopCoroutine(lastRoutine);
                lastRoutine = StartCoroutine(ShowHideUI());
            }
        }
    }

    private void OnDestroy()
    {
        stats.HP.OnIndicatorChange -= OnHPUIUpdate;//HAY QUE DESUSCRIBIRSE AL SALIR DEL NIVEL

        if (control is LevelControllerPoints) { 
            ((LevelControllerPoints)control).points.OnIndicatorChange -= OnPointUIUpdate;
            ((LevelControllerPoints)control).timeSeconds.OnIndicatorChange -= OnTimeUIUpdate;
        }
    }

    //Gestión de eventos de teclado

    public void OnShowHUDButton(InputAction.CallbackContext context) {
        if (context.performed && context.action.WasPerformedThisFrame() && context.action.IsPressed())
        {
            ShowHUD();
        }
        if (context.canceled)
        {
            StopCoroutine(lastRoutine);
            lastRoutine = StartCoroutine(ShowHideUI());
        }
    }

    protected virtual IEnumerator ShowHideUI(int seg = 2)
    {
        if (!alwayShow) { 
            ShowHUD();
            yield return new WaitForSecondsRealtime(seg);
            HideHUD(); //TODO quitar el requerimiento de mirar aqui la Q
        }
    }

    //Funciones publicas
    public virtual void ShowHUD()
    {
        EnableHPUI();

        if(control is LevelControllerPoints)
        {
            EnablePointsUI();
            EnableTimeUI();
        }
    }

    protected virtual void HideHUD()
    {
        if (alwayShow) return;
        

        DisableHPUI();

        if (control is LevelControllerPoints)
        {
            DisablePointsUI();
            DisableTimeUI();
        }
    }






    //Gestion de Vidas
    public void EnableHPUI() {
         HPBar.gameObject.SetActive(true);
    }
    public void DisableHPUI()
    {
        HPBar.gameObject.SetActive(false);
    }

    public void SetHPUI(float currentHP, float maxHP)
    {
        float maxWidth = HPBar.GetComponent<RectTransform>().rect.width - 0;//El valor de 0 se cambiaría si se quieren dejar bordes a los lados de la barra
        float currentWidth = (maxWidth * currentHP) / maxHP; 

        RectTransform bar = HPBar.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        bar.sizeDelta=new Vector2(currentWidth, bar.rect.height);
    }

    //Gestion de Tiempo
    public void EnableTimeUI()
    {
        timeValue.gameObject.SetActive(true);
    }
    public void DisableTimeUI()
    {
        timeValue.gameObject.SetActive(false);
    }
    public void SetTimeUI(int tiempoMin,int tiempoSeg)
    {
        timeValue.SetText(tiempoMin.ToString("D2") +":"+ tiempoSeg.ToString("D2"));
    }

    //Gestion de Puntos
    public void EnablePointsUI()
    {
        pointValue.gameObject.SetActive(true);
    }
    public void DisablePointsUI()
    {
        pointValue.gameObject.SetActive(false);
    }
    public void SetPointsUI(int points)
    {
        pointValue.SetText(points.ToString("D3"));
    }

}
