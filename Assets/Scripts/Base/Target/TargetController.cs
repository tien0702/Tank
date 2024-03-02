using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    static List<TargetController> targets = new List<TargetController>();


    public static Transform GetTarget(FilterTargetController filter)
    {
        Transform bestTarget = null;

        foreach(TargetController target in targets)
        {
            if (!filter.CheckTarget(target.transform)) continue;
            if (bestTarget == null) {
                bestTarget = target.transform;
                continue;
            }
            float distanceCurrentTarget = Vector3.Distance(filter.transform.position, bestTarget.position);
            float distanceTarget = Vector3.Distance(filter.transform.position, target.transform.position);
            if (distanceCurrentTarget > distanceTarget)
            {
                bestTarget = target.transform;
            }
        }
        return bestTarget;
    }

    private void Awake()
    {
        if (!targets.Contains(this))
            targets.Add(this);
    }

    private void OnDestroy()
    {
        if (targets.Contains(this))
            targets.Remove(this);
    }
}
