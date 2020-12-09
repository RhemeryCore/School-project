using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    public float health = 100;
    public GameObject TextUI;
    public int BuildingIndex;
    public bool EnemyTag = false;
    public bool destroyed = false;

    // Update is called once per frame
    void Update()
    {
        TextUI.GetComponent<UnityEngine.UI.Text>().text = health.ToString();
        if ((health <= 0) && (destroyed == false))
        {
            instance_destroy(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Structure") && (this.gameObject.tag != "BuildingGhost"))
        {
            instance_destroy();
        }
    }

    private void instance_destroy()
    {
        if ((CameraMovement.GameState == "Preaparation") || (CameraMovement.GameState == "Battle"))
        {
            if (EnemyTag == true)
            {
                CameraMovement.BuildingEnemyPlaced[BuildingIndex]--;
            }
            else
            {
                CameraMovement.BuildingPlayerPlaced[BuildingIndex]--;
            }
        }
        Destroy(this.gameObject);
    }
    private void instance_destroy(bool t)
    {
        if(t == true)
        {
            destroyed = true;
            if (EnemyTag == true)
            {
                CameraMovement.BuildingEnemyPlaced[BuildingIndex]--;
            }
            else
            {
                CameraMovement.BuildingPlayerPlaced[BuildingIndex]--;

                CameraMovement.prev_is_hit = false;
            }
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
            this.gameObject.transform.position = new Vector3(x - (200 * xx), this.gameObject.transform.position.y, z - (50 * zz));
        }
    }
}
