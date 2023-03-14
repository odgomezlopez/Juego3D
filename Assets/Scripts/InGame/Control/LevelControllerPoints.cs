using System.Collections;
using UnityEngine;

public class LevelControllerPoints : LevelControllerBase
{
    // Start is called before the first frame update
    public Indicator points;
    public Indicator timeSeconds;
    public float deacreaseTime = 1;

    protected override IEnumerator Start()
    {
        yield return base.Start();
        
        playerController.GetStats().invulnerable = false;
        points.RestartStats();
        timeSeconds.RestartStats();

        StartCoroutine(IncreaseTime());
        //points.maxValue = 3;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    //Funciones de control de Puntos
    public void AddPoints(int p)
    {
        //Sumo
        points.CurrentValue += p;

        if (points.CurrentValue >= points.maxValue) {
            StartCoroutine(LevelWon());
        }
    }

    public IEnumerator IncreaseTime()
    {
        //Sumo
        while(timeSeconds.CurrentValue > 0)
        {
            timeSeconds.CurrentValue -= deacreaseTime;
            yield return new WaitForSecondsRealtime(1f);
        }
        StartCoroutine(LevelWon());
    }

    //Funciones de estadisticas
    protected override void LevelStatistical()
    {
        //Muestro las estadisticas genericas
        base.LevelStatistical();
        //Muestro las estadisticas propias de un nivel por puntos
        Debug.Log($"Puntos obtenidos: {points}");
    }

    //Control UI Puntos


    //Gestion de fin 
    public override IEnumerator LevelWon()
    {
        data.SaveLastRanking((int) points.CurrentValue,true);
        yield return base.LevelWon();
    }

    public override IEnumerator LevelGameOver()
    {
        data.SaveLastRanking((int)points.CurrentValue,false);
        yield return base.LevelGameOver();
    }

}
