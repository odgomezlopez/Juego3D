using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Indicator
{
    //Valores
    [HideInInspector]
    private float currentValue;
    public float maxValue;
    public float initValue;

    //Gestion recuperación/deterioro automatico
    public float autoUpdateRate = 0;
    public Image icon;

    //Inicialización
    public virtual void RestartStats()
    {
        CurrentValue = initValue;
        autoUpdateRate = 0;
    }

    //Operaciones
    public float GetPercentage()
    {
        return CurrentValue / maxValue;
    }

    //Get y Set publico
    public float CurrentValue
    {
        get => currentValue;
        set
        {
            currentValue = Mathf.Clamp(value, 0.0f, maxValue);
            if (OnIndicatorChange != null)
                OnIndicatorChange(value);
        }
    }

    //Eventos para detectar cambios en el indicador
    public delegate void OnIndicatorChangeDelegate(float newValue);
    public event OnIndicatorChangeDelegate OnIndicatorChange;
}
