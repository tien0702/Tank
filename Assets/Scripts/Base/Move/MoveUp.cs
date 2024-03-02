using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour,ITypeMove
{
    public Vector3 Move()
    {
        return transform.up;
    }

}
