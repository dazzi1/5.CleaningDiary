﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 能射击的军人僵尸
/// </summary>
public class EnemySoldier : Enemy
{
    //资源
    public GameObject bulletGo;
    public AudioClip shootClip;

    //引用
    public Transform shootPoint;
    protected AudioSource audioSource;

    //属性
    public float attackCD;
    protected float curCD;
    public int magazine;
    protected int curMagazine;
    public float reload;
    protected float curReload;
    public float inaccuracy;

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }
    protected override void Update()
    {
        CalculateCD();
        base.Update();
    }
    /// <summary>
    /// 计算冷却
    /// </summary>
    protected void CalculateCD() {
        curCD -= Time.deltaTime;
        curReload -= Time.deltaTime;
        if (curReload <= 0 && curMagazine <= 0) {
            curMagazine = magazine;
        }
    }
    /// <summary>
    /// 攻击方法
    /// </summary>
    protected override void LookAtPlayerAndAttack()
    {
        base.LookAtPlayerAndAttack();
        if (curCD<=0&&curMagazine>0)
        {
            audioSource.PlayOneShot(shootClip,GameManager.Instance.volume);
            Instantiate(bulletGo, shootPoint.position, Quaternion.Euler(0,0, transform.rotation.eulerAngles.z
                + Random.Range(-inaccuracy, inaccuracy)));
            curCD = attackCD;
            curMagazine -= 1;
            if (curMagazine<=0)
            {
                curReload = reload;
            }
        }
    }
}
