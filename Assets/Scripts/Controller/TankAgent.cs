using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class TankAgent : Agent, IHit
{
    public GameObject rayPrefab;
    List<Transform> targets;
    List<Transform> obstacles;

    TankController tank;

    bool canShoot = true;

    protected override void Awake()
    {
        base.Awake();
        tank = GetComponent<TankController>();

        rayPrefab = Resources.Load<GameObject>("Prefabs/Ray");
    }

    private void Start()
    {
        targets = transform.parent.GetComponentsInChildren<TankController>().ToList().Select(x => x.GetComponent<Transform>()).ToList();
        targets.Remove(gameObject.transform);
        var obs = GameObject.FindGameObjectsWithTag("obstacle");
        obstacles = new List<Transform>(obs.Length);
        foreach (var g in obs) obstacles.Add(g.transform);

        tank.hpController.onDie.Add(OnDie);

        BehaviorParameters behaviors = GetComponent<BehaviorParameters>();

        GameObject.Instantiate(rayPrefab, tank.model);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("wall")) ||
            collision.gameObject.layer.Equals(LayerMask.NameToLayer("obstacle")) ||
            collision.gameObject.layer.Equals(LayerMask.NameToLayer("tank")))
        {
            Debug.Log(collision.gameObject.layer);
            AddReward(-1);
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
        if (target != null)
        {
            sensor.AddObservation(target.transform.position);
            sensor.AddObservation(target.GetComponent<TankController>().hpController.CurrentValue);
        }
        else
        {
            sensor.AddObservation(Vector3.zero);
            sensor.AddObservation(-1f);
        }

        float nearestTargetDistance = target != null ? Vector3.Distance(transform.position, target.position) : 0f;
        sensor.AddObservation(nearestTargetDistance);
        sensor.AddObservation(tank.hpController.CurrentValue);
        sensor.AddObservation(transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log(actions.ContinuousActions[0]);
        var moveDirX = actions.ContinuousActions[0];
        var moveDirY = actions.ContinuousActions[1];
        tank.Move(new Vector3(moveDirX, moveDirY, transform.position.z));

        Transform nearestTarget = GetNearestTarget();
        if (nearestTarget != null && Vector3.Distance(transform.position, nearestTarget.position) <= tank.Info.attackRange)
        {
            var rotateY = actions.ContinuousActions[2];
            var rotateX = actions.ContinuousActions[3];
            Vector3 cannonDir = new Vector3(rotateX, rotateY);

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
        AddReward(-1);
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
            if (target == null) continue;
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
