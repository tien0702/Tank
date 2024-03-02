using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class EnemyAgent : Agent
{
    List<TankController> tanks;

    TankController tank;

    protected override void Awake()
    {
        base.Awake();
        tank = GetComponent<TankController>();
    }


    private void Start()
    {
        tanks = GameObject.FindObjectsOfType<TankController>().ToList();
        tanks.Remove(GetComponent<EnemyController>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<TankController>(out TankController tank))
        {
            SetReward(1);
            EndEpisode();
        }
        else
        {
            SetReward(-1);
            EndEpisode();
        }
        /*Debug.Log(collision.gameObject.tag);
        
        if (collision.gameObject.tag == "tank")
        {
            SetReward(1);
            EndEpisode();
        }
        if (collision.gameObject.tag == "obstacle")
        {
            SetReward(-1);
            EndEpisode();
        }*/ 
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        /*sensor.AddObservation(tank.model.rotation.eulerAngles);
        sensor.AddObservation(tank.cannonAttachment.rotation.eulerAngles);*/
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector2(UnityEngine.Random.Range(-18f, 18f), UnityEngine.Random.Range(-15f, 10f));
        tank.model.rotation = Quaternion.identity;
        tank.cannonAttachment.rotation = Quaternion.identity;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionX = 2f * Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        var actionY = 2f * Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);

        if (transform.localPosition.x < -18 || transform.localPosition.x > 18 || transform.localPosition.y < -10 || transform.localPosition.y > 10)
        {
            SetReward(-1);
            EndEpisode();
        }
        /*Debug.Log(actionY);
        Debug.Log(actionX);*/
        tank.Move(new Vector3(actionX, actionY, transform.position.z));
    }
}
