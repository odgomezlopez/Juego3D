using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [Header("Action Generic Info")]
    public string actionName = "Activar";

    protected virtual void Start()
    {
    }

    public virtual IEnumerator ExecuteAction()
    {
        yield return new WaitForEndOfFrame();
    }

    public virtual bool CheckRequirement()
    {
        return true;
    }
}