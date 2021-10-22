using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>


public class UAVOperation : MonoBehaviour
{
    public GameObject iamge;//显示UI
    public GameObject body;
    public List<GameObject> wing;
    //public GameObject floor;
    private bool bodyPostureRecover;//机身姿势恢复
    private bool wingPostureRecover;//机翼姿势恢复

    private float rotateSpeed;//螺旋翼转速
    private float upF;//升力

    private float sideF;//侧力
    private float sideFSize;//侧力改变大小
    private float sideAngleMax;//侧飞最大角度
    private float sideRotateSpeed;//侧翻速率

    private float frontF;//进力
    private float frontFSize;//进力改变大小
    private float frontAngleMax;//前飞最大角度
    private float frontRotateSpeed;//前翻速率

    private float turnAngleMax;//转向最大角度
    private float turnSpeed;//转向速率
    private float turnRotateSpeed;//螺旋翼翻转速率

    private Quaternion targetDir; //一个常量
    private void Start()
    {
        bodyPostureRecover = false;
        wingPostureRecover = false;

        upF = 0;
        sideF = 0;
        frontF = 0;

        sideFSize = 2;
        frontFSize = 5;
        
        sideAngleMax = 60;
        frontAngleMax = 20;
        turnAngleMax = 45;

        sideRotateSpeed = 15 * Time.deltaTime;
        frontRotateSpeed = 5 * Time.deltaTime;
        turnSpeed = 15 * Time.deltaTime;
        turnRotateSpeed = 30 * Time.deltaTime;
    }
    private void FixedUpdate()
    {
        body.GetComponent<Rigidbody>().AddRelativeForce(0, upF, 0);
        body.GetComponent<Rigidbody>().AddRelativeForce(sideF, 0, 0);
        body.GetComponent<Rigidbody>().AddRelativeForce(0, 0, frontF);
        if (upF>0.5f||upF<-0.5f)  WingRotation(); 
        //加力上升 
        if (Input.GetKey(KeyCode.W) && (upF < 15))
        {
            upF += 0.1f;
            rotateSpeed += 0.15f;
        }  
        //减力下降
        if (Input.GetKey(KeyCode.S)&&(upF > -15))
        {
            upF -= 0.1f;
            rotateSpeed -= 0.15f;
        }
        //悬停
        if (Input.GetKey(KeyCode.Space))
        {
            body.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            upF = 10;
            rotateSpeed = 15;
            
        }
    }

    private void Update()
    {   
        //右横飞
        if (Input.GetKey(KeyCode.RightArrow))
        {
            bodyPostureRecover = false;
            sideF = sideFSize;
            //WingRotation();
            if (Vector3.Angle(transform.up, Vector3.up)<sideAngleMax)
            {
                body.transform.Rotate(0, 0, -sideRotateSpeed, Space.Self);
            }
            
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            bodyPostureRecover = true;
            sideF = 0;
        }

        //左横飞
        if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            bodyPostureRecover = false;
            sideF = -sideFSize;
            //WingRotation();
           if (Vector3.Angle(transform.up, Vector3.up) < sideAngleMax)
            {
                body.transform.Rotate(0, 0, sideRotateSpeed, Space.Self);
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            bodyPostureRecover = true;
            sideF = 0;
        }

        //右转向
        if (Input.GetKey(KeyCode.E))
        {
            wingPostureRecover = false;
            body.transform.Rotate(0,turnSpeed, 0, Space.Self);
            //WingRotation();
            if (Vector3.Angle(wing[0].transform.up, Vector3.up) < turnAngleMax)
            {
                for (int i = 0; i < 4; i++)
                {
                    wing[i].transform.Rotate(0, 0, -turnRotateSpeed, Space.World);
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            wingPostureRecover = true;
        }


        //左转向
        if (Input.GetKey(KeyCode.Q))
        {
            wingPostureRecover = false;
            body.transform.Rotate(0, -turnSpeed, 0,Space.Self);
            //WingRotation();
            if (Vector3.Angle(wing[0].transform.up, Vector3.up) < turnAngleMax)
            {
                for (int i = 0; i < 4; i++)
                {
                    wing[i].transform.Rotate(0, 0, turnRotateSpeed, Space.World);
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            wingPostureRecover = true;
        }

        //前进
        if (Input.GetKey(KeyCode.UpArrow))
        {
            bodyPostureRecover = false;
            frontF = frontFSize;
            //WingRotation();
            if (Vector3.Angle(transform.up, Vector3.up) < frontAngleMax)
            {
                body.transform.Rotate(frontRotateSpeed, 0, 0, Space.Self);
            }
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            bodyPostureRecover = true;
            frontF = 0;
        }

        //后退
        if (Input.GetKey(KeyCode.DownArrow))
        {
            bodyPostureRecover = false;
            frontF = -frontFSize;
            //WingRotation();
            if (Vector3.Angle(transform.up, Vector3.up) < frontAngleMax)
            {
                body.transform.Rotate(-frontRotateSpeed, 0, 0, Space.Self);
            }
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            bodyPostureRecover = true;
            frontF = 0;
        }

        //机身姿态恢复
        if (bodyPostureRecover)
        {
            targetDir.eulerAngles = new Vector3(0,transform.eulerAngles.y , 0);
            body.transform.rotation = Quaternion.RotateTowards(body.transform.rotation, targetDir, 20 * Time.deltaTime);
        }
        //机翼姿态恢复
        if (wingPostureRecover)
        {
            targetDir.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            for (int i = 0; i < 4; i++)
            {
                wing[i].transform.rotation = Quaternion.RotateTowards(wing[i].transform.rotation, targetDir, 60 * Time.deltaTime);
            }
        }

        //UI显示
        iamge.GetComponent<Text>().text = "升力 " + upF+"N";
    }
    
    //机翼旋转
    private void WingRotation()
    {
        for (int i = 0; i < 4; i++)
        {
            wing[i].transform.Rotate(0, rotateSpeed, 0, Space.Self);
        }
    }


    //两物体的夹角
    /*public float GetAngle(GameObject Cylinder1)
    {
        //点乘
        float dot = Vector3.Dot(Cylinder1.transform.up, floor.transform.up);
        //弧度转角度
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        Vector3 dir = Vector3.Cross(Cylinder1.transform.up, floor.transform.up);
        angle = (Vector3.Dot(floor.transform.forward, dir) < 0 ? -1 : 1) * angle;
        Debug.Log(+angle);

        return angle;
    }*/
}


