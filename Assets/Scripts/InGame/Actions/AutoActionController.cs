using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoActionController : ActionController
{
    // Start is called before the first frame update
    //public bool loop = false;

    protected override void Start()
    {
        StartCoroutine(ExecuteAction());
    }

    public override IEnumerator ExecuteAction()
    {
        yield return new WaitForEndOfFrame();
        //if (loop)
        //yield return ExecuteAction();
    }

    public override bool CheckRequirement()
    {
        return true;
    }
}
