using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionRemover : MonoBehaviour
{
    private bool hit = false;
    private bool deleted = false;
    private int timer = 60;
    private void Update()
    {
        if((hit == true) && (timer > 0))
        {
            timer--;
        }
        if((timer <= 0) && (deleted == false))
        {
            this.gameObject.GetComponent<CapsuleCollider>().isTrigger = false;
            Destroy(this.gameObject.GetComponent<CapsuleCollider>());
            Destroy(this.gameObject.GetComponent<Rigidbody>());
            deleted = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(hit == false)
        {
            
            if ((other.gameObject.tag == "Structure") && (this.gameObject.tag != "HitAreaGhost"))
            {
                hit = true;
                //this.gameObject.tag = "Visual";
            }
        }
    }
}
