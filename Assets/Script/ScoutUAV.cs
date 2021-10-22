using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>


public class ScoutUAV : MonoBehaviour
{
    public GameObject body;

    public GameObject read;
    private Vector3 targe;
    int i = 1;

    private void Start()
    {
        //初始地点
        targe = read.transform.GetChild(0).transform.position;
        transform.position = targe;

    }
    private void FixedUpdate()
    {
        body.GetComponent<Rigidbody>().AddRelativeForce(0, 10, 0);

        targe = read.transform.GetChild(i).transform.position;
        if (Vector3.Distance(transform.position, targe) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position,targe, 5 * Time.deltaTime);
        }
        else
        {
            i++;
        }
        if (i == read.transform.childCount)
        {
            gameObject.SetActive(false);
            i = 0;
        }
    }
}
