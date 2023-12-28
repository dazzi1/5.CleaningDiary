using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 提示信息
/// </summary>
public class Instruction : MonoBehaviour
{
    private float viewDistance;
    private TextMesh textMesh;
    private float distance;
    private Transform playerTrans;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        viewDistance = 1.5f;
        playerTrans = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTrans == null) {
            return;
        }
        distance = Vector3.Distance(transform.position, playerTrans.position);
        if (distance<viewDistance)
        {
            textMesh.characterSize = 1 - distance / viewDistance;
        }
        else
        {
            textMesh.characterSize = 0;
        }
    }
}
