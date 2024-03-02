using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTAUnityBase.Base.DesignPattern;
public interface IHit
{
    void OnHit(float damage);
}

public class BulletController : MonoBehaviour
{
    public TankController owner;
    public GameObject explosionPrefab;
    public string explosionName;
    public float lifeTime = 0;

    float elapsedTime = 0;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= lifeTime)
        {
            elapsedTime = 0;
            this.MoveToPool();
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 0.2f);
        if (hit.transform != null)
        {
            IHit iHit = hit.transform.parent.GetComponent<IHit>();
            if (iHit != null)
            {
                iHit.OnHit(owner.Info.damage);
                this.MoveToPool();
                return;
            }

        }
        //Move(transform.up);
    }

    void MoveToPool()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        PoolingObject.DestroyPooling<BulletController>(this);
    }
}