using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICheckTarget
{
    public bool CheckTarget(Transform target);
}

public class FilterTargetController : MonoBehaviour
{

    ICheckTarget[] checkTargets;
    // Start is called before the first frame update
    void Start()
    {
        checkTargets = GetComponents<ICheckTarget>();
    }

    public bool CheckTarget(Transform target)
    {
        if (checkTargets == null || checkTargets.Length == 0) return true;
        foreach(ICheckTarget checkTarget in checkTargets)
        {
            if (!checkTarget.CheckTarget(target)) return false;
        }
        return true;
    }
}
