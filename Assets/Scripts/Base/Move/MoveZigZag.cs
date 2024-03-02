using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveZigZag : MonoBehaviour , ITypeMove
{
    Vector3 direction = Vector3.zero;

    int rightDirection = -1;

    public Vector3 Move()
    {
        if (direction.magnitude >= 5f)
        {
            rightDirection = -rightDirection;
        }
        direction += rightDirection * transform.right;
        return direction;
    }

    //private void Update()
    //{
        
    //}
}
