using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    private bool hit = false;
    public GameObject other = null;
    public GameObject particles;

    void Start()
    {
        other = FindClosestStructure();
    }

    public GameObject FindClosestStructure()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("StructurePoint");
        GameObject closest = null;
        float distance = CameraMovement.HitAreaRadius[CameraMovement.selectedHitArea];
        Vector3 position = this.gameObject.transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    private void Update()
    {
        if (this.gameObject.tag == "HitAreaGhost")
        {
            Destroy(particles);
        }
        if ((hit == false) && (other != null))
        {
            hit = true;

            if (this.gameObject.tag == "HitArea")
            {
                if (other.gameObject.tag == "StructurePoint")
                {
                    if (other.gameObject.GetComponentInParent<BuildingScript>().destroyed == false)
                    {
                        if (this.gameObject.transform.position.z > 0)
                        {
                            if (CameraMovement.CurrentPlayer == 0)
                            {
                                CoreSystem.system_log_write("Enemy hit something at: " + this.gameObject.transform.position.x.ToString() + ", " + this.gameObject.transform.position.y.ToString() + ", " + this.gameObject.transform.position.z.ToString());
                                //CameraMovement.prev_is_hit = true;
                            }
                            if (CameraMovement.CurrentPlayer == 1)
                            {
                                CoreSystem.system_log_write("Player hit something at: " + this.gameObject.transform.position.x.ToString() + ", " + this.gameObject.transform.position.y.ToString() + ", " + this.gameObject.transform.position.z.ToString());
                            }
                        }
                        else
                        {
                            if (CameraMovement.CurrentPlayer == 0)
                            {
                                CoreSystem.system_log_write("Enemy hit something at: " + this.gameObject.transform.position.x.ToString() + ", " + this.gameObject.transform.position.y.ToString() + ", " + this.gameObject.transform.position.z.ToString());
                                CameraMovement.prev_is_hit = true;
                            }
                            if (CameraMovement.CurrentPlayer == 1)
                            {
                                CoreSystem.system_log_write("Player hit something at: " + this.gameObject.transform.position.x.ToString() + ", " + this.gameObject.transform.position.y.ToString() + ", " + this.gameObject.transform.position.z.ToString());
                            }
                        }

                        int dmg = 0;
                        if (CameraMovement.selectedHitArea == 0)
                        {
                            dmg = 100;
                        }
                        if (CameraMovement.selectedHitArea == 1)
                        {
                            dmg = 50;
                        }
                        if (CameraMovement.selectedHitArea == 2)
                        {
                            dmg = 25;
                        }
                        other.gameObject.GetComponentInParent<BuildingScript>().health -= dmg;
                        Debug.Log("Structure hit");

                        CameraMovement.CurrentPlayer = 1 + (-CameraMovement.CurrentPlayer);
                        CameraMovement.PlayerShot = false;
                        
                            CameraMovement.UpdateHitAreaGhost = true;
                            CameraMovement.CameraWaitMove = 5;
                        

                        //this.gameObject.GetComponent<Renderer>().material.SetColor("Albedo", Color.red);

                        CoreSystem.instance_create(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z, CameraMovement.HitAreaRed);

                        var x = this.gameObject.transform.position.x;
                        var z = this.gameObject.transform.position.z;

                        var xx = 0;
                        var zz = 0;
                        if (x > 0)
                        {
                            xx = 1;
                        }
                        if (x < 0)
                        {
                            xx = -1;
                        }

                        if (z > 0)
                        {
                            zz = 1;
                        }
                        if (z < 0)
                        {
                            zz = -1;
                        }
                        CoreSystem.instance_create(x - (200 * xx), this.gameObject.transform.position.y, z - (50 * zz), CameraMovement.HitAreaRed);

                        Destroy(this.gameObject);
                    }
                }
            }
        }
        if ((hit == false) && (other == null))
        {
            hit = true;

            if (this.gameObject.tag == "HitArea")
            {
                if (this.gameObject.transform.position.z < 0)
                {
                    if (CameraMovement.CurrentPlayer == 0)
                    {
                        CoreSystem.system_log_write("Enemy missed at: " + this.gameObject.transform.position.x.ToString() + ", " + this.gameObject.transform.position.y.ToString() + ", " + this.gameObject.transform.position.z.ToString());
                        //CameraMovement.prev_is_hit = false;
                    }
                    if (CameraMovement.CurrentPlayer == 1)
                    {
                        CoreSystem.system_log_write("Player missed at: " + this.gameObject.transform.position.x.ToString() + ", " + this.gameObject.transform.position.y.ToString() + ", " + this.gameObject.transform.position.z.ToString());
                    }
                }
            }
        }
    }
}
