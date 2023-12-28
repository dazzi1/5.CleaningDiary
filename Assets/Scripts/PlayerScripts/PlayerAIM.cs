using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 安德鲁的AI管理者，负责搜索敌人以及瞄准敌人
/// </summary>
public class PlayerAIM : MonoBehaviour
{
    public Transform cameraTrans;//摄像机
    public GameObject mark;//标记敌人的标记
    public float minDistance;//最近距离
    public float maxDistance;//视野范围
    private Player player;//玩家引用
    private Enemy nearestEnemy;//最近敌人
    private RaycastHit2D hit;//敌人发出的射线 进行检测
    //private int layerValue;//2进制层级
    private LayerMask layerMask;
    private float distance;//怪物和玩家间的距离

    void Start()
    {
        cameraTrans = Camera.main.transform;
        player = GameManager.Instance.player;
        mark.SetActive(false);
        minDistance = maxDistance = 10;
        //按位取反前九层的射线检测
        //layerValue = ~(1 << 9);
        layerMask = ~(1 << 9)&~(1 << 2);
        //射线遮罩总结(第九层)
        //1.想要打开某一个层的射线检测： layerMask = 1 << 9;
        //2.想要关闭某一个层的射线检测： layerMask = ~(1 << 9);
        //3.打开所有层的射线检测： layerMask = ~(1 << 0);
        //4.打开某几层的射线检测： layerMask = (1<<10)|(1<<9);
    }

    void Update()
    {
        for (int i = 0; i < player.enemyList.Count; i++)
        {
            //防止射线自我检测
            if (player.enemyList[i]==null)
            {
                return;
            }
            hit = Physics2D.Raycast(transform.position,
                player.enemyList[i].transform.position - transform.position, 4, layerMask);
            if (hit.collider != null)
            {
                if (!hit.collider.gameObject.CompareTag("Wall"))
                {
                    distance = Vector3.Distance(transform.position,
                        player.enemyList[i].transform.position);
                    if (distance < maxDistance && distance < minDistance)
                    {
                        //在玩家视野里
                        minDistance = distance;
                        nearestEnemy = player.enemyList[i];
                    }
                }
            }
        }   
        //有目标
        if (nearestEnemy != null)
        {
            
            //mark.transform.SetParent(nearestEnemy.transform);
            //mark.transform.localPosition = Vector3.zero;
            //mark.transform.rotation = transform.rotation;
            mark.SetActive(true);
            //方向向量
            Vector3 moveDirection = nearestEnemy.transform.position - transform.position;
            if (moveDirection != Vector3.zero)
            {
                //Mathf.Rad2Deg 弧度转换角度 Mathf.Atan2 反正切函数
                float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
            }
            
        }
        else {
            mark.SetActive(false);
            transform.rotation = Quaternion.Euler(0, 0, player.moveAngle);
            
        }
        //摄像机跟随
        cameraTrans.transform.position = new Vector3(transform.position.x, transform.position.y,-3);
    }

    private void LateUpdate()
    {
        if (nearestEnemy!=null)
        {
            mark.transform.position = nearestEnemy.transform.position;
            if (hit.collider != null) {
                //丢失目标置空
                if (hit.collider.CompareTag("Wall"))
                {
                    minDistance = maxDistance = 10;
                    nearestEnemy = null;
                }
            }
        }
        else
        {
            minDistance = maxDistance = 10;
            nearestEnemy = null;
        }
        
    }
}
