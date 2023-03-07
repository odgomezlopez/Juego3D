using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelStadisticalUIController : MonoBehaviour
{
    Data data;
    public TextMeshProUGUI resultado;
    public Button botonSiguiente;

    [SerializeField]
    bool restartOnGameOver = true;

    void Start()
    {
        data = GameObject.FindGameObjectWithTag("GameData").GetComponent<Data>();
        string texto = "";
        /*if (data.IsLastLevel() && data.GetCurrentLevelWin())
        {
            texto = $"Enhorabuena has terminado el juego!!!!";
            //botonSiguiente.interactable = false;
            botonSiguiente.gameObject.SetActive(false);

        }
        else if (data.GetCurrentLevelWin())
        {
            texto = $"Has acumulado {data.GetCurrentPoints()} puntos!";
            //data.SetNextLevelByName("TODO");
        }
        else {
            texto = "No has superado el nivel...";
            botonSiguiente.interactable = false;
            //Si muero vuelvo al nivel1
            if (restartOnGameOver) data.SetNextLevelByName("Level1");
        }*/

        resultado.SetText(texto);
    }

}
