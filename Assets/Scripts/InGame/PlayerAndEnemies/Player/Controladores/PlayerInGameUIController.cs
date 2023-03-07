using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInGameUIController : MonoBehaviour
{
    private GameObject playerInGameUI;
    private TextMeshProUGUI textUI;

    // Start is called before the first frame update
    void Start()
    {
        playerInGameUI = gameObject.transform.Find("PlayerInGameUI").gameObject;
        textUI = playerInGameUI.transform.GetChild(0).GetChild(0).gameObject.GetComponentInChildren<TextMeshProUGUI>();
        playerInGameUI.SetActive(false);
    }

    // Update is called once per frame
    public void EnableUI(string text,bool activable)
    {
        textUI.SetText(text);
        playerInGameUI.SetActive(true);

        if (activable)
        {
            textUI.color = Color.black;
        }
        else
        {
            textUI.color = Color.gray;
        }
    }

    public void DisableUI()
    {
        playerInGameUI.SetActive(false);
    }
}
