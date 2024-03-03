using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTAUnityBase.Base.DesignPattern;
using System;

public interface IHit
{
    void OnHit(float damage);
}

public class BulletController : MonoBehaviour
{
    public TankController owner;
    public Action OnHitTarget = null;
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
            IHit[] iHits = hit.transform.GetComponentsInChildren<IHit>();
            foreach (IHit ihit in iHits)
            {
                if (ihit != null && ihit != owner as IHit)
                {
                    ihit.OnHit(owner.Info.damage);
                    OnHitTarget?.Invoke();
                    this.MoveToPool();
                }
            }
        }
    }

    void MoveToPool()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        OnHitTarget = null;
        PoolingObject.DestroyPooling<BulletController>(this);
    }
}
