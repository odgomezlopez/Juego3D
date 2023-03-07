using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    Data data;
    GameObject ranking, input;
    TMP_InputField nameValue;
    TextMeshProUGUI pointValue, rankingValue;
    Button saveButton;

    void Start()
    {
        //Obtengo los datos actuales
        data = GameObject.FindGameObjectWithTag("GameData").GetComponent<Data>();

        //Obtengo referencias a los elementos de UI
        ranking = gameObject.transform.Find("Ranking").gameObject;
        input = gameObject.transform.Find("Input").gameObject;

        //Referencias a los elementos de Input
        pointValue = input.transform.Find("PointsValue").gameObject.GetComponent<TextMeshProUGUI>();
        nameValue = input.transform.Find("NameInput").gameObject.GetComponent<TMP_InputField>();
        saveButton = input.transform.Find("Save").gameObject.GetComponent<Button>();

        //Referencias a los elementos de Ranking
        rankingValue = ranking.transform.Find("Results").gameObject.GetComponent<TextMeshProUGUI>();

        OnInit();
    }

    private void OnInit()
    {
        if(data.lastPlayerWin == true)
        {
            input.SetActive(true);
            ranking.SetActive(false);

            pointValue.SetText(data.lastPlayerPoint.ToString("D3"));
        }
        else
        {
            GenerateRanking();
            input.SetActive(false);
            ranking.SetActive(true);
        }

    }
    public void InputCheckLength()
    {
        saveButton.interactable = (nameValue.text.Length == 3) && data.CheckName(nameValue.text);
    }

    public void OnSaveButton()
    {
        //Guardamos el nuevo ranking
        int currentPlayerPos=data.AddPlayerRank(nameValue.text.ToUpper(), data.lastPlayerPoint);
        GenerateRanking(currentPlayerPos);


        //Activamos la pantalla del ranking
        input.SetActive(false);
        ranking.SetActive(true);
    }

    private void GenerateRanking(int currentPlayerPos=0)
    {
        //Recuperamos los rankings a mostrar (un total de 7)
        List<(int, Record)> lista = data.GetRankingToShow(currentPlayerPos);

        string cadena = "";
        string sep = "########";
        int lastPos = 0;
        for (int i = 0; i < lista.Count; i++)
        {
            if (lastPos + 1 != lista[i].Item1) cadena+= "\n"+sep;
            lastPos = lista[i].Item1;

            string pos = lista[i].Item1.ToString("D2");
            string name = lista[i].Item2.name.ToUpper();
            string points = lista[i].Item2.points.ToString("D3");

            if (i != 0) cadena += "\n";

            cadena += $"{pos} - {name} - {points} points";
        }

        //if (lastPos != data.RankingCount() + 1) cadena += "\n"+ sep;

        rankingValue.SetText(cadena);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
