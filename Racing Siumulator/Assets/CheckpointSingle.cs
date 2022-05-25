using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckpointSingle : MonoBehaviour
{
 private void OnTriggerEnter(Collider other)
 {
   if(other.gameObject.layer == LayerMask.NameToLayer("CarLayer")) // If this object is a car
        {
            Debug.Log("checkpoint");
        }
 }
}