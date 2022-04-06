using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public float forwardForce = 2000f;
    public float sideForce = 500f;

    public Rigidbody rb;
    // Update is called once per frame
    void FixedUpdate()
    {


        //TO DO
        //CHANGE DIRECTIONS OF THE FORCE BASED ON PLAYER NOT ENVIRONEMNT
        //ADD ROTATION OF OBJECT 
        //ALE KOGO TO OBCHODZI JAK TO KLOCEK TYLKO A NIE RAJDOWKA POKI CO
        if (Input.GetKey("s"))
        {
            rb.AddForce(forwardForce * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("w"))
        {
            rb.AddForce(-forwardForce * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("d"))
        {
            rb.AddForce(0, 0, sideForce * Time.deltaTime);
        }
        if (Input.GetKey("a"))
        {
            rb.AddForce(0, 0, sideForce * -Time.deltaTime);
        }

    }
}
