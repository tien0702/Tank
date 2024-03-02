using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TankInfo
{
    public string bulletName;
    public string modelName;
    public string cannonName;
    public int damage;
    public int hp;
    public int aspd;
    public float speed;
    public float attackRange;
}

public abstract class TankController : MoveController, IHit
{
    public TankInfo Info { protected set; get; }

    public Transform cannonAttachment;

    public Transform model;

    public List<Transform> shootPoints;

    public HPController hpController;

    public LevelController levelController;
    public GameObject shootFx;
    public BulletController bulletPrefab;

    protected virtual void Awake()
    {
        shootFx = Resources.Load<GameObject>("Prefabs/FXs/ShootFX");
    }

    public override void Move(Vector3 direction)
    {
        model.up = direction;
        base.Move(direction);
    }

    public virtual void RotateGun(Vector3 direction)
    {
        cannonAttachment.up = direction;
    }

    public void Shoot()
    {
        foreach(Transform shootPoint in shootPoints)
        {
            // fx
            var fx = Instantiate(shootFx, shootPoint);
            fx.transform.rotation = shootPoint.rotation;

            // bullet
            var bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.owner = this;
        }
    }

    public void OnHit(float damage)
    {
        hpController.TakeDamage(damage);
    }

    protected abstract void OnDie();

    protected virtual void OnLevelUp(int level)
    {
        Info = GetTankInfo(level);
        hpController.HP = Info.hp;
        this.speed = Info.speed;
        //Sound.Instance.PlaySound("power_up_sound");
    }

    protected abstract TankInfo GetTankInfo(int level);

}
