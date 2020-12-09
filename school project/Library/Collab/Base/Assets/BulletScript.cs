using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameObject parent;
    public GameObject bullet;
    public GameObject hitArea;
    public Color altColor = Color.black;
    public Renderer rend;
    /*public float hspeed = 0.1f;
    public float vspeed = 0.1f;
    float gravity = 0.0001f;*/

    /*float rot;

    Vector3 Direction;*/

    void Example()
    {
        altColor.g = 0f;
        altColor.r = 0f;
        altColor.b = 0f;
        altColor.a = 0f;
    }      

    // Start is called before the first frame update
    void Start()
    {
        /*rot = Random.Range(0, 360);
        Direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * rot), 0, Mathf.Cos(Mathf.Deg2Rad * rot));*/

        Example();
        //Get the renderer of the object so we can access the color
        rend = bullet.GetComponent<Renderer>();
        //Set the initial color (0f,0f,0f,0f)
        rend.material.color = altColor;
    }

    // Update is called once per frame
    void Update()
    {
        parent.transform.Translate(Vector3.down * 1);

        altColor.r = 0.5f;
        //Assign the changed color to the material. 
        rend.material.color = altColor;
        /*vspeed -= gravity;

        parent.transform.Translate(Direction * hspeed);
        parent.transform.Translate(Vector3.up * vspeed);*/
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Surface")
        {
            var x = bullet.transform.position.x;
            var z = bullet.transform.position.z;
            CoreSystem.instance_create(x, 0, z, hitArea);
            CoreSystem.instance_create(-x, 0, -z, hitArea);
            Destroy(parent);
        }
    }
}
