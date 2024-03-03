using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTAUnityBase.Base.DesignPattern;
using System.Linq;
using Unity.Barracuda;
using System;

public class PlayerController : TankController
{
    FilterTargetController filterTarget;

    protected override void Awake()
    {
        base.Awake();

        var hpCtrl = Resources.Load<GameObject>("Prefabs/HPGreen").GetComponentInChildren<HPController>();
        var lvCtrl = Resources.Load<GameObject>("Prefabs/LvController").GetComponentInChildren<LevelController>();
        hpController = Instantiate<HPController>(hpCtrl, transform);
        levelController = Instantiate<LevelController>(lvCtrl, transform);
        hpController.onDie.Add(OnDie);
        levelController.onLevelUp = OnLevelUp;
        filterTarget = GetComponent<FilterTargetController>();
    }

    private void Start()
    {
        levelController.Level = 1;
        levelController.maxValue = 60;
        levelController.CurrentValue = 0;
    }

    protected override void OnDie()
    {
        Debug.Log("OnDie");
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
            levelController.Level += 1;

        Vector3 direction = new Vector3(horizontal, vertical);
        if(!direction.Equals(Vector2.zero)) Move(direction);

        Vector3 gunDirection = Vector3.zero;
        gunDirection = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

        gunDirection.z = transform.position.z;
        RotateGun(gunDirection);
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    protected override void OnLevelUp(int level)
    {
        base.OnLevelUp(level);
        if(model == null || model.name != Info.modelName)
        {
            if(model != null) Destroy(model.gameObject);
            var modelPrefab = Resources.Load<Transform>("Prefabs/Tanks/" +  Info.modelName);
            model = Instantiate<Transform>(modelPrefab, transform);
            this.cannonAttachment = model.GetChild(0);
        }

        foreach(Transform trans in cannonAttachment)
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

    protected override TankInfo GetTankInfo(int level)
    {
        return DataManager.Instance.playerVO.GetTankInfo(level);
    }
}

public class Player : SingletonMonoBehaviour<PlayerController>
{

}
