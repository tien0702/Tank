using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class PlayerOnlineController : TankController
{
    FilterTargetController filterTarget;
    PhotonView pv;

    protected override void Awake()
    {
        pv = GetComponent<PhotonView>();
        base.Awake();

        /*var hpCtrl = Resources.Load<GameObject>("Prefabs/HPGreen").GetComponentInChildren<HPController>();
        var lvCtrl = Resources.Load<GameObject>("Prefabs/LvController").GetComponentInChildren<LevelController>();
        var namePrf = Resources.Load<GameObject>("Prefabs/TankName").GetComponentInChildren<TextMesh>();*/
        
        hpController = GetComponentInChildren<HPController>();
        levelController = GetComponentInChildren<LevelController>();
        TextMesh pName = GetComponentInChildren<TextMesh>();
        if(pv.IsMine)
        {
            pName.text = PhotonNetwork.LocalPlayer.NickName;
            GameObject.FindAnyObjectByType<FollowTarget>().target = transform;
        }
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
        Destroy(gameObject);
    }

    void Update()
    {
        if (!pv.IsMine) return;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
            levelController.Level += 1;

        Vector3 direction = new Vector3(horizontal, vertical);
        if (!direction.Equals(Vector2.zero)) Move(direction);

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

    [PunRPC]
    public override void Shoot()
    {
        base.Shoot();
    }


    protected override TankInfo GetTankInfo(int level)
    {
        return DataManager.Instance.playerVO.GetTankInfo(level);
    }
}
