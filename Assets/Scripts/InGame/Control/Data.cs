using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Data : MonoBehaviour, ISaveable
{
    [SerializeField]
    private string  currentLevelName;
    public GameObject currentAvatar;

    [SerializeField]
    public int lastPlayerPoint = 0;
    [SerializeField]
    public bool lastPlayerWin = false;



    public List<Record> ranking;


    //Utilizada para guardar/cargar
    List<ISaveable> elementosCargar;

    private void Awake()
    {
        //Comrpuebo si es necesario borrar
        int numInstances = FindObjectsOfType<Data>().Length;
        if (numInstances != 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);


            //Defino la información a cargar/Guardar
            elementosCargar = new List<ISaveable>();
            elementosCargar.Add(this);

            //Cargo
            SaveDataManager.LoadJsonData(elementosCargar);

            if (ranking == null) ranking = new List<Record>();
        }
    }

    void Start()
    {
        //Inicializo los datos
        SetNextLevelByName("Level1");
    }

    //Utilidades de cargar/guardar partida

    private void OnApplicationQuit()
    {
        Save();
    }

    private void Save()
    {
        SaveDataManager.SaveJsonData(elementosCargar);
    }

    public void PopulateSaveData(SaveData a_SaveData)
    {
        a_SaveData.ranking = ranking;
    }

    public void LoadFromSaveData(SaveData a_SaveData)
    {
        ranking = a_SaveData.ranking;
    }

    //Almacenamiento del siguiente nivel

    public void SetNextLevelByName(string name)
    {
        currentLevelName = name;
        //Si es la primera vez que entro al nivel, inicializo sus valores
        //if (!levelsWins.ContainsKey(currentLevelName)) levelsWins[currentLevelName] = false;
        //if (!levelsPoints.ContainsKey(currentLevelName)) levelsPoints[currentLevelName] = 0;

    }

    public string GetCurrentLevelName()
    {
        return currentLevelName;
    }

    public bool IsLastLevel()//TODO
    {
        return GetCurrentLevelName()=="Level1";
    }

    //Gestión de puntos superados y punos


    public void SaveLastRanking(int newPoints,bool win)
    {
        lastPlayerPoint = newPoints;
        lastPlayerWin = win;
        //TODO
    }

    //trabajo con el ranking

    public bool CheckName(string newName)
    {
        for(int i = 0; i < ranking.Count; i++)
        {
            if (ranking[i].name == newName) return false;
        }
        return true;
    }
    public int AddPlayerRank(string name, int points)
    {
        Record r = new Record(name, points);
        ranking.Add(r);
        ranking.Sort(new SurnameComparer());

        //Vacio la info del ultimo player
        lastPlayerWin = false;
        lastPlayerPoint = 0;

        //Devuelvo la posición inciial
        return ranking.IndexOf(r);
    }

    public int RankingCount()
    {
        return ranking.Count;
    }

    public List<(int,Record)> GetRankingToShow(int currentPlayerPos, int maxShowable = 6)
    {
        ranking.Sort(new SurnameComparer());


        List<(int, Record)> lista = new List<(int, Record)>();

        List<int> posicionesMostrar = new List<int>();

        //calculo las posiciones de la lista a mostrar
        if (ranking.Count < maxShowable || currentPlayerPos < maxShowable)
        {
            for(int i=0; (i< ranking.Count) && i < maxShowable; i++)
            {
                posicionesMostrar.Add(i);
            }
        }
        else
        {
            posicionesMostrar.Add(0);
            posicionesMostrar.Add(1);
            posicionesMostrar.Add(2);

            if (currentPlayerPos == ranking.Count - 1) posicionesMostrar.Add(currentPlayerPos - 2);
            posicionesMostrar.Add(currentPlayerPos - 1);
            posicionesMostrar.Add(currentPlayerPos);
            if (currentPlayerPos != ranking.Count - 1) posicionesMostrar.Add(currentPlayerPos +1);
        }
        //añado los elementos
        foreach(int i in posicionesMostrar)
        {
            lista.Add((i + 1, ranking[i]));
        }

        return lista;
    }


    //Metodos de ordenación
    //ranking.Sort(new SurnameComparer());
    //ranking.ForEach(employee => Console.WriteLine(employee));



}
