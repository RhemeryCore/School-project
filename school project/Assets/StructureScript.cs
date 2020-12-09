using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureScript : MonoBehaviour
{
    public static int selectedStructure;
    public string structureName;
    public GameObject ShootPoint;
    public GameObject Bullet;

    public float Health = 100;
    
    //int alarm = 0;

    public void SelectStructure()
    {
        selectedStructure = 1;
    }

    void Update()
    {

        /*alarm -= 1;
        if(alarm <= 0)
        {
            Vector3 pointPos = ShootPoint.transform.position;
            CoreSystem.instance_create(pointPos.x, pointPos.y, pointPos.z, Bullet);

            alarm = 60;
        }*/


    }
}
