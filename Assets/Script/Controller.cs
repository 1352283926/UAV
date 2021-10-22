using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>


public class Controller : MonoBehaviour
{
    public GameObject readyUAV;
    public GameObject read;
    public GameObject read2;
    public GameObject scoutUAV;
    private Transform UAV;
    private Vector3 targe;
    private Vector3 targe1;
    private bool wuSwith;//机群开关
    private bool roadSwith;//寻路开关
    private bool roadSwith3;//寻路开关
    private bool roadSwith4;//寻路开关

    int j = 0;//路径下标

    private void Start()
    {
        wuSwith = false;
        roadSwith = false;
    }
    private void Update()
    {
        //机群
        if (Input.GetKeyDown(KeyCode.Alpha1)) wuSwith = true;
        if (wuSwith)
        {
            for (int i = 0; i < 15; i++)
            {
                targe = transform.GetChild(0).GetChild(i).transform.position;
                UAV = readyUAV.transform.GetChild(i);
                UAV.gameObject.SetActive(true);
                if (Vector3.Distance(UAV.position,targe)>0.1f)//与目标点的距离
                {
                    UAV.transform.position = Vector3.MoveTowards(UAV.position, targe, 5 * Time.deltaTime);
                }
            }
        }
        //主机寻路
        if (Input.GetKeyDown(KeyCode.Alpha2)) roadSwith=true;
        if (roadSwith)
        {
            targe = read.transform.GetChild(j).transform.position;
            UAV = transform;
            if (Vector3.Distance(UAV.position, targe)>0.1f) //与目标点的距离
            {
                UAV.position = Vector3.MoveTowards(UAV.position, targe, 5 * Time.deltaTime);
            }
            else
            {
                j++;
            }
            if (j == read.transform.childCount)
            {
                print("到达");
                roadSwith = false;
                j = 0;
            }
        }
        //召唤辅机寻路
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            scoutUAV.transform.GetChild(0).gameObject.SetActive(true);

            //实例化 将预制件scoutPrefab实例给scout
            //GameObject scout = Instantiate(scoutPrefab);
            //将创建的scout放在包含scoutController组件的节点之下
            //scout.transform.parent = transform;
        }
    }
}
