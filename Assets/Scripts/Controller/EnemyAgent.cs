using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class EnemyAgent : Agent
{
    List<Transform> targets;
    List<Transform> obstacles;

    TankController tank;

    bool canShoot = true;

    protected override void Awake()
    {
        base.Awake();
        tank = GetComponent<TankController>();
    }

    private void Start()
    {
        targets = GameObject.FindObjectsOfType<TankController>().ToList().Select(x => x.GetComponent<Transform>()).ToList();
        targets.Remove(gameObject.transform);
        obstacles = transform.parent.Find("Obstacles").GetComponentsInChildren<Transform>().ToList();
        obstacles.Remove(transform.parent.Find("Obstacles"));

        BehaviorParameters behaviors = GetComponent<BehaviorParameters>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetReward(-1);
        EndEpisode();
    }


    public void Shoot()
    {
        foreach (Transform shootPoint in tank.shootPoints)
        {
            // fx
            var fx = Instantiate(tank.shootFx, shootPoint);
            fx.transform.rotation = shootPoint.rotation;

            // bullet
            var bullet = Instantiate(tank.bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.owner = tank;
            bullet.OnHitTarget = OnHitTarget;
        }
    }

    void OnHitTarget()
    {
        AddReward(0.1f);
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector2(UnityEngine.Random.Range(-18f, 18f), UnityEngine.Random.Range(-15f, 10f));
        tank.model.rotation = Quaternion.identity;
        tank.cannonAttachment.rotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float nearestTargetDistance = GetNearestTargetDistance();
        sensor.AddObservation(nearestTargetDistance);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var moveDirX = 2f * Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        var moveDirY = 2f * Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);
        tank.Move(new Vector3(moveDirX, moveDirY, transform.position.z));

        if (transform.localPosition.x < -18 || transform.localPosition.x > 18 || transform.localPosition.y < -16 || transform.localPosition.y > 11)
        {
            SetReward(-1);
            EndEpisode();
        }

        Transform nearestTarget = GetNearestTarget();
        if (nearestTarget != null && Vector3.Distance(transform.position, nearestTarget.position) <= tank.Info.attackRange)
        {
            var rotateX = 2f * Mathf.Clamp(actions.ContinuousActions[2], -1f, 1f);
            var roateY = 2f * Mathf.Clamp(actions.ContinuousActions[3], -1f, 1f);

            tank.RotateGun(new Vector3(rotateX, roateY));

            if (canShoot)
            {
                canShoot = false;
                Shoot();
                LeanTween.delayedCall(1000f / tank.Info.aspd, () => { canShoot = true; });
            }
        }

    }

    private float GetNearestTargetDistance()
    {
        Transform nearestTarget = GetNearestTarget();
        return nearestTarget != null ? Vector3.Distance(transform.position, nearestTarget.position) : 0f;
    }

    private Transform GetNearestTarget()
    {
        Transform nearestTarget = null;
        float minDistance = float.MaxValue;

        foreach (Transform target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTarget = target;
            }
        }

        return nearestTarget;
    }
}
