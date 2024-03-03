using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    Transform target;
    private void Awake()
    {
        PlayerController player = GameObject.FindAnyObjectByType<PlayerController>();
        target = player.transform;
    }

    private void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
