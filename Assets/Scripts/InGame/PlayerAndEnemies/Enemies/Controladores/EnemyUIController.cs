using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyUIController : MonoBehaviour
{
    protected Stats stats;
    public Image cooldown;


    void Start()
    {
        cooldown = gameObject.transform.Find("Life").GetChild(0).GetChild(0).gameObject.GetComponent<Image>();

        stats =GetComponent<Stats>();
        stats.HP.OnIndicatorChange += OnUpdateEnemyHP;

    }

    void OnUpdateEnemyHP(float HP)
    {
        cooldown.fillAmount = stats.HP.GetPercentage();
    }

    private void OnDestroy()
    {
        stats.HP.OnIndicatorChange -= OnUpdateEnemyHP;
    }
}
