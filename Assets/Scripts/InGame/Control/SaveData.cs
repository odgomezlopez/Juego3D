using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //Se pone aquí toda la información a guardar
    public List<Record> ranking;

    //Se inicializan las variables
    public SaveData()
    {
        ranking = new List<Record>();
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string a_Json)
    {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }
}