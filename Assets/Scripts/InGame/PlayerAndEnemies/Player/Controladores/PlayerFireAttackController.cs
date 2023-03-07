using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireAttackController : MonoBehaviour
{
    //Variables propias del controlador de ataque
    GameObject attackSpawPoint1;
    private bool canShoot = true;

    //
    public PlayerStats stats;
    private PlayerStateController stateController;
    private Rigidbody fisicas;


    // Start is called before the first frame update
    void Start()
    {
        stats = GameObject.FindGameObjectWithTag("GameData").GetComponent<PlayerStats>();
        stateController = GetComponent<PlayerStateController>();
        fisicas = GetComponent<Rigidbody>();

        //Spawn Points
        attackSpawPoint1 = gameObject.transform.Find("AttackSpawnPoint").gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Attack()
    {
        if (canShoot)
        {
            StartCoroutine(AttackCoroutine());
            return true;
        }
        return false;
    }

    private IEnumerator AttackCoroutine() {
        //Variables de control de estado
        canShoot = false;
        stateController.UpdateState(PlayerStates.ATTACK);

        //Calculo la dirección del ataque


        //Genero el ataque 1
        GameObject newAttack = Instantiate(stats.attack, attackSpawPoint1.transform.position,gameObject.transform.rotation);
        //newAttack.transform.SetParent(null);
        newAttack.GetComponent<FireBallAttack>().attackOrigin = gameObject;
        newAttack.GetComponent<Rigidbody>().velocity = fisicas.velocity;
        StartCoroutine(newAttack.GetComponent<FireBallAttack>().ExecuteAction());

        stateController.UpdateState(stateController.previousState);
        yield return new WaitForSecondsRealtime(stats.attackTimeBetween);

        //Vuelvo a habilitar el ataque
        canShoot = true;
        yield return null;
    }
}
