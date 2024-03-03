using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class EnemyAgent : Agent, IHit
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

        tank.hpController.onDie.Add(OnDie);

        BehaviorParameters behaviors = GetComponent<BehaviorParameters>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("wall")) ||
            collision.gameObject.layer.Equals(LayerMask.NameToLayer("obstacle")) ||
            collision.gameObject.layer.Equals(LayerMask.NameToLayer("tank")))
        {
            Debug.Log(collision.gameObject.layer);
            SetReward(-1);
            EndEpisode();
        }
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector2(UnityEngine.Random.Range(-18f, 18f), UnityEngine.Random.Range(-15f, 10f));
        tank.hpController.CurrentValue = tank.hpController.maxValue;
        tank.model.rotation = Quaternion.identity;
        tank.cannonAttachment.rotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Transform target = GetNearestTarget();
        if (target != null) sensor.AddObservation(target.transform.position);
        else sensor.AddObservation(Vector3.zero);

        float nearestTargetDistance = target != null ? Vector3.Distance(transform.position, target.position) : 0f;
        sensor.AddObservation(nearestTargetDistance);
        sensor.AddObservation(tank.hpController.CurrentValue);
        sensor.AddObservation(transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var moveDirX = actions.ContinuousActions[0];
        var moveDirY = actions.ContinuousActions[1];
        tank.Move(new Vector3(moveDirX, moveDirY, transform.position.z));

        Transform nearestTarget = GetNearestTarget();
        if (nearestTarget != null && Vector3.Distance(transform.position, nearestTarget.position) <= tank.Info.attackRange)
        {
            Vector3 cannonDir = transform.position - nearestTarget.position;
            tank.RotateGun(cannonDir);

            if (canShoot)
            {
                canShoot = false;
                Shoot();
                LeanTween.delayedCall(1000f / tank.Info.aspd, () => { canShoot = true; });
            }
        }
    }

    void OnDie()
    {
        Debug.Log("ondie");
        SetReward(-1);
        EndEpisode();
    }

    public void Shoot()
    {
        Debug.Log("shoot");
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

    public void OnHit(float damage)
    {
        Debug.Log("hit");
        AddReward(-0.1f);
    }
}
