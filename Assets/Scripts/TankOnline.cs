using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class TankOnline : TankController
{
    protected override void Awake()
    {
        base.Awake();

        hpController = GetComponentInChildren<HPController>();
        levelController = GetComponentInChildren<LevelController>();

        hpController.onDie.Add(OnDie);
        levelController.onLevelUp = OnLevelUp;
        levelController.Level = 1;
        levelController.maxValue = 60;
        levelController.CurrentValue = 0;
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
    }

    protected override TankInfo GetTankInfo(int level)
    {
        return DataManager.Instance.enemyVO.GetTankInfo(level);
    }
}
