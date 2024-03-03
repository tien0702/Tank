using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : TankController
{

    protected override void Awake()
    {
        base.Awake();

        var hpCtrl = Resources.Load<GameObject>("Prefabs/HPRed").GetComponentInChildren<HPController>();
        var lvCtrl = Resources.Load<GameObject>("Prefabs/LvController").GetComponentInChildren<LevelController>();
        hpController = Instantiate<HPController>(hpCtrl, transform);
        levelController = Instantiate<LevelController>(lvCtrl, transform);
        hpController.onDie.Add(OnDie);
        levelController.onLevelUp = OnLevelUp;
        levelController.Level = 1;
        levelController.maxValue = 60;
        levelController.CurrentValue = 0;
    }

    private void Start()
    {
    }

    protected override void OnLevelUp(int level)
    {
        base.OnLevelUp(level);
        if (model == null || model.name != Info.modelName)
        {
            if (model != null) Destroy(model.gameObject);
            var modelPrefab = Resources.Load<Transform>("Prefabs/Tanks/" + Info.modelName);
            model = Instantiate<Transform>(modelPrefab, transform);
            this.cannonAttachment = model.GetChild(0);
        }

        foreach (Transform trans in cannonAttachment)
        {
            Destroy(trans.gameObject);
        }
        var cannonPrefab = Resources.Load<Transform>("Prefabs/Cannons/" + Info.cannonName);
        Transform cannon = Instantiate<Transform>(cannonPrefab, cannonAttachment);
        Transform shootPoint = cannon.Find("ShootPoints");
        this.shootPoints = shootPoint.GetComponentsInChildren<Transform>().ToList();
        shootPoints.Remove(shootPoint);

        this.bulletPrefab = Resources.Load<BulletController>("Prefabs/Bullets/" + Info.bulletName);
    }

    protected override void OnDie()
    {
        var explosionFxPrefab = Resources.Load<Animator>("Prefabs/FXs/ExplosionFX");
        var fx = Instantiate(explosionFxPrefab, transform.position, transform.rotation);
        fx.Play("explosion-2");
        //Destroy(gameObject);
    }

    protected override TankInfo GetTankInfo(int level)
    {
        return DataManager.Instance.enemyVO.GetTankInfo(level);
    }
}
