using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoint : MonoBehaviour
{
    private List<CheckpointSingle> checkpointSingleList;
    private int nextCheckpointSingleIndex;

    private void Awake()
    {
       Transform checkpointsTransform = transform.Find("Spline/Checkpoints");
       checkpointSingleList=new List<CheckpointSingle>();

       foreach (Transform checkpointSingleTransform in checkpointsTransform)
       {
           CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();
           checkpointSingle.SetTrackCheckpoints(this);
           checkpointSingleList.Add(checkpointSingle);
       }

       nextCheckpointSingleIndex=0;
    }

    public void PlayerTroughCheckpoint(CheckpointSingle checkpointSingle, CarController carController)
    {
        if(checkpointSingleList.IndexOf(checkpointSingle)==nextCheckpointSingleIndex)
        {
        
            //correct checkpoint
            Debug.Log("Correct");
            nextCheckpointSingleIndex = (nextCheckpointSingleIndex + 1)%checkpointSingleList.Count;
            carController.CheckpointHit();
        }
        else
        {
            //wrong checkpoint
            Debug.Log("Wrong");
        }

    }

    public void Reset()
    {
        nextCheckpointSingleIndex=0;
    }
}
