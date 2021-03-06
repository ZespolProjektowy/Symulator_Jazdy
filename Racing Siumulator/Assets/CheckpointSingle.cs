using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckpointSingle : MonoBehaviour
{
  private TrackCheckpoint trackCheckpoint;
 private void OnTriggerEnter(Collider other)
 {
   if(other.gameObject.layer == LayerMask.NameToLayer("CarLayer")) // If this object is a car
        {
          CarController controller = other.transform.parent.parent.GetComponent<CarController>();
            trackCheckpoint.PlayerTroughCheckpoint(this, controller);
        }
        
 }

 public void SetTrackCheckpoints(TrackCheckpoint trackCheckpoint)
 {
   this.trackCheckpoint=trackCheckpoint;

 }
}