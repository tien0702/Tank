using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetDistance : MonoBehaviour, ICheckTarget
{
    public bool CheckTarget(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= 5f) return true;
        return false;
    }
}
