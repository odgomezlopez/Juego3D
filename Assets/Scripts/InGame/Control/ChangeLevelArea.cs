using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevelArea : MonoBehaviour
{
    [SerializeField]
    public bool winningDoor;

    [SerializeField]
    protected string nextLevel;
    protected LevelControllerBase control;
    // Start is called before the first frame update
    void Start()
    {
        control = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelControllerBase>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player")) { 
            if(winningDoor)
                StartCoroutine(control.LevelWon());
            else
                StartCoroutine(control.LevelChange(nextLevel));
        }
    }

}
